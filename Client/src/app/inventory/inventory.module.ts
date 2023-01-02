import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryComponent } from './inventory.component';
import { InventoryEditorComponent } from './inventory-editor/inventory-editor.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { InventoryRoutingModule } from './inventory-routing.module';



@NgModule({
  declarations: [InventoryComponent, InventoryEditorComponent],
  imports: [
    CommonModule,
    SharedModule,
    InventoryRoutingModule,
    CoreModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ]
})
export class InventoryModule { }
