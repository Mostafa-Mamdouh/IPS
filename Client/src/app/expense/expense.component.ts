import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import * as FileSaver from 'file-saver';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { ExpenseParams, IExpense } from '../shared/models/expense';
import { ExpenseService } from './expense.service';

@Component({
  selector: 'app-expense',
  templateUrl: './expense.component.html',
  styleUrls: ['./expense.component.scss']
})
export class ExpenseComponent implements OnInit {
  public totalItems = 64;
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate', { static: false }) fromDateTerm: ElementRef;
  @ViewChild('toDate', { static: false }) toDateTerm: ElementRef;


  expenses: IExpense[] = [];
  expenseParams = new ExpenseParams();
  exportParams = new ExpenseParams();
  totalCount: number;
  deletemodalRef: BsModalRef;

  constructor(
    private expenseService: ExpenseService,
    private activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router,
  ) {}

  ngOnInit(): void {


    this.getExpenses();
  }

  getExpenses() {
    this.expenseService
      .getExpenses( this.expenseParams)
      .subscribe(
        (res) => {

            this.expenses = res.data;
         
          this.expenseParams.pageNumber = res.pageIndex;
          this.expenseParams.pageSize = res.pageSize;
          this.totalCount = res.count;
        },
        (error) => {
          console.log(error);
        }
      );
  }

  onPageChanged(event: any) {
    if (this.expenseParams.pageNumber !== event) {
      this.expenseParams.pageNumber = event;
      this.getExpenses();
    }
  }

  onSearch() {
    this.expenseParams.search = this.searchTerm.nativeElement.value;
    this.expenseParams.pageNumber = 1;
    this.getExpenses();
  }

  onFilter() {
    var transactionFromDate = this.fromDateTerm.nativeElement.value;
    var transactionToDate = this.toDateTerm.nativeElement.value;

    if (transactionFromDate && transactionToDate && transactionFromDate > transactionToDate) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (transactionFromDate) {
        var datefromMomentObject = moment(transactionFromDate, 'DD/MM/YYYY').add(2, 'hours');
        this.expenseParams.transactionFromDate = datefromMomentObject.toDate();

      }
      else {
        this.expenseParams.transactionFromDate =transactionFromDate;
      } 

      if (transactionToDate) {
        var datetoMomentObject = moment(transactionToDate, 'DD/MM/YYYY').add(2, 'hours');
        this.expenseParams.transactionToDate = datetoMomentObject.toDate();
      }
      else {
        this.expenseParams.transactionToDate =transactionToDate;
      } 
    
      this.expenseParams.pageNumber = 1;
      this.getExpenses();

    }
  }

  Export() {
    var transactionFromDate = this.fromDateTerm.nativeElement.value;
    var transactionToDate = this.toDateTerm.nativeElement.value;

    if (transactionFromDate && transactionToDate && transactionFromDate > transactionToDate) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (transactionFromDate) {
        var datefromMomentObject = moment(transactionFromDate, 'DD/MM/YYYY').add(2, 'hours');
        this.exportParams.transactionFromDate = datefromMomentObject.toDate();

      }
      else {
        this.exportParams.transactionFromDate =transactionFromDate;
      } 

      if (transactionToDate) {
        var datetoMomentObject = moment(transactionToDate, 'DD/MM/YYYY').add(2, 'hours');
        this.exportParams.transactionToDate = datetoMomentObject.toDate();
      }
      else {
        this.exportParams.transactionToDate =transactionToDate;
      } 

      this.exportParams.pageNumber = 1;
      this.exportParams.pageSize = 1000000;


      this.expenseService
        .exportExpense(this.exportParams)
        .subscribe(
          (data: Blob) => {
            var fileName = 'Expenses_list_' + Date.now() + '';
            FileSaver.saveAs(data, fileName);
          },
          (err: any) => {
            console.log(`Unable to export file`);
          }
        );
    }
  }

  openDeleteModal(
    template: TemplateRef<any>,
  ) {

    this.deletemodalRef = this.modalService.show(template);
  }


  deleteExpense(id:number){

    this.expenseService.deleteExpense(id).subscribe(
      () => {
        this.deletemodalRef.hide();
        this.toastr.success(`Transaction has been deleted successfully`);
        setTimeout (() => {
          this.reloadCurrentPage();
       }, 2000);
      },
      (err) => {
        console.log(err);
      }
    );
  }

  reloadCurrentPage(){
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
    this.router.navigate([currentUrl]);
    });
  }
}
