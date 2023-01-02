import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerComponent } from './customer.component';
import { CustomerEditorComponent } from './customer-editor/customer-editor.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CoreModule } from '../core/core.module';
import { CustomerRoutingModule } from './customer-routing.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [CustomerComponent, CustomerEditorComponent],
  imports: [
    CommonModule,
    SharedModule,
    CustomerRoutingModule,
    CoreModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ]
})
export class CustomerModule { }
