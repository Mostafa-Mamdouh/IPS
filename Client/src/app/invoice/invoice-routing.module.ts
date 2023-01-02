import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { InvoiceDetailComponent } from './invoice-detail/invoice-detail.component';
import { InvoiceEditorComponent } from './invoice-editor/invoice-editor.component';
import { InvoiceComponent } from './invoice.component';



const routes: Routes = [
  {path: '', component: InvoiceComponent,canActivate: [AuthGuard],data:{ 
      Permission: ['Purchasing:List/View Purchasing Invoices','Sales:List/View Sales Invoices']}},
  {path: 'add', component: InvoiceEditorComponent,canActivate: [AuthGuard], data: { breadcrumb: 'Add Invoice',
      Permission: ['Purchasing:Add Purchasing Invoice','Sales:Add Sales Invoice']}},
  {path: 'edit/:id', component: InvoiceEditorComponent,canActivate: [AuthGuard] , data: { breadcrumb: 'Edit Invoice' ,
      Permission: ['Purchasing:Edit Sales Invoice','Sales:Edit Sales Invoice']}},
  {path: 'view/:id', component: InvoiceDetailComponent,canActivate: [AuthGuard] , data: { breadcrumb: 'View Invoice',
      Permission: ['Purchasing:List/View Purchasing Invoices','Sales:List/View Sales Invoices'] }},
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InvoiceRoutingModule {}
