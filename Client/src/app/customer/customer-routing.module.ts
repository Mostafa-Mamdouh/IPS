import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { CustomerEditorComponent } from './customer-editor/customer-editor.component';
import { CustomerComponent } from './customer.component';




const routes: Routes = [
  {path: '', component: CustomerComponent ,data:{
      Permission: ['Clients:List/View Clients','Suppliers:List/View Suppliers']},canActivate: [AuthGuard]},
  {path: 'add', component: CustomerEditorComponent,canActivate: [AuthGuard],data: { breadcrumb: 'Add',
      Permission: ['Clients:Add Client','Suppliers:Add Supplier']}},
  {path: 'edit/:id', component: CustomerEditorComponent,canActivate: [AuthGuard] ,data: { breadcrumb: 'Edit' ,
      Permission: ['Clients:Edit Client','Suppliers:Edit Supplier']}},
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CustomerRoutingModule {}
