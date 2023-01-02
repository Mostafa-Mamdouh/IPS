import {
  Component,
  ElementRef,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import * as moment from 'moment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { CustomerParams, ICustomerLookup } from '../shared/models/customer';
import { customerType } from '../shared/models/Enum';
import { Ilookup } from '../shared/models/lookup';
import { CustomerService } from './customer.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
})
export class CustomerComponent implements OnInit {
  customerType: customerType;
  countries: Ilookup[];
  cities: Ilookup[];

  public totalItems = 64;
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate', { static: false }) fromDateTerm: ElementRef;
  @ViewChild('toDate', { static: false }) toDateTerm: ElementRef;
  @ViewChild('invoiceFromDate', { static: false }) invoiceFromDate: ElementRef;
  @ViewChild('invoiceToDate', { static: false }) invoiceToDate: ElementRef;

  customers: ICustomerLookup[] = [];
  customerParams = new CustomerParams();
  exportParams = new CustomerParams();
  totalCount: number;
  deletemodalRef: BsModalRef;

  constructor(
    private customerService: CustomerService,
    private activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.activatedroute.data.subscribe((data) => {
      this.customerType = data.type;
    });

    this.getCustomers();
  }

  getCustomers() {
    this.customerService
      .getCustomers(this.customerType, this.customerParams)
      .subscribe(
        (res) => {
          this.customers = res.data;
          this.customerParams.pageNumber = res.pageIndex;
          this.customerParams.pageSize = res.pageSize;
          this.totalCount = res.count;
        },
        (error) => {
          console.log(error);
        }
      );
  }
  getCountries() {
    this.customerService.getCountriesLookup().subscribe(
      (res) => {
        this.countries = res;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getCitiesByCountryId(countryId: number) {
    this.customerService.getCitiesByCountryIdLookup(countryId).subscribe(
      (res) => {
        this.cities = res;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  onPageChanged(event: any) {
    if (this.customerParams.pageNumber !== event) {
      this.customerParams.pageNumber = event;
      this.getCustomers();
    }
  }

  onSearch() {
    this.customerParams.search = this.searchTerm.nativeElement.value;
    this.customerParams.pageNumber = 1;
    this.getCustomers();
  }

  onFilter() {
    var createFromDate = this.fromDateTerm.nativeElement.value;
    var createToDate = this.toDateTerm.nativeElement.value;
    var invoiceFromDate = this.invoiceFromDate.nativeElement.value;
    var invoiceToDate = this.invoiceToDate.nativeElement.value;

    if (
      (createFromDate && createToDate && createFromDate > createToDate) ||
      (invoiceFromDate && invoiceToDate && invoiceFromDate > invoiceToDate)
    ) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (createFromDate) {
        var datefromMomentObject = moment(createFromDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.customerParams.createFromDate = datefromMomentObject.toDate();
      } else {
        this.customerParams.createFromDate = createFromDate;
      }

      if (createToDate) {
        var datetoMomentObject = moment(createToDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.customerParams.createToDate = datetoMomentObject.toDate();
      } else {
        this.customerParams.createToDate = createToDate;
      }

      if (invoiceFromDate) {
        var datefromMomentObject = moment(invoiceFromDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.customerParams.invoiceFromDate = datefromMomentObject.toDate();
      } else {
        this.customerParams.invoiceFromDate = invoiceFromDate;
      }

      if (invoiceToDate) {
        var datetoMomentObject = moment(invoiceToDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.customerParams.invoiceToDate = datetoMomentObject.toDate();
      } else {
        this.customerParams.invoiceToDate = invoiceToDate;
      }

      this.customerParams.pageNumber = 1;
      this.getCustomers();
    }
  }

  Export() {
    var createFromDate = this.fromDateTerm.nativeElement.value;
    var createToDate = this.toDateTerm.nativeElement.value;
    var invoiceFromDate = this.invoiceFromDate.nativeElement.value;
    var invoiceToDate = this.invoiceToDate.nativeElement.value;

    if (
      (createFromDate && createToDate && createFromDate > createToDate) ||
      (invoiceFromDate && invoiceToDate && invoiceFromDate > invoiceToDate)
    ) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (createFromDate) {
        var datefromMomentObject = moment(createFromDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.exportParams.createFromDate = datefromMomentObject.toDate();
      } else {
        this.exportParams.createFromDate = createFromDate;
      }

      if (createToDate) {
        var datetoMomentObject = moment(createToDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.exportParams.createToDate = datetoMomentObject.toDate();
      } else {
        this.exportParams.createToDate = createToDate;
      }

      if (invoiceFromDate) {
        var datefromMomentObject = moment(invoiceFromDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.exportParams.invoiceFromDate = datefromMomentObject.toDate();
      } else {
        this.exportParams.invoiceFromDate = invoiceFromDate;
      }

      if (invoiceToDate) {
        var datetoMomentObject = moment(invoiceToDate, 'DD/MM/YYYY').add(
          2,
          'hours'
        );
        this.exportParams.invoiceToDate = datetoMomentObject.toDate();
      } else {
        this.exportParams.invoiceToDate = invoiceToDate;
      }

      this.exportParams.pageNumber = 1;
      this.exportParams.pageSize = 1000000;

      this.customerService
        .exportCustomers(this.customerType, this.exportParams)
        .subscribe(
          (data: Blob) => {
            var fileName =
              this.customerType == customerType.Client
                ? 'Clients_list_' + Date.now() + ''
                : 'Suppliers_list_' + Date.now() + '';
            FileSaver.saveAs(data, fileName);
          },
          (err: any) => {
            console.log(`Unable to export file`);
          }
        );
    }
  }

  openDeleteModal(template: TemplateRef<any>) {
    this.deletemodalRef = this.modalService.show(template);
  }

  deleteCustomer(id: number) {
    this.customerService.deleteCustomer(this.customerType, id).subscribe(
      () => {
        this.deletemodalRef.hide();
        this.toastr.success(
          `${
            this.customerType == customerType.Client ? 'Client' : 'Supplier'
          } has been deleted successfully`
        );
        setTimeout(() => {
          this.reloadCurrentPage();
        }, 2000);
      },
      (err) => {
        console.log(err);
      }
    );
  }
  reloadCurrentPage() {
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }
}
