import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IResetPassword } from 'src/app/shared/models/user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.scss']
})
export class ForgetPasswordComponent implements OnInit {

  userForm: FormGroup;
  resetPasswordEditor: IResetPassword
  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {

    this.createUserForm();

  
  }

  createUserForm() {
    this.userForm = this.formBuilder.group({
      email: new FormControl('', [
        Validators.required,
        Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$'),
      ]),
    
    });
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
  collectFormData() {
    const model: IResetPassword = {
      email:this.userForm.controls['email'].value
    };
    this.resetPasswordEditor = model;
  }
  changeUserPassword() {

    this.userService.resetPassword(this.resetPasswordEditor).subscribe(
      (res) => {
        if(res.userId){
          this.toastr.success(`The link has been sent , please check your email to reset your password.`);
          this.router.navigateByUrl('/account/login',{skipLocationChange: true});
        }
   
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
