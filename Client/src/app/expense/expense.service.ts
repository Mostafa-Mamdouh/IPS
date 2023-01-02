import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { inventoryType } from '../shared/models/Enum';
import { ExpenseParams, IExpense, IExpenseEditor } from '../shared/models/expense';
import { Ilookup } from '../shared/models/lookup';
import { IPagination, Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination();
  expense:IExpense;
  expenseEditor:IExpenseEditor;
  expenseTypes:Ilookup[]=[];
  constructor(private http: HttpClient) { }

  getExpenseById(id: number) {
    return this.http.get<IExpense>(
      this.baseUrl +
        `expense/` +
        id
    );
  }


  getExpenses( expenseParams: ExpenseParams) {
    let params = new HttpParams();
    if (expenseParams.transactionFromDate) {
      params = params.append(
        'TransactionFromDate',
        expenseParams.transactionFromDate.toISOString()
      );
    }

    if (expenseParams.transactionToDate) {
      params = params.append(
        'TransactionToDate',
        expenseParams.transactionToDate.toISOString()
      );
    }


    if (expenseParams.search) {
      params = params.append('search', expenseParams.search);
    }

    params = params.append('sort', expenseParams.sort);
    params = params.append('pageIndex', expenseParams.pageNumber.toString());
    params = params.append('pageSize', expenseParams.pageSize.toString());

    return this.http
      .get<IPagination>(
        this.baseUrl + `expense`,
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
  exportExpense(expenseParams: ExpenseParams) {
    let params = new HttpParams();
    if (expenseParams.transactionFromDate) {
      params = params.append(
        'TransactionFromDate',
        expenseParams.transactionFromDate.toISOString()
      );
    }

    if (expenseParams.transactionToDate) {
      params = params.append(
        'TransactionToDate',
        expenseParams.transactionToDate.toISOString()
      );
    }

    params = params.append('pageIndex', expenseParams.pageNumber.toString());
    params = params.append('pageSize', expenseParams.pageSize.toString());
    return this.http
      .get(
        this.baseUrl +
          `expense/export`,
        {
          responseType: 'blob',
          params: params,
        }
      )
      .pipe();
  }
  saveExpense( values: IExpenseEditor,id:number) {
    return this.http
      .post<IExpenseEditor>(
        this.baseUrl +
          `expense/${id >0? 'update' : 'create'}`,
        values
      )
      .pipe(
        map((response) => {
          this.expenseEditor = response;
          return this.expenseEditor;
        })
      );
  }


  deleteExpense(id:number ) {
    return this.http
      .delete<IExpenseEditor>(
        this.baseUrl +
          `expense/${id}`,
      )
      .pipe(
        map((response) => {
          this.expenseEditor = response;
          return this.expenseEditor;
        })
      );


}

getExpenseTypesLookup() {
  return this.http
    .get<Ilookup[]>(this.baseUrl + 'home/expensetypes', {
      observe: 'response',
    })
    .pipe(
      map((response) => {
        this.expenseTypes = response.body;
        return this.expenseTypes;
      })
    );
}

}


