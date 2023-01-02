import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { HomeService } from 'src/app/home/home.service';
import { transactionType } from 'src/app/shared/models/Enum';
import {
  IlatestTransaction,
  LatestTransactionParams,
} from 'src/app/shared/models/latestTransaction';
import { IUser } from 'src/app/shared/models/user';
import * as moment from 'moment';
import * as FileSaver from 'file-saver';
import { CreditService } from '../credit.service';
import { ICredit } from 'src/app/shared/models/credit';
@Component({
  selector: 'app-credit',
  templateUrl: './credit.component.html',
  styleUrls: ['./credit.component.scss'],
})
export class CreditComponent implements OnInit {
  transactionType: transactionType;
  latestBankTransactionParams: LatestTransactionParams =
    new LatestTransactionParams();
  latestCashTransactionParams: LatestTransactionParams =
    new LatestTransactionParams();

  bankCurrentCredit: number = 0;
  cashCurrentCredit: number = 0;

  bankTransactions: IlatestTransaction[];
  cashTransactions: IlatestTransaction[];

  totalBankCount: number = 64;
  totalCashCount: number = 64;

  currentUser$: Observable<IUser>;

  credit: ICredit;
  constructor(
    private accountService: AccountService,
    private creditService: CreditService,
    private activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    this.getTranasctions(transactionType.Bank);
    this.getTranasctions(transactionType.Cash);
    this.loadCredit();
  }
  getTranasctions(transactionType: transactionType) {
    this.creditService
      .getTransactions(
        transactionType,
        transactionType == 1
          ? this.latestBankTransactionParams
          : this.latestCashTransactionParams
      )
      .subscribe(
        (res) => {
          transactionType == 1
            ? (this.bankTransactions = res.data)
            : (this.cashTransactions = res.data);
          transactionType == 1
            ? (this.latestBankTransactionParams.pageNumber = res.pageIndex)
            : (this.latestCashTransactionParams.pageNumber = res.pageIndex);
          transactionType == 1
            ? (this.latestBankTransactionParams.pageSize = res.pageSize)
            : (this.latestCashTransactionParams.pageSize = res.pageSize);
          transactionType == 1
            ? (this.totalBankCount = res.count)
            : (this.totalCashCount = res.count);
        },
        (error) => {
          console.log(error);
        }
      );
  }

  onBankPageChanged(event: any) {
    if (this.latestBankTransactionParams.pageNumber !== event) {
      this.latestBankTransactionParams.pageNumber = event;
      this.getTranasctions(transactionType.Bank);
    }
  }
  onCashPageChanged(event: any) {
    if (this.latestCashTransactionParams.pageNumber !== event) {
      this.latestCashTransactionParams.pageNumber = event;
      this.getTranasctions(transactionType.Cash);
    }
  }
  loadCredit() {
    this.creditService.getCurrentCredit().subscribe(
      (res) => {
        this.credit = res;
      },
      (err) => {
        console.log(err.message);
      }
    );
  }
}
