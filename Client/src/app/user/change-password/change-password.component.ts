import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IChangePassword, IUser } from 'src/app/shared/models/user';
import { UserService } from '../user.service';
import { ConfirmedValidator } from './old-pwd.validators';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
})
export class ChangePasswordComponent implements OnInit {
  changePasswordEditor: IChangePassword;
  userId = 0;
  userForm: FormGroup;
  user: IUser;
  token: string = null;
  isReset: string = 'false';
  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.isReset = this.activatedroute.snapshot.paramMap.get('isReset');
    this.token = this.activatedroute.snapshot.paramMap.get('token');
    this.userId = +this.activatedroute.snapshot.paramMap.get('id');

    this.createUserForm();

    if (this.isReset == 'True') {
      this.userForm.controls['oldPwd'].setValidators(null);
      this.userForm.controls['oldPwd'].updateValueAndValidity();
    }
  }

  createUserForm() {
    this.userForm = this.formBuilder.group(
      {
        oldPwd: ['', [Validators.required]],
        newPwd: [
          '',
          [
            Validators.required,
            Validators.pattern('((?=.*d)(?=.*[a-z])(?=.*[A-Z]).{8,30})'),
          ],
        ],
        confirmPwd: ['', Validators.required],
      },
      {
        validator: ConfirmedValidator('newPwd', 'confirmPwd'),
      }
    );
  }

  collectFormData() {
    const model: IChangePassword = {
      id: +this.activatedroute.snapshot.paramMap.get('id'),
      oldPassword: this.userForm.controls['oldPwd'].value,
      newPassword: this.userForm.controls['newPwd'].value,
      token: this.token,
    };
    this.changePasswordEditor = model;
  }

  onSubmit() {
    this.markFormGroupTouched(this.userForm);
    if (this.userForm.valid) {
      this.collectFormData();
      this.changeUserPassword();
    }
  }
  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control) => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  changeUserPassword() {
    this.userService.changePasswors(this.changePasswordEditor).subscribe(
      (res) => {
        if (res.emailConfirmed) {
          this.toastr.success(`Email Confirmed Successfully`);
          this.router.navigateByUrl('/account/login', {
            skipLocationChange: true,
          });
        }
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
