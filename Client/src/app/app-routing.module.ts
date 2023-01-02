import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { NotFountComponent } from './core/not-fount/not-fount.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { customerType, inventoryType, invoiceType } from './shared/models/Enum';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./home/home.module').then((mod) => mod.HomeModule),
    data: { breadcrumb: 'Home' },
  },
  {
    path: 'test-error',
    component: TestErrorComponent,
    data: { breadcrumb: 'Test Errors' },
  },
  {
    path: 'server-error',
    component: ServerErrorComponent,
    data: { breadcrumb: 'Server Errors' },
  },
  {
    path: 'not-found',
    component: NotFountComponent,
    data: { breadcrumb: 'Not Found' },
  },
  {
    path: 'account',
    loadChildren: () =>
      import('./account/account.module').then((mod) => mod.AccountModule),
    data: { breadcrumb: { skip: true } },
  },
  {
    path: 'purchase-invoice',
    loadChildren: () =>
      import('./invoice/invoice.module').then((mod) => mod.InvoiceModule),
    data: { breadcrumb: 'Purchasing Invoices', type: invoiceType.Purchase },
  },
  {
    path: 'sales-invoice',
    loadChildren: () =>
      import('./invoice/invoice.module').then((mod) => mod.InvoiceModule),
    data: { breadcrumb: 'Sales Invoices', type: invoiceType.Sales },
  },
  {
    path: 'product',
    loadChildren: () =>
      import('./inventory/inventory.module').then((mod) => mod.InventoryModule),
    data: {
      breadcrumb: 'Products',
      type: inventoryType.Product,
    },
  },
  {
    path: 'service',
    loadChildren: () =>
      import('./inventory/inventory.module').then((mod) => mod.InventoryModule),
    data: { breadcrumb: 'Services', type: inventoryType.Service },
  },
  {
    path: 'client',
    loadChildren: () =>
      import('./customer/customer.module').then((mod) => mod.CustomerModule),

    data: {
      breadcrumb: 'Clients',
      type: customerType.Client,
    },
  },
  {
    path: 'supplier',
    loadChildren: () =>
      import('./customer/customer.module').then((mod) => mod.CustomerModule),
    data: { breadcrumb: 'Suppliers', type: customerType.Supplier },
  },
  {
    path: 'expenses',
    loadChildren: () =>
      import('./expense/expense.module').then((mod) => mod.ExpenseModule),
    data: { breadcrumb: 'Expenses' },
  },

  {
    path: 'users',
    loadChildren: () =>
      import('./user/user.module').then((mod) => mod.UserModule),
    data: { breadcrumb: 'Users' },
  },
  {
    path: 'transaction',
    loadChildren: () =>
      import('./credit/credit.module').then((mod) => mod.CreditModule),

    data: {
      breadcrumb: 'Transactions',
    },
  },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
