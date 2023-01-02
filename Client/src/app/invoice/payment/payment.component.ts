import { DOCUMENT } from '@angular/common';
import { Component, Inject, Input, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import {
  IInvoicePayment,
  IInvoicePaymentEditor,
} from 'src/app/shared/models/invoice';
import { Ilookup } from 'src/app/shared/models/lookup';
import { IPayment } from 'src/app/shared/models/payment';
import { InvoiceService } from '../invoice.service';
import * as FileSaver from 'file-saver';
@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss'],
})
export class PaymentComponent implements OnInit {
  @Input() payments: IInvoicePayment[];
  @Input() payment: IPayment;
  @Input() modalRef: BsModalRef;

  deletemodalRef: BsModalRef;

  InvoicePayment: IInvoicePaymentEditor;
  paymentForm: FormGroup;
  paymentMethod: Ilookup[] = [];
  paymentType: Ilookup[] = [];
  isBankOrCheque: boolean = false;

  constructor(
    private invoiceService: InvoiceService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute,
    private modalService: BsModalService,
    @Inject(DOCUMENT) private _document: Document
  ) {}

  ngOnInit(): void {}

  openModal(
    template: TemplateRef<any>,
    remaingAmount: number,
    invoicedate: Date,
    invoiceId: number
  ) {
    this.modalRef = this.modalService.show(template);
  }

  openDeleteModal(template: TemplateRef<any>) {
    this.deletemodalRef = this.modalService.show(template);
  }
  deletePayment(id: number) {
    this.invoiceService.deletePayment(this.payment.invoiceType, id).subscribe(
      () => {
        this.deletemodalRef.hide();
        this.toastr.success('invoice payment has been deleted successfully');
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
