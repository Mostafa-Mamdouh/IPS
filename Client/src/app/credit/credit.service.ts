import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ICredit } from '../shared/models/credit';
import { creditType, transactionType } from '../shared/models/Enum';
import { LatestTransactionParams } from '../shared/models/latestTransaction';
import { IPagination, Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class CreditService {
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiUrl;
  pagination = new Pagination();
  credit: ICredit;
  getTransactions(type: number, transactionParams: LatestTransactionParams) {
    let params = new HttpParams();

    params = params.append(
      'pageIndex',
      transactionParams.pageNumber.toString()
    );
    params = params.append('pageSize', transactionParams.pageSize.toString());

    return this.http
      .get<IPagination>(
        this.baseUrl +
          `${
            type == transactionType.Bank
              ? 'home/bank-transaction'
              : 'home/cash-transaction'
          }`,
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

  getCurrentCredit() {
    return this.http.get<ICredit>(
      this.baseUrl + `credit/` + creditType.currentCredit
    );
  }
  saveCredit(credit: ICredit) {
    return this.http.post<ICredit>(this.baseUrl + `credit/update`, credit).pipe(
      map((response) => {
        this.credit = response;
        return this.credit;
      })
    );
  }
}
