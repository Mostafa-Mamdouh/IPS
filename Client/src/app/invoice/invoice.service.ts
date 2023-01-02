import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICustomerLookup } from '../shared/models/customer';
import { invoiceType } from '../shared/models/Enum';
import {
  IInvoice,
  IInvoiceEditor,
  IInvoiceList,
  IInvoicePayment,
  IInvoicePaymentEditor,
} from '../shared/models/invoice';
import { InvoiceParams } from '../shared/models/invoice-params';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IProductLookup } from '../shared/models/product';
import { IserviceLookup } from '../shared/models/service';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination();
  invoices: IInvoiceList[] = [];
  customers: ICustomerLookup[] = [];
  products: IProductLookup[] = [];
  services: IserviceLookup[] = [];
  invoice: IInvoice;
  invoicePayment: IInvoicePayment;

  constructor(private http: HttpClient) {}

  getInvoice(type: number, id: number) {
    return this.http.get<IInvoice>(
      this.baseUrl +
        `${type === invoiceType.Purchase ? 'purchase' : 'sales'}/` +
        id,
    );
  }
  getInvoices(type: number, invoiceParams: InvoiceParams) {
    let params = new HttpParams();
    if (invoiceParams.invoiceFromDate) {
      params = params.append(
        'InvoiceFromDate',
        invoiceParams.invoiceFromDate.toISOString(),
      );
    }

    if (invoiceParams.invoiceToDate) {
      params = params.append(
        'InvoiceToDate',
        invoiceParams.invoiceToDate.toISOString(),
      );
    }

    if (invoiceParams.search) {
      params = params.append('search', invoiceParams.search);
    }

    params = params.append('sort', invoiceParams.sort);
    params = params.append('pageIndex', invoiceParams.pageNumber.toString());
    params = params.append('pageSize', invoiceParams.pageSize.toString());

    return this.http
      .get<IPagination>(
        this.baseUrl +
          `${type === invoiceType.Purchase ? 'purchase' : 'sales'}`,
        {
          observe: 'response',
          params,
        },
      )
      .pipe(
        map((response) => {
          this.pagination = response.body;
          return this.pagination;
        }),
      );
  }

  exportInvoices(type: number, invoiceParams: InvoiceParams) {
    let params = new HttpParams();
    if (invoiceParams.invoiceFromDate) {
      params = params.append(
        'InvoiceFromDate',
        invoiceParams.invoiceFromDate.toDateString(),
      );
    }

    if (invoiceParams.invoiceToDate) {
      params = params.append(
        'InvoiceToDate',
        invoiceParams.invoiceToDate.toDateString(),
      );
    }

    params = params.append('pageIndex', invoiceParams.pageNumber.toString());
    params = params.append('pageSize', invoiceParams.pageSize.toString());
    return this.http
      .get(
        this.baseUrl +
          `${type === invoiceType.Purchase ? 'purchase' : 'sales'}/export`,
        {
          responseType: 'blob',
          params,
        },
      )
      .pipe();
  }

  downloadFile(filePath: string) {
    let params = new HttpParams();
    if (filePath) {
      params = params.append('file', filePath);
    }
    return this.http
      .get(this.baseUrl + `upload/download`, {
        responseType: 'blob',
        params,
      })
      .pipe();
  }

  GenerateInvoiceReport(type: number, invoiceParams: InvoiceParams) {
    let params = new HttpParams();
    if (invoiceParams.invoiceFromDate) {
      params = params.append(
        'InvoiceFromDate',
        invoiceParams.invoiceFromDate.toDateString(),
      );
    }

    if (invoiceParams.invoiceToDate) {
      params = params.append(
        'InvoiceToDate',
        invoiceParams.invoiceToDate.toDateString(),
      );
    }
    return this.http
      .get(
        this.baseUrl +
          `${type === invoiceType.Purchase ? 'purchase' : 'sales'}/report`,
        {
          responseType: 'blob',
          params,
        },
      )
      .pipe();
  }

  getSuppliersLookup() {
    let params = new HttpParams();
    params = params.append('pageSize', '1000000');
    return this.http
      .get<ICustomerLookup[]>(this.baseUrl + 'supplier/lookup', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          this.customers = response.body;
          return this.customers;
        }),
      );
  }
  getClientsLookup() {
    let params = new HttpParams();
    params = params.append('pageSize', '1000000');
    return this.http
      .get<ICustomerLookup[]>(this.baseUrl + 'client/lookup', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          this.customers = response.body;
          return this.customers;
        }),
      );
  }
  getProductsLookup() {
    let params = new HttpParams();
    params = params.append('pageSize', '1000000');
    return this.http
      .get<IProductLookup[]>(this.baseUrl + 'product/lookup', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          this.products = response.body;
          return this.products;
        }),
      );
  }
  getServicesLookup() {
    let params = new HttpParams();
    params = params.append('pageSize', '1000000');
    return this.http
      .get<IserviceLookup[]>(this.baseUrl + 'service/lookup', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          this.services = response.body;
          return this.services;
        }),
      );
  }
  saveInvoice(type: number, values: IInvoiceEditor, id: number) {
    return this.http
      .post<IInvoice>(
        this.baseUrl +
          `${type === invoiceType.Purchase ? 'purchase' : 'sales'}/${
            id > 0 ? 'update' : 'create'
          }`,
        values,
      )
      .pipe(
        map((response) => {
          this.invoice = response;
          return this.invoice;
        }),
      );
  }

  savePayment(type: number, values: IInvoicePaymentEditor, id: number) {
    return this.http
      .post<IInvoicePayment>(
        this.baseUrl +
          `${type === invoiceType.Purchase ? 'purchase' : 'sales'}/${
            id > 0 ? 'update-payment' : 'create-payment'
          }`,
        values,
      )
      .pipe(
        map((response) => {
          this.invoicePayment = response;
          return this.invoicePayment;
        }),
      );
  }

  deleteInvoice(type: number, id: number) {
    return this.http
      .delete<IInvoice>(
        this.baseUrl +
          `${type === invoiceType.Purchase ? 'purchase' : 'sales'}/${id}`,
      )
      .pipe(
        map((response) => {
          this.invoice = response;
          return this.invoice;
        }),
      );
  }

  deletePayment(type: number, id: number) {
    return this.http
      .delete<IInvoicePayment>(
        this.baseUrl +
          `${
            type === invoiceType.Purchase ? 'purchase' : 'sales'
          }/payment/${id}`,
      )
      .pipe(
        map((response) => {
          this.invoicePayment = response;
          return this.invoicePayment;
        }),
      );
  }
}
