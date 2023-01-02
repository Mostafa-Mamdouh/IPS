import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { inventoryType } from '../shared/models/Enum';
import { InventoryParams } from '../shared/models/inventory-params';
import { Ilookup } from '../shared/models/lookup';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IInventoryEditor, IProductLookup } from '../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination();
  categories: Ilookup[] = [];
  inventory: IInventoryEditor;
  constructor(private http: HttpClient) {}

  getInventoryById(type: number, id: number) {
    return this.http.get<IInventoryEditor>(
      this.baseUrl +
        `${type == inventoryType.Product ? 'product' : 'service'}/` +
        id
    );
  }

  getInventory(type: number, inventoryParams: InventoryParams) {
    let params = new HttpParams();
    if (inventoryParams.createFromDate) {
      params = params.append(
        'CreateFromDate',
        inventoryParams.createFromDate.toISOString()
      );
    }

    if (inventoryParams.createToDate) {
      params = params.append(
        'CreateToDate',
        inventoryParams.createToDate.toISOString()
      );
    }

    if (inventoryParams.categoryId) {
      params = params.append(
        'CategoryId',
        inventoryParams.categoryId.toString()
      );
    }

    if (inventoryParams.search) {
      params = params.append('search', inventoryParams.search);
    }

    params = params.append('sort', inventoryParams.sort);
    params = params.append('sortDirection', inventoryParams.SortDirection);

    params = params.append('pageIndex', inventoryParams.pageNumber.toString());
    params = params.append('pageSize', inventoryParams.pageSize.toString());
    return this.http
      .get<IPagination>(
        this.baseUrl +
          `${type == inventoryType.Product ? 'product' : 'service'}`,
        {
          observe: 'response',
          params,
        }
      )
      .pipe(
        map((response) => {
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }
  exportInventory(type: number, inventoryParams: InventoryParams) {
    let params = new HttpParams();
    if (inventoryParams.createFromDate) {
      params = params.append(
        'CreateFromDate',
        inventoryParams.createFromDate.toISOString()
      );
    }

    if (inventoryParams.createToDate) {
      params = params.append(
        'CreateToDate',
        inventoryParams.createToDate.toISOString()
      );
    }

    if (inventoryParams.categoryId) {
      params = params.append(
        'CategoryId',
        inventoryParams.categoryId.toString()
      );
    }

    params = params.append('pageIndex', inventoryParams.pageNumber.toString());
    params = params.append('pageSize', inventoryParams.pageSize.toString());
    return this.http
      .get(
        this.baseUrl +
          `${type == inventoryType.Product ? 'product' : 'service'}/export`,
        {
          responseType: 'blob',
          params: params,
        }
      )
      .pipe();
  }
  saveInventory(type: number, values: IInventoryEditor, id: number) {
    return this.http
      .post<IInventoryEditor>(
        this.baseUrl +
          `${type == inventoryType.Product ? 'product' : 'service'}/${
            id > 0 ? 'update' : 'create'
          }`,
        values
      )
      .pipe(
        map((response) => {
          this.inventory = response;
          return this.inventory;
        })
      );
  }
  getCategoriesLookup() {
    return this.http
      .get<Ilookup[]>(this.baseUrl + 'home/category', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          this.categories = response.body;
          return this.categories;
        })
      );
  }

  deleteInventory(type: number, id: number) {
    return this.http
      .delete<IInventoryEditor>(
        this.baseUrl +
          `${type == inventoryType.Product ? 'product' : 'service'}/${id}`
      )
      .pipe(
        map((response) => {
          this.inventory = response;
          return this.inventory;
        })
      );
  }

  inventoryExist(
    type: inventoryType,
    category: number,
    code: string,
    id: number,
    name: string
  ) {
    let params = new HttpParams();
    params = params.append('categoryId', category.toString());
    params = params.append('code', type == inventoryType.Product ? code : null);
    params = params.append('id', id.toString());
    params = params.append('name', name);

    return this.http
      .get<boolean>(
        this.baseUrl +
          `${
            type == inventoryType.Product
              ? 'product/productexists'
              : 'service/serviceexists'
          }`,
        {
          observe: 'response',
          params,
        }
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }
}
