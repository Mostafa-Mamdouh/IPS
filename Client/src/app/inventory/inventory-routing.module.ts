import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { InventoryEditorComponent } from './inventory-editor/inventory-editor.component';
import { InventoryComponent } from './inventory.component';




const routes: Routes = [
  {path: '', component: InventoryComponent,canActivate: [AuthGuard],data:{  
      Permission: 'Inventory:List/View Products'}},
  {path: 'add', component: InventoryEditorComponent,canActivate: [AuthGuard],data: { breadcrumb: 'Add',
      Permission: 'Inventory:Add Product' }},
  {path: 'edit/:id', component: InventoryEditorComponent,canActivate: [AuthGuard] ,data: { breadcrumb: 'Edit',
      Permission: 'Inventory:Edit Product' }},
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryRoutingModule {}
