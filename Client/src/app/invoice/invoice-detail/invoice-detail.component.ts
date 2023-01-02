import { DOCUMENT } from '@angular/common';
import {
  Component,
  ElementRef,
  Inject,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { invoiceType } from 'src/app/shared/models/Enum';
import {
  IInvoice,
  IInvoicePayment,
  IInvoiceProduct,
} from 'src/app/shared/models/invoice';
import { IPayment } from 'src/app/shared/models/payment';
import { InvoiceService } from '../invoice.service';
import { PrintInvoiceComponent } from '../print-invoice/print-invoice.component';
import * as FileSaver from 'file-saver';
@Component({
  selector: 'app-invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.scss'],
})
export class InvoiceDetailComponent implements OnInit {
  invoice: IInvoice;
  invoiceType: invoiceType;
  totalInvoiceAmount: number = 0;
  invoiceProducts: string;
  invoiceServices: string;
  payment: IPayment = new IPayment();
  invoicePaymentItems: IInvoicePayment[] = [];
  deleteinvoicemodalRef: BsModalRef;
  invoiceProductItems: IInvoiceProduct[] = [];
  @ViewChild(PrintInvoiceComponent, { read: ElementRef })
  PrintComponent: ElementRef;
  private PrintTemplateTpl: TemplateRef<any>;
  constructor(
    private invoiceService: InvoiceService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute,
    private modalService: BsModalService,
    @Inject(DOCUMENT) private _document: Document
  ) {}

  ngOnInit(): void {
    this.activatedroute.data.subscribe((data) => {
      this.invoiceType = data.type;
    });
    this.loadInvoice();
  }

  loadInvoice() {
    this.invoiceService
      .getInvoice(
        this.invoiceType,
        +this.activatedroute.snapshot.paramMap.get('id')
      )
      .subscribe(
        (res) => {
          this.invoice = res;
          this.payment.purchaseInvoiceId =
            this.invoiceType == invoiceType.Purchase ? this.invoice.id : null;
          this.payment.salesInvoiceId =
            this.invoiceType == invoiceType.Sales ? this.invoice.id : null;
          this.payment.invoiceType = this.invoiceType;
          this.payment.remaingAmount =
            this.invoice.totalInvoice - this.invoice.totalPaid;
          this.payment.invoiceDate = this.invoice.invoiceDate;
          this.payment.invoiceCreatedBy = this.invoice.createdBy;
          this.payment.deleted = this.invoice.deleted;

          this.invoiceProductItems = this.invoice.invoiceProducts;
          this.invoicePaymentItems = this.invoice.invoicePayments;
          this.invoiceProducts = this.invoiceProductItems
            .map((x) => x.productName)
            .join(' ');
          this.invoiceServices = this.invoiceProductItems
            .map((x) => x.serviceName)
            .join(' ');
          this.totalInvoiceAmount = this.invoiceProductItems
            .map((a) => a.price * a.quantity)
            .reduce(function (a, b) {
              return a + b;
            });
        },
        (err) => {
          console.log(err.message);
        }
      );
  }
  openDeleteInvoiceModal(template: TemplateRef<any>) {
    this.deleteinvoicemodalRef = this.modalService.show(template);
  }

  deleteInvoice(id: number) {
    this.invoiceService.deleteInvoice(this.invoiceType, id).subscribe(
      () => {
        this.deleteinvoicemodalRef.hide();
        this.toastr.success('invoice has been cancelled successfully');
        setTimeout(() => {
          this.reloadCurrentPage();
        }, 2000);
      },
      (err) => {
        console.log(err);
      }
    );
  }
  reloadCurrentPage() {
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }

  downloadAttachment(filePath: string, fileName: string) {
    var file = fileName;
    this.invoiceService.downloadFile(filePath).subscribe(
      (data: Blob) => {
        var fileName = file.replace(
          file.substring(file.indexOf('_attach'), file.lastIndexOf('.')),
          ''
        );
        FileSaver.saveAs(data, fileName);
      },
      (err: any) => {
        console.log(`Unable to download file`);
      }
    );
  }
}
