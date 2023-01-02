import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceComponent } from './invoice.component';
import { InvoiceRoutingModule } from './invoice-routing.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { InvoiceEditorComponent } from './invoice-editor/invoice-editor.component';
import { InvoiceDetailComponent } from './invoice-detail/invoice-detail.component';
import { PaymentEditorComponent } from './payment-editor/payment-editor.component';
import { PaymentComponent } from './payment/payment.component';
import { PrintInvoiceComponent } from './print-invoice/print-invoice.component';





@NgModule({
  declarations: [InvoiceComponent, InvoiceEditorComponent, InvoiceDetailComponent, PaymentEditorComponent, PaymentComponent, PrintInvoiceComponent],
  imports: [
    CommonModule,
    CoreModule,
    InvoiceRoutingModule,
    SharedModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ]
})
export class InvoiceModule { }
