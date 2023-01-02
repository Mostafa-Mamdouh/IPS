import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpenseComponent } from './expense.component';
import { ExpenseEditorComponent } from './expense-editor/expense-editor.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CoreModule } from '../core/core.module';
import { ExpenseRoutingModule } from './expense-routing.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [ExpenseComponent, ExpenseEditorComponent],
  imports: [
    CommonModule,
    SharedModule,
    ExpenseRoutingModule,
    CoreModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ]
})
export class ExpenseModule { }
