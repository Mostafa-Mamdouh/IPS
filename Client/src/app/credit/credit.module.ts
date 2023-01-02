import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreditEditorComponent } from './credit-editor/credit-editor.component';
import { CreditComponent } from './credit/credit.component';
import { CoreModule } from '../core/core.module';
import { CreditRoutingModule } from './credit-routing.module';
import { SharedModule } from '../shared/shared.module';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@NgModule({
  declarations: [CreditEditorComponent, CreditComponent],
  imports: [
    CommonModule,
    SharedModule,
    CreditRoutingModule,
    CoreModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ],
})
export class CreditModule {}
