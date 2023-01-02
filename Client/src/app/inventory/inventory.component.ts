import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as FileSaver from 'file-saver';
import * as moment from 'moment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { inventoryType } from '../shared/models/Enum';
import { InventoryParams } from '../shared/models/inventory-params';
import { Ilookup } from '../shared/models/lookup';
import { IProductLookup } from '../shared/models/product';
import { IserviceLookup } from '../shared/models/service';
import { InventoryService } from './inventory.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss']
})
export class InventoryComponent implements OnInit {

  inventoryType:inventoryType;
  categories:Ilookup[];
  public totalItems = 64;
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @ViewChild('fromDate', { static: false }) fromDateTerm: ElementRef;
  @ViewChild('toDate', { static: false }) toDateTerm: ElementRef;



  products: IProductLookup[] = [];
  services:IserviceLookup[]=[];
  inventoryParams = new InventoryParams();
  exportParams = new InventoryParams();
  totalCount: number;
  deletemodalRef: BsModalRef;

  constructor(
    private inventoryService: InventoryService,
    private activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.activatedroute.data.subscribe((data) => {
      this.inventoryType = data.type;
    });
    this.getCategories();

    this.getInventory();
  }
  onCategoryChanged(e){
    if(e)
    this.inventoryParams.categoryId=e.id;
    else
    this.inventoryParams.categoryId=null;
  }

  getInventory() {
    this.inventoryService
      .getInventory(this.inventoryType, this.inventoryParams)
      .subscribe(
        (res) => {

          if(this.inventoryType==inventoryType.Product){
            this.products = res.data;
          }
          else{
            this.services = res.data;
          }
          this.inventoryParams.pageNumber = res.pageIndex;
          this.inventoryParams.pageSize = res.pageSize;
          this.totalCount = res.count;
        },
        (error) => {
          console.log(error);
        }
      );
  }
  getCategories() {
    this.inventoryService
      .getCategoriesLookup()
      .subscribe(
        (res) => {
          this.categories = res;
        },
        (error) => {
          console.log(error);
        }
      );
  }
 
  onPageChanged(event: any) {
    if (this.inventoryParams.pageNumber !== event) {
      this.inventoryParams.pageNumber = event;
      this.getInventory();
    }
  }

  onSearch() {
    this.inventoryParams.search = this.searchTerm.nativeElement.value;
    this.inventoryParams.pageNumber = 1;
    this.getInventory();
  }

  onFilter() {
    var createFromDate = this.fromDateTerm.nativeElement.value;
    var createToDate = this.toDateTerm.nativeElement.value;

    if (createFromDate && createToDate && createFromDate > createToDate) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (createFromDate) {
        var datefromMomentObject = moment(createFromDate, 'DD/MM/YYYY').add(2, 'hours');
        this.inventoryParams.createFromDate = datefromMomentObject.toDate();

      }
      else {
        this.inventoryParams.createFromDate =createFromDate;
      } 

      if (createToDate) {
        var datetoMomentObject = moment(createToDate, 'DD/MM/YYYY').add(2, 'hours');
        this.inventoryParams.createToDate = datetoMomentObject.toDate();
      }
      else {
        this.inventoryParams.createToDate =createToDate;
      } 
    
      this.inventoryParams.pageNumber = 1;
      this.getInventory();

    }
  }

  Export() {
    var createFromDate = this.fromDateTerm.nativeElement.value;
    var createToDate = this.toDateTerm.nativeElement.value;

    if (createFromDate && createToDate && createFromDate > createToDate) {
      this.toastr.error('ToDate must be after from date');
    } else {
      if (createFromDate) {
        var datefromMomentObject = moment(createFromDate, 'DD/MM/YYYY').add(2, 'hours');
        this.exportParams.createFromDate = datefromMomentObject.toDate();
      }
      else {
        this.exportParams.createFromDate =createFromDate;
      } 

      if (createToDate) {
        var datetoMomentObject = moment(createToDate, 'DD/MM/YYYY').add(2, 'hours');
        this.exportParams.createToDate = datetoMomentObject.toDate();
      }
      else {
        this.exportParams.createToDate =createToDate;
      } 

      this.exportParams.pageNumber = 1;
      this.exportParams.pageSize = 1000000;


      this.inventoryService
        .exportInventory(this.inventoryType,this.exportParams)
        .subscribe(
          (data: Blob) => {
            var fileName =
            this.inventoryType == inventoryType.Product
                ? 'Products_list_' + Date.now() + ''
                : 'Services_list_' + Date.now() + '';
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


  deleteInventory(id:number){

    this.inventoryService.deleteInventory(this.inventoryType,id).subscribe(
      () => {
        this.deletemodalRef.hide();
        this.toastr.success(`${this.inventoryType==inventoryType.Product?'Product':'Service'} has been deleted successfully`);
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
