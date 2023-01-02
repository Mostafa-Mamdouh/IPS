import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {
  CustomerParams,
  ICustomerEditor,
  ICustomerLookup,
} from '../shared/models/customer';
import { customerType } from '../shared/models/Enum';
import { Ilookup } from '../shared/models/lookup';
import { IPagination, Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination();
  citeies: Ilookup[] = [];
  countries: Ilookup[] = [];

  customer: ICustomerEditor;
  customerLookup: ICustomerLookup;
  constructor(private http: HttpClient) {}

  getCustomerById(type: number, id: number) {
    return this.http.get<ICustomerLookup>(
      this.baseUrl +
        `${type == customerType.Client ? 'client' : 'supplier'}/` +
        id,
    );
  }
  getCustomers(type: number, customerParams: CustomerParams) {
    let params = new HttpParams();
    if (customerParams.createFromDate) {
      params = params.append(
        'CreateFromDate',
        customerParams.createFromDate.toISOString(),
      );
    }

    if (customerParams.createToDate) {
      params = params.append(
        'CreateToDate',
        customerParams.createToDate.toISOString(),
      );
    }
    if (customerParams.invoiceFromDate) {
      params = params.append(
        'InvoiceFromDate',
        customerParams.invoiceFromDate.toISOString(),
      );
    }

    if (customerParams.invoiceToDate) {
      params = params.append(
        'InvoiceToDate',
        customerParams.invoiceToDate.toISOString(),
      );
    }

    if (customerParams.search) {
      params = params.append('search', customerParams.search);
    }

    params = params.append('sort', customerParams.sort);
    params = params.append('pageIndex', customerParams.pageNumber.toString());
    params = params.append('pageSize', customerParams.pageSize.toString());

    return this.http
      .get<IPagination>(
        this.baseUrl + `${type == customerType.Client ? 'client' : 'supplier'}`,
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
  exportCustomers(type: number, customerParams: CustomerParams) {
    let params = new HttpParams();
    if (customerParams.createFromDate) {
      params = params.append(
        'CreateFromDate',
        customerParams.createFromDate.toISOString(),
      );
    }

    if (customerParams.createToDate) {
      params = params.append(
        'CreateToDate',
        customerParams.createToDate.toISOString(),
      );
    }
    if (customerParams.invoiceFromDate) {
      params = params.append(
        'InvoiceFromDate',
        customerParams.invoiceFromDate.toISOString(),
      );
    }

    if (customerParams.invoiceToDate) {
      params = params.append(
        'InvoiceToDate',
        customerParams.invoiceToDate.toISOString(),
      );
    }

    params = params.append('pageIndex', customerParams.pageNumber.toString());
    params = params.append('pageSize', customerParams.pageSize.toString());
    return this.http
      .get(
        this.baseUrl +
          `${type == customerType.Client ? 'client' : 'supplier'}/export`,
        {
          responseType: 'blob',
          params: params,
        },
      )
      .pipe();
  }
  saveCustomer(type: number, values: ICustomerEditor, id: number) {
    return this.http
      .post<ICustomerEditor>(
        this.baseUrl +
          `${type == customerType.Client ? 'client' : 'supplier'}/${
            id > 0 ? 'update' : 'create'
          }`,
        values,
      )
      .pipe(
        map((response) => {
          this.customer = response;
          return this.customer;
        }),
      );
  }
  getCountriesLookup() {
    return this.http
      .get<Ilookup[]>(this.baseUrl + 'home/country', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          this.countries = response.body;
          return this.countries;
        }),
      );
  }

  getCitiesByCountryIdLookup(countryId: number) {
    return this.http
      .get<Ilookup[]>(this.baseUrl + `home/cities/${countryId}`, {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          this.citeies = response.body;
          return this.citeies;
        }),
      );
  }
  deleteCustomer(type: number, id: number) {
    return this.http
      .delete<ICustomerLookup>(
        this.baseUrl +
          `${type == customerType.Client ? 'client' : 'supplier'}/${id}`,
      )
      .pipe(
        map((response) => {
          this.customer = response;
          return this.customer;
        }),
      );
  }

  customerExist(
    type: customerType,
    name: string,
    id: number,
    taxRefNo: string,
  ) {
    let params = new HttpParams();
    params = params.append('name', name);
    params = params.append('id', id.toString());
    params = params.append('taxRefNo', taxRefNo);
    return this.http
      .get<boolean>(
        this.baseUrl +
          `${
            type == customerType.Client
              ? 'client/clientexists'
              : 'supplier/supplierexists'
          }`,
        {
          observe: 'response',
          params,
        },
      )
      .pipe(
        map((response) => {
          return response;
        }),
      );
  }
}
