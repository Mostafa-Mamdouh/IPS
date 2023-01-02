import { DOCUMENT } from '@angular/common';
import {
  Component,
  ElementRef,
  Inject,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import * as moment from 'moment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { invoiceType } from '../shared/models/Enum';
import { IInvoiceList } from '../shared/models/invoice';
import { InvoiceParams } from '../shared/models/invoice-params';
import { IPayment } from '../shared/models/payment';
import { InvoiceService } from './invoice.service';

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.scss'],
})
export class InvoiceComponent implements OnInit {
  public totalItems = 64;
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate', { static: false }) fromDateTerm: ElementRef;
  @ViewChild('toDate', { static: false }) toDateTerm: ElementRef;

  modalRef: BsModalRef;
  deletemodalRef: BsModalRef;

  payment: IPayment = new IPayment();
  invoices: IInvoiceList[] = [];
  invoiceParams = new InvoiceParams();
  exportInvoiceParams = new InvoiceParams();

  totalCount: number;
  invoiceType: invoiceType;

  constructor(
    private invoiceService: InvoiceService,
    private activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router,
    @Inject(DOCUMENT) private _document: Document
  ) {}

  ngOnInit(): void {
    this.activatedroute.data.subscribe((data) => {
      this.invoiceType = data.type;
    });
    this.getInvoices();
  }

  getInvoices() {
    this.invoiceService
      .getInvoices(this.invoiceType, this.invoiceParams)
      .subscribe(
        (res) => {
          this.invoices = res.data;
          this.invoiceParams.pageNumber = res.pageIndex;
          this.invoiceParams.pageSize = res.pageSize;
          this.totalCount = res.count;
        },
        (error) => {
          console.log(error);
        }
      );
  }
  onPageChanged(event: any) {
    if (this.invoiceParams.pageNumber !== event) {
      this.invoiceParams.pageNumber = event;
      this.getInvoices();
    }
  }
  onSearch() {
    this.invoiceParams.search = this.searchTerm.nativeElement.value;
    this.invoiceParams.pageNumber = 1;
    this.getInvoices();
  }
  onFilter() {
    var invoiceFromDate = this.fromDateTerm.nativeElement.value;
    var invoiceToDate = this.toDateTerm.nativeElement.value;

    if (invoiceFromDate && invoiceToDate && invoiceFromDate > invoiceToDate) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (invoiceFromDate) {
        var datefromMomentObject = moment(invoiceFromDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.invoiceParams.invoiceFromDate = datefromMomentObject.toDate();
      } else {
        this.invoiceParams.invoiceFromDate = invoiceFromDate;
      }

      if (invoiceToDate) {
        var datetoMomentObject = moment(invoiceToDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.invoiceParams.invoiceToDate = datetoMomentObject.toDate();
      } else {
        this.invoiceParams.invoiceToDate = invoiceToDate;
      }
      this.invoiceParams.pageNumber = 1;

      this.getInvoices();
    }
  }

  Export() {
    var invoiceFromDate = this.fromDateTerm.nativeElement.value;
    var invoiceToDate = this.toDateTerm.nativeElement.value;

    if (invoiceFromDate && invoiceToDate && invoiceFromDate > invoiceToDate) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (invoiceFromDate) {
        var datefromMomentObject = moment(invoiceFromDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.exportInvoiceParams.invoiceFromDate =
          datefromMomentObject.toDate();
      } else {
        this.exportInvoiceParams.invoiceFromDate = invoiceFromDate;
      }

      if (invoiceToDate) {
        var datetoMomentObject = moment(invoiceToDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.exportInvoiceParams.invoiceToDate = datetoMomentObject.toDate();
      } else {
        this.exportInvoiceParams.invoiceToDate = invoiceToDate;
      }

      this.exportInvoiceParams.pageNumber = 1;
      this.exportInvoiceParams.pageSize = 1000000;

      this.invoiceService
        .exportInvoices(this.invoiceType, this.exportInvoiceParams)
        .subscribe(
          (data: Blob) => {
            var fileName =
              this.invoiceType == invoiceType.Purchase
                ? 'Purchasing_Invoices_list_' + Date.now() + ''
                : 'Sales_Invoices_list_' + Date.now() + '';
            FileSaver.saveAs(data, fileName);
          },
          (err: any) => {
            console.log(`Unable to export file`);
          }
        );
    }
  }

  GenerateReport() {
    var invoiceFromDate = this.fromDateTerm.nativeElement.value;
    var invoiceToDate = this.toDateTerm.nativeElement.value;

    if (invoiceFromDate && invoiceToDate && invoiceFromDate > invoiceToDate) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (invoiceFromDate) {
        var datefromMomentObject = moment(invoiceFromDate, 'DD/MM/YYYY');
        this.exportInvoiceParams.invoiceFromDate =
          datefromMomentObject.toDate();
      } else {
        this.exportInvoiceParams.invoiceFromDate = invoiceFromDate;
      }
      if (invoiceToDate) {
        var datetoMomentObject = moment(invoiceToDate, 'DD/MM/YYYY');
        this.exportInvoiceParams.invoiceToDate = datetoMomentObject.toDate();
      } else {
        this.exportInvoiceParams.invoiceToDate = invoiceToDate;
      }
      this.exportInvoiceParams.pageNumber = 1;
      this.exportInvoiceParams.pageSize = 1000000;

      this.invoiceService
        .GenerateInvoiceReport(this.invoiceType, this.exportInvoiceParams)
        .subscribe(
          (data: Blob) => {
            var fileName =
              this.invoiceType == invoiceType.Purchase
                ? 'Purchasing_Invoices_Report_' + Date.now() + ''
                : 'Sales_Invoices_Report_' + Date.now() + '';
            FileSaver.saveAs(data, fileName);
          },
          (err: any) => {
            console.log(`Unable to export file`);
          }
        );
    }
  }

  openModal(
    template: TemplateRef<any>,
    remaingAmount: number,
    invoicedate: Date,
    invoiceId: number
  ) {
    this.payment.remaingAmount = remaingAmount;
    this.payment.invoiceDate = new Date(invoicedate);
    this.payment.invoiceType = this.invoiceType;
    this.payment.purchaseInvoiceId =
      this.invoiceType == invoiceType.Purchase ? invoiceId : null;
    this.payment.salesInvoiceId =
      this.invoiceType == invoiceType.Sales ? invoiceId : null;

    this.modalRef = this.modalService.show(template);
  }
  openDeleteModal(template: TemplateRef<any>) {
    this.deletemodalRef = this.modalService.show(template);
  }

  deleteInvoice(id: number) {
    this.invoiceService.deleteInvoice(this.invoiceType, id).subscribe(
      () => {
        this.deletemodalRef.hide();
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
}
