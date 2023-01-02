import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { CreditEditorComponent } from './credit-editor/credit-editor.component';
import { CreditComponent } from './credit/credit.component';

const routes: Routes = [
  {
    path: '',
    component: CreditComponent,
    data: {
      Permission: ['Transaction:List/View Transactions'],
    },
    canActivate: [AuthGuard],
  },
  {
    path: 'edit',
    component: CreditEditorComponent,
    canActivate: [AuthGuard],
    data: {
      breadcrumb: 'Update',
      Permission: ['Transaction:List/View Transactions'],
    },
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreditRoutingModule {}
