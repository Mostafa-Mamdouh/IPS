import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserComponent } from './user.component';
import { UserEditorComponent } from './user-editor/user-editor.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CoreModule } from '../core/core.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SharedModule } from '../shared/shared.module';
import { UserRoutingModule } from './user-routing.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ForgetPasswordComponent } from './forget-password/forget-password.component';



@NgModule({
  declarations: [UserComponent, UserEditorComponent, UserDetailComponent, ChangePasswordComponent, ForgetPasswordComponent],
  imports: [
    CommonModule,
    SharedModule,
    UserRoutingModule,
    CoreModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ]
})
export class UserModule { }
