import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, Inject, Input, OnInit } from '@angular/core';
import * as moment from 'moment';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import {
  invoiceType,
  paymentMethod,
  paymentType,
} from 'src/app/shared/models/Enum';
import {
  IInvoicePayment,
  IInvoicePaymentEditor,
} from 'src/app/shared/models/invoice';
import { Ilookup } from 'src/app/shared/models/lookup';
import { IPayment } from 'src/app/shared/models/payment';
import { InvoiceService } from '../invoice.service';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-payment-editor',
  templateUrl: './payment-editor.component.html',
  styleUrls: ['./payment-editor.component.scss'],
})
export class PaymentEditorComponent implements OnInit {
  @Input() payment: IPayment;
  @Input() modalRef: BsModalRef;
  @Input() invoicePayment: IInvoicePayment = null;
  maxPaymentDate: Date;

  InvoicePayment: IInvoicePaymentEditor;
  paymentForm: FormGroup;
  paymentMethod: Ilookup[] = [];
  paymentType: Ilookup[] = [];
  isBankOrCheque: boolean = false;
  chequeNumber: string;
  transferNumber: string;
  paymentArchiveId: number = null;
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
    this.paymentMethod.push(
      { id: paymentMethod.Cash, name: paymentMethod[paymentMethod.Cash] },
      { id: paymentMethod.Cheque, name: paymentMethod[paymentMethod.Cheque] },
      {
        id: paymentMethod.BankTransfer,
        name: paymentMethod[paymentMethod.BankTransfer],
      }
    );
    this.paymentType.push(
      { id: paymentType.Deposit, name: paymentType[paymentType.Deposit] },
      {
        id: paymentType.ActualPayment,
        name: paymentType[paymentType.ActualPayment],
      }
    );
    this.createPaymentForm();

    var maxPaymentDatefromMomentObject = moment(
      this.payment.invoiceDate,
      'yyyy-MM-DD'
    );
    this.maxPaymentDate = maxPaymentDatefromMomentObject.toDate();

    if (this.invoicePayment) {
      this.assignFormData();
    }
  }

  assignFormData() {
    if (this.invoicePayment) {
      this.paymentForm.controls['id'].setValue(this.invoicePayment.id);
      this.paymentForm.controls['purchaseInvoiceId'].setValue(
        this.invoicePayment.purchaseInvoiceId
      );
      this.paymentForm.controls['salesInvoiceId'].setValue(
        this.invoicePayment.salesInvoiceId
      );

      var datefromMomentObject = moment(
        this.invoicePayment.paymentDate,
        'yyyy-MM-DD'
      );
      this.paymentForm.controls['paymentDate'].setValue(
        datefromMomentObject.toDate()
      );

      if (this.invoicePayment.type == paymentType.Deposit) {
        this.paymentForm.controls['type'].patchValue({
          id: this.invoicePayment.type,
          name: paymentType[paymentType.Deposit],
        });
      } else {
        this.paymentForm.controls['type'].patchValue({
          id: this.invoicePayment.type,
          name: paymentType[paymentType.ActualPayment],
        });
      }
      this.paymentForm.controls['amount'].setValue(this.invoicePayment.amount);
      this.paymentForm.controls['amount'].setValidators(
        Validators.max(this.payment.remaingAmount + this.invoicePayment.amount)
      );
      if (this.invoicePayment.paymentMethod == paymentMethod.Cash) {
        this.paymentForm.controls['paymentMethod'].patchValue({
          id: this.invoicePayment.paymentMethod,
          name: paymentMethod[paymentMethod.Cash],
        });
      } else if (this.invoicePayment.paymentMethod == paymentMethod.Cheque) {
        this.paymentForm.controls['paymentMethod'].patchValue({
          id: this.invoicePayment.paymentMethod,
          name: paymentMethod[paymentMethod.Cheque],
        });
      } else {
        this.paymentForm.controls['paymentMethod'].patchValue({
          id: this.invoicePayment.paymentMethod,
          name: paymentMethod[paymentMethod.BankTransfer],
        });
      }

      this.paymentForm.controls['archiveId'].setValue(
        this.invoicePayment.archiveId
      );

      this.onChangePaymentMethod();
      this.paymentForm.controls['transactionNumber'].setValue(
        this.invoicePayment.paymentMethod == paymentMethod.Cheque
          ? this.invoicePayment.chequeNumber
          : this.invoicePayment.paymentMethod == paymentMethod.BankTransfer
          ? this.invoicePayment.transferNumber
          : ''
      );
    }
  }

  collectFormData() {
    const model: IInvoicePaymentEditor = {
      id: this.paymentForm.controls['id'].value,
      purchaseInvoiceId: this.paymentForm.controls['purchaseInvoiceId'].value,
      salesInvoiceId: this.paymentForm.controls['salesInvoiceId'].value,
      type: this.paymentForm.controls['type'].value.id,
      amount: this.paymentForm.controls['amount'].value,
      paymentDate: this.paymentForm.controls['paymentDate'].value,
      paymentMethod: this.paymentForm.controls['paymentMethod'].value.id,
      chequeNumber:
        this.paymentForm.controls['paymentMethod'].value.id == 2
          ? this.paymentForm.controls['transactionNumber'].value
          : null,
      transferNumber:
        this.paymentForm.controls['paymentMethod'].value.id == 3
          ? this.paymentForm.controls['transactionNumber'].value
          : null,
      archiveId: this.paymentForm.controls['archiveId'].value,
    };
    this.InvoicePayment = model;
  }

  onChangePaymentMethod() {
    var paymentMethodId = this.paymentForm.controls['paymentMethod'].value.id;

    if (
      paymentMethodId == paymentMethod.BankTransfer ||
      paymentMethodId == paymentMethod.Cheque
    ) {
      this.paymentForm.controls['transactionNumber'].setValidators(
        Validators.required
      );
      this.paymentForm.controls['transactionNumber'].updateValueAndValidity();
      this.isBankOrCheque = true;
    } else {
      this.paymentForm.controls['transactionNumber'].setValidators(null);
      this.paymentForm.controls['transactionNumber'].updateValueAndValidity();
      this.isBankOrCheque = false;
    }
  }

  onChangePaymentType(){
        var paymentTypeId = this.paymentForm.controls['type'].value.id;
        if(paymentTypeId==paymentType.ActualPayment && !this.invoicePayment ){
          this.paymentForm.controls['amount'].setValue(this.payment.remaingAmount);
          this.paymentForm.controls['amount'].updateValueAndValidity();
        }
        else{
           this.paymentForm.controls['amount'].setValue(null);
          this.paymentForm.controls['amount'].updateValueAndValidity();
        }
  }
  createPaymentForm() {
    this.paymentForm = this.formBuilder.group({
      id: new FormControl(0),
      purchaseInvoiceId: new FormControl(this.payment.purchaseInvoiceId),
      salesInvoiceId: new FormControl(this.payment.salesInvoiceId),
      type: new FormControl(null, [Validators.required]),
      amount: new FormControl(null, [
        Validators.required,
        Validators.max(this.payment.remaingAmount),
        Validators.min(0),
      ]),
      paymentDate: new FormControl('', [Validators.required]),
      paymentMethod: new FormControl(null, [Validators.required]),
      transactionNumber: new FormControl(''),
      archiveId: new FormControl(null, [Validators.required]),
    });
  }

  onSubmit() {
    this.markFormGroupTouched(this.paymentForm);

    if (this.paymentForm.valid) {
      this.collectFormData();
      this.savePayment();
    }
  }
  savePayment() {
    this.invoiceService
      .savePayment(
        this.payment.invoiceType,
        this.InvoicePayment,
        this.invoicePayment ? this.invoicePayment.id : 0
      )
      .subscribe(
        () => {
          this.modalRef.hide();
          this.toastr.success('invoice payment has been saved successfully');
          setTimeout(() => {
            this.reloadCurrentPage();
          }, 2000);
        },
        (err) => {
          console.log(err);
        }
      );
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control) => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  reloadCurrentPage() {
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }
  attachArchieveId(e) {
    this.paymentForm.controls['archiveId'].setValue(e);
    this.paymentForm.controls['archiveId'].updateValueAndValidity();
  }
}
