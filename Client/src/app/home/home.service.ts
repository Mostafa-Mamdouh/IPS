import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import {
  CustomerParams,
  ICustomerEditor,
  ICustomerLookup,
} from '../shared/models/customer';
import { IDashboardAmounts } from '../shared/models/dashboard-amounts';
import { customerType, transactionType } from '../shared/models/Enum';
import {
  IlatestTransaction,
  LatestTransactionParams,
} from '../shared/models/latestTransaction';
import { ILineChart } from '../shared/models/line-chart';
import { Ilookup } from '../shared/models/lookup';
import { IPagination, Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination();

  bankTransactions: IlatestTransaction;
  cashTransactions: IlatestTransaction;
  dashboardAmounts: IDashboardAmounts;
  lineChart: ILineChart;

  constructor(private http: HttpClient) {}

  getDashboardAmounts() {
    return this.http
      .get<IDashboardAmounts>(this.baseUrl + 'home/dashboard-amounts', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          this.dashboardAmounts = response.body;
          return this.dashboardAmounts;
        })
      );
  }

  getLineChart() {
    return this.http
      .get<ILineChart>(this.baseUrl + 'home/line-chart', {
        observe: 'response',
      })
      .pipe(
        map((response) => {
          this.lineChart = response.body;
          return this.lineChart;
        })
      );
  }
}
