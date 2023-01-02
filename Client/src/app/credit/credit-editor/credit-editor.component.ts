import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ICredit } from 'src/app/shared/models/credit';
import { creditType } from 'src/app/shared/models/Enum';
import { CreditService } from '../credit.service';

@Component({
  selector: 'app-credit-editor',
  templateUrl: './credit-editor.component.html',
  styleUrls: ['./credit-editor.component.scss'],
})
export class CreditEditorComponent implements OnInit {
  creditId: number = 1;
  creditForm: FormGroup;
  creditEditor: ICredit;
  credit: ICredit;

  constructor(
    private creditService: CreditService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.createCreditForm();
    this.loadCredit();
  }

  createCreditForm() {
    this.creditForm = this.formBuilder.group({
      id: new FormControl(creditType.currentCredit),
      bankCredit: new FormControl(0, [Validators.required]),
      cashCredit: new FormControl(0, [Validators.required]),
    });
  }

  collectFormData() {
    const model: ICredit = {
      id: this.creditForm.controls['id'].value,
      bankCredit: this.creditForm.controls['bankCredit'].value,
      cashCredit: this.creditForm.controls['cashCredit'].value,
      createDate: null,
      createdBy: null,
    };
    this.creditEditor = model;
  }
  assignFormData(credit: ICredit) {
    this.creditForm.controls['id'].setValue(credit.id);
    this.creditForm.controls['bankCredit'].setValue(credit.bankCredit);
    this.creditForm.controls['cashCredit'].setValue(credit.cashCredit);
  }
  loadCredit() {
    this.creditService.getCurrentCredit().subscribe(
      (res) => {
        this.credit = res;
        this.assignFormData(this.credit);
      },
      (err) => {
        console.log(err.message);
      }
    );
  }

  onSubmit() {
    this.markFormGroupTouched(this.creditForm);
    if (this.creditForm.valid) {
      this.collectFormData();
      this.saveCredit();
    }
  }
  saveCredit() {
    this.creditService.saveCredit(this.creditEditor).subscribe(
      (res) => {
        this.toastr.success('credit saved successfully');
        this.router.navigateByUrl(`/transaction`);
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
}
