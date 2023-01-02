import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AccountService } from '../account/account.service';
import { transactionType } from '../shared/models/Enum';
import {
  IlatestTransaction,
  LatestTransactionParams,
} from '../shared/models/latestTransaction';
import { IUser } from '../shared/models/user';
import { HomeService } from './home.service';
import * as FileSaver from 'file-saver';
import { IDashboardAmounts } from '../shared/models/dashboard-amounts';
import { ILineChart } from '../shared/models/line-chart';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  currentUser$: Observable<IUser>;
  dashboardAmounts: IDashboardAmounts;
  lineChart: ILineChart = new ILineChart();
  lineChartData: Array<any>;
  public now: Date = new Date();
  constructor(
    private accountService: AccountService,
    private homeService: HomeService,
    private activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService,
    private datePipe: DatePipe
  ) {
    setInterval(() => {
      this.now = new Date();
    }, 1);
  }

  ngOnInit(): void {
    this.getDashboardAmounts();
    this.getLineChart();
    this.currentUser$ = this.accountService.currentUser$;
  }

  getDashboardAmounts() {
    this.homeService.getDashboardAmounts().subscribe(
      (res) => {
        this.dashboardAmounts = res;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getLineChart() {
    this.homeService.getLineChart().subscribe(
      (res) => {
        this.lineChart = res;
        this.lineChartData = [
          {
            data: [
              this.lineChart.janInvoice,
              this.lineChart.febInvoice,
              this.lineChart.marInvoice,
              this.lineChart.aprInvoice,
              this.lineChart.mayInvoice,
              this.lineChart.junInvoice,
              this.lineChart.julInvoice,
              this.lineChart.augInvoice,
              this.lineChart.sepInvoice,
              this.lineChart.octInvoice,
              this.lineChart.novInvoice,
              this.lineChart.decInvoice,
            ],
            label: 'Production',
          },
          {
            data: [
              this.lineChart.janPaid,
              this.lineChart.febPaid,
              this.lineChart.marPaid,
              this.lineChart.aprPaid,
              this.lineChart.mayPaid,
              this.lineChart.junPaid,
              this.lineChart.julPaid,
              this.lineChart.augPaid,
              this.lineChart.sepPaid,
              this.lineChart.octPaid,
              this.lineChart.novPaid,
              this.lineChart.decPaid,
            ],
            label: 'Payment',
          },
        ];
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
