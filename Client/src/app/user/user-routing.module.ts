import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ForgetPasswordComponent } from './forget-password/forget-password.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserEditorComponent } from './user-editor/user-editor.component';
import { UserComponent } from './user.component';

const routes: Routes = [
  {
    path: '',
    component: UserComponent,
    canActivate: [AuthGuard],
    data: {
      Permission: 'Users:List/View Users',
    },
  },
  {
    path: 'add',
    component: UserEditorComponent,
    canActivate: [AuthGuard],
    data: { breadcrumb: 'Add', Permission: 'Users:Add User' },
  },
  {
    path: 'edit/:id',
    component: UserEditorComponent,
    canActivate: [AuthGuard],
    data: { breadcrumb: 'Edit', Permission: 'Users:Edit User' },
  },
  {
    path: 'view/:id',
    component: UserDetailComponent,
    canActivate: [AuthGuard],
    data: { breadcrumb: 'View', Permission: 'Users:List/View Users' },
  },
  {
    path: 'change-password/:token/:id/:isReset',
    component: ChangePasswordComponent,
    data: { breadcrumb: 'Change-Password' },
  },
  {
    path: 'reset-password',
    component: ForgetPasswordComponent,
    data: { breadcrumb: 'Reset-Password' },
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
