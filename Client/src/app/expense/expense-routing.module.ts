import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { ExpenseEditorComponent } from './expense-editor/expense-editor.component';
import { ExpenseComponent } from './expense.component';





const routes: Routes = [
  {path: '', component: ExpenseComponent,canActivate: [AuthGuard],data:{  
      Permission: 'Expenses:List/View Expenses'}},
  {path: 'add', component: ExpenseEditorComponent,canActivate: [AuthGuard],data: { breadcrumb: 'Add',
      Permission: 'Expenses:Add Expense' }},
  {path: 'edit/:id', component: ExpenseEditorComponent ,canActivate: [AuthGuard],data: { breadcrumb: 'Edit',
      Permission: 'Expenses:Edit Expense' }},
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExpenseRoutingModule {}
