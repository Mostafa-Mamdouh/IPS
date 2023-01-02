import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { expenseType, paymentMethod } from 'src/app/shared/models/Enum';
import { IExpense, IExpenseEditor } from 'src/app/shared/models/expense';
import { Ilookup } from 'src/app/shared/models/lookup';
import { ExpenseService } from '../expense.service';

@Component({
  selector: 'app-expense-editor',
  templateUrl: './expense-editor.component.html',
  styleUrls: ['./expense-editor.component.scss'],
})
export class ExpenseEditorComponent implements OnInit {
  expenseForm: FormGroup;
  expenseEditor: IExpenseEditor;
  expenseType: expenseType;
  expenseId = 0;
  expenseTypes: Ilookup[];
  paymentMethod: Ilookup[] = [];
  expense: IExpense;
  isBankOrCheque: boolean = false;
  constructor(
    private expenseService: ExpenseService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
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
    this.expenseId = +this.activatedroute.snapshot.paramMap.get('id');
    this.createExpenseForm();
    this.getExpenseTypesLookup();
  }

  createExpenseForm() {
    this.expenseForm = this.formBuilder.group({
      id: new FormControl(0),
      description: new FormControl(''),
      expenseType: new FormControl(null, [Validators.required]),
      transactionDate: new FormControl('', [Validators.required]),
      amount: new FormControl(null, [Validators.required, Validators.min(0)]),
      paymentMethod: new FormControl(null, [Validators.required]),
      transactionNumber: new FormControl(''),
    });
  }

  collectFormData() {
    const model: IExpenseEditor = {
      id: this.expenseForm.controls['id'].value,
      description: this.expenseForm.controls['description'].value,
      amount: this.expenseForm.controls['amount'].value,
      transactionDate: this.expenseForm.controls['transactionDate'].value,
      paymentMethod: this.expenseForm.controls['paymentMethod'].value.id,
      chequeNumber:
        this.expenseForm.controls['paymentMethod'].value.id == 2
          ? this.expenseForm.controls['transactionNumber'].value
          : null,
      transferNumber:
        this.expenseForm.controls['paymentMethod'].value.id == 3
          ? this.expenseForm.controls['transactionNumber'].value
          : null,
      expenseTypeId: this.expenseForm.controls['expenseType'].value.id,
    };
    this.expenseEditor = model;
  }
  assignFormData(expense: IExpense) {
    this.expenseForm.controls['id'].setValue(expense.id);
    this.expenseForm.controls['description'].setValue(expense.description);
    var datefromMomentObject = moment(expense.transactionDate, 'yyyy-MM-DD');
    this.expenseForm.controls['transactionDate'].setValue(
      datefromMomentObject.toDate()
    );

    this.expenseForm.controls['expenseType'].patchValue(
      this.expenseTypes.find((x) => x.id == expense.expenseTypeId)
    );
    this.expenseForm.controls['amount'].setValue(expense.amount);
    if (expense.paymentMethod == paymentMethod.Cash) {
      this.expenseForm.controls['paymentMethod'].patchValue({
        id: expense.paymentMethod,
        name: paymentMethod[paymentMethod.Cash],
      });
    } else if (expense.paymentMethod == paymentMethod.Cheque) {
      this.expenseForm.controls['paymentMethod'].patchValue({
        id: expense.paymentMethod,
        name: paymentMethod[paymentMethod.Cheque],
      });
    } else {
      this.expenseForm.controls['paymentMethod'].patchValue({
        id: expense.paymentMethod,
        name: paymentMethod[paymentMethod.BankTransfer],
      });
    }
    this.onChangePaymentMethod();
    this.expenseForm.controls['transactionNumber'].setValue(
      expense.paymentMethod == paymentMethod.Cheque
        ? expense.chequeNumber
        : expense.paymentMethod == paymentMethod.BankTransfer
        ? expense.transferNumber
        : ''
    );
  }
  loadExpense() {
    this.expenseService
      .getExpenseById(+this.activatedroute.snapshot.paramMap.get('id'))
      .subscribe(
        (res) => {
          this.expense = res;
          if (this.expense) this.assignFormData(this.expense);
        },
        (err) => {
          console.log(err.message);
        }
      );
  }
  onSubmit() {
    this.markFormGroupTouched(this.expenseForm);

    if (this.expenseForm.valid) {
      this.collectFormData();
      this.saveExpense();
    }
  }

  onChangePaymentMethod() {
    var paymentMethodId = this.expenseForm.controls['paymentMethod'].value.id;

    if (
      paymentMethodId == paymentMethod.BankTransfer ||
      paymentMethodId == paymentMethod.Cheque
    ) {
      this.expenseForm.controls['transactionNumber'].setValidators(
        Validators.required
      );
      this.expenseForm.controls['transactionNumber'].updateValueAndValidity();
      this.isBankOrCheque = true;
    } else {
      this.expenseForm.controls['transactionNumber'].setValidators(null);
      this.expenseForm.controls['transactionNumber'].updateValueAndValidity();
      this.isBankOrCheque = false;
    }
  }

  saveExpense() {
    this.expenseService
      .saveExpense(this.expenseEditor, this.expenseId)
      .subscribe(
        (res) => {
          this.toastr.success('transaction saved successfully');
          this.router.navigateByUrl(`/expenses`);
        },
        (err) => {
          console.log(err);
        }
      );
  }

  getExpenseTypesLookup() {
    this.expenseService.getExpenseTypesLookup().subscribe(
      (res) => {
        this.expenseTypes = res;
        if (this.expenseId > 0) {
          this.loadExpense();
        }
      },
      (error) => {
        console.log(error);
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
}
