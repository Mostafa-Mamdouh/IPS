import { AfterViewInit, Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { ICustomerLookup } from 'src/app/shared/models/customer';
import { invoiceType } from 'src/app/shared/models/Enum';
import {
  IInvoice,
  IInvoiceEditor,
  IInvoiceProduct,
} from 'src/app/shared/models/invoice';
import { IProductLookup } from 'src/app/shared/models/product';
import { IserviceLookup } from 'src/app/shared/models/service';
import { InvoiceService } from '../invoice.service';

@Component({
  selector: 'app-invoice-editor',
  templateUrl: './invoice-editor.component.html',
  styleUrls: ['./invoice-editor.component.scss'],
})
export class InvoiceEditorComponent implements OnInit {
  invoiceForm: FormGroup;
  invoiceProducts: FormArray;
  invoiceProductItems: IInvoiceProduct[] = [];
  deletedInvoiceProductOrServicesIds: number[] = [];

  invoiceEditor: IInvoiceEditor;
  invoice: IInvoice;

  invoiceType: invoiceType;
  invoiceId = 0;
  ArchiveId: number = null;

  maxDate: Date = new Date();

  private invoiceProductItemsSource = new BehaviorSubject<IInvoiceProduct[]>(
    null
  );
  currentInvoiceProductItems$ = this.invoiceProductItemsSource.asObservable();
  totalInvoiceAmount: number = 0;
  formData = new FormData();
  customers: ICustomerLookup[] = [];
  products: IProductLookup[] = [];
  services: IserviceLookup[] = [];

  constructor(
    private invoiceService: InvoiceService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.invoiceId = +this.activatedroute.snapshot.paramMap.get('id');

    this.activatedroute.data.subscribe((data) => {
      this.invoiceType = data.type;
    });
    this.createInvoiceForm();

    if (this.invoiceType == invoiceType.Purchase) {
      this.getSuppliersLookup();
      this.invoiceForm.controls['supplier'].setValidators(Validators.required);
      this.invoiceForm.controls['supplier'].updateValueAndValidity();
      if (this.invoiceForm.controls['isTax'].value) {
        this.invoiceForm.controls['invoiceNumber'].setValidators(
          Validators.required
        );
        this.invoiceForm.controls['invoiceNumber'].updateValueAndValidity();
      }
    } else if (this.invoiceType == invoiceType.Sales) {
      this.getClientsLookup();
      this.invoiceForm.controls['client'].setValidators(Validators.required);
      this.invoiceForm.controls['client'].updateValueAndValidity();
    }

    this.getProductsLookup();
  }

  loadInvoice() {
    this.invoiceService
      .getInvoice(
        this.invoiceType,
        +this.activatedroute.snapshot.paramMap.get('id')
      )
      .subscribe(
        (res) => {
          this.invoice = res;
          if (this.invoice) this.assignFormData(this.invoice);
        },
        (err) => {
          console.log(err.message);
        }
      );
  }

  assignFormData(invoice: IInvoice) {
    this.invoiceForm.controls['product'].patchValue(
      this.products.filter((item1) =>
        this.invoice.invoiceProducts
          .filter((x) => !x.deleted)
          .some((item2) => item2.productId === item1.id)
      )
    );

    this.invoiceForm.controls['service'].patchValue(
      this.services.filter((item1) =>
        this.invoice.invoiceProducts
          .filter((x) => !x.deleted)
          .some((item2) => item2.serviceId === item1.id)
      )
    );

    this.invoiceProductItems = invoice.invoiceProducts;
    this.invoiceProducts = this.invoiceForm.get('invoiceProducts') as FormArray;
    var invoiceProductsData = this.invoiceProductItems.filter(
      (x) => x.productId != null
    );
    var invoiceServicesData = this.invoiceProductItems.filter(
      (x) => x.serviceId != null
    );
    for (let i = 0; i < invoiceProductsData.length; i++) {
      this.invoiceProducts.push(
        this.createinvoiceProductItem(
          invoiceProductsData[i].productId,
          this.products.filter(
            (x) => x.id == invoiceProductsData[i].productId
          )[0].productName,
          this.products.filter(
            (x) => x.id == invoiceProductsData[i].productId
          )[0].stock,
          invoiceProductsData[i].id,
          invoiceProductsData[i].purchaseInvoiceId,
          invoiceProductsData[i].salesInvoiceId,
          invoiceProductsData[i].quantity,
          invoiceProductsData[i].price
        )
      );
    }
    for (let i = 0; i < invoiceServicesData.length; i++) {
      this.invoiceProducts.push(
        this.createinvoiceServiceItem(
          invoiceServicesData[i].serviceId,
          this.services.filter(
            (x) => x.id == invoiceServicesData[i].serviceId
          )[0].name,
          invoiceServicesData[i].id,
          invoiceServicesData[i].purchaseInvoiceId,
          invoiceServicesData[i].salesInvoiceId,
          invoiceServicesData[i].quantity,
          invoiceServicesData[i].price
        )
      );
    }

    this.invoiceForm.controls['id'].setValue(invoice.id);
    this.invoiceForm.controls['isTax'].setValue(invoice.isTax);
    this.invoiceForm.controls['isTax'].disable();
    if (invoice.isTax && this.invoiceType == invoiceType.Purchase) {
      this.invoiceForm.controls['invoiceNumber'].setValidators(
        Validators.required
      );
      this.invoiceForm.controls['invoiceNumber'].updateValueAndValidity();
    }

    this.invoiceType == invoiceType.Purchase
      ? this.invoiceForm.controls['supplier'].setValue(
          this.customers.filter((x) => x.id == invoice.supplierId)[0]
        )
      : this.invoiceForm.controls['client'].setValue(
          this.customers.filter((x) => x.id == invoice.clientId)[0]
        );

    this.invoiceForm.controls['invoiceDate'].setValue(
      new Date(invoice.invoiceDate)
    );

    if (this.invoiceType == invoiceType.Purchase) {
      this.invoiceForm.controls['invoiceNumber'].setValue(
        invoice.purchaseInvoiceNumber
      );
    } else {
      this.invoiceForm.controls['invoiceNumber'].setValue(
        invoice.salesInvoiceNumber
      );
    }

    this.invoiceType == invoiceType.Sales
      ? this.invoiceForm.controls['brokerName'].setValue(invoice.brokerName)
      : null;

    this.invoiceForm.controls['tax'].setValue(invoice.tax);
    this.invoiceForm.controls['transfer'].setValue(invoice.transfer);

    this.invoiceType == invoiceType.Purchase
      ? this.invoiceForm.controls['additionalFees'].setValue(
          invoice.additionalFees
        )
      : this.invoiceForm.controls['transportation'].setValue(
          invoice.transportaion
        );
    this.invoiceForm.controls['note'].setValue(invoice.note);
    this.resetInvoiceProducts();
    this.resetInvoiceServices();
  }
  collectFormData() {
    const model: IInvoiceEditor = {
      id: this.invoiceForm.controls['id'].value,
      supplierId:
        this.invoiceType == invoiceType.Purchase
          ? this.invoiceForm.controls['supplier'].value.id
          : null,
      clientId:
        this.invoiceType == invoiceType.Sales
          ? this.invoiceForm.controls['client'].value.id
          : null,
      invoiceDate: this.invoiceForm.controls['invoiceDate'].value,

      purchaseInvoiceNumber:
        this.invoiceType == invoiceType.Purchase
          ? this.invoiceForm.controls['invoiceNumber'].value
          : null,
      salesInvoiceNumber:
        this.invoiceType == invoiceType.Sales
          ? this.invoiceForm.controls['invoiceNumber'].value
          : null,
      brokerName:
        this.invoiceType == invoiceType.Sales
          ? this.invoiceForm.controls['brokerName'].value
          : '',
      tax: this.invoiceForm.controls['tax'].value,
      transfer: this.invoiceForm.controls['transfer'].value,
      additionalFees:
        this.invoiceType == invoiceType.Purchase
          ? this.invoiceForm.controls['additionalFees'].value
          : null,
      transportation:
        this.invoiceType == invoiceType.Sales
          ? this.invoiceForm.controls['transportation'].value
          : null,
      invoiceProducts: this.invoiceForm.controls['invoiceProducts'].value,
      note: this.invoiceForm.controls['note'].value,
      deletedIds: [],
      archiveId: this.ArchiveId,
      isTax: this.invoiceForm.controls['isTax'].value,
    };
    this.invoiceEditor = model;
  }

  createInvoiceForm() {
    this.invoiceForm = this.formBuilder.group({
      id: new FormControl(0),
      supplier: new FormControl(null),
      client: new FormControl(null),
      invoiceDate: new FormControl('', [Validators.required]),
      invoiceNumber: new FormControl(null),
      brokerName: new FormControl(''),
      product: new FormControl(''),
      service: new FormControl(''),
      invoiceProducts: this.formBuilder.array([]),
      tax: new FormControl(14, [
        Validators.required,
        Validators.max(100),
        Validators.min(0),
      ]),
      transfer: new FormControl(0, [
        Validators.required,
        Validators.max(100),
        Validators.min(0),
      ]),
      additionalFees: new FormControl(null, Validators.min(0)),
      transportation: new FormControl(null, Validators.min(0)),
      note: new FormControl(''),
      isTax: new FormControl(true),
    });
  }
  createinvoiceProductItem(
    productId,
    productName,
    stock,
    id,
    purchaseInvoiceId,
    salesInvoiceId,
    quantity,
    price
  ): FormGroup {
    return this.formBuilder.group({
      id: new FormControl(id),
      purchaseInvoiceId: new FormControl(purchaseInvoiceId),
      salesInvoiceId: new FormControl(salesInvoiceId),
      productId: new FormControl(productId),
      productName: new FormControl(productName),
      serviceid: new FormControl(null),
      serviceName: new FormControl(''),
      quantity: new FormControl(quantity, [
        Validators.required,
        Validators.max(
          this.invoiceType == invoiceType.Sales
            ? this.invoiceId == 0
              ? stock
              : stock -
                this.invoiceProductItems.find((x) => x.id == id).quantity
            : 1000000
        ),
        Validators.min(1),
      ]),
      price: new FormControl(price, [Validators.required]),
    });
  }
  createinvoiceServiceItem(
    serviceId,
    serviceName,
    id,
    purchaseInvoiceId,
    salesInvoiceId,
    quantity,
    price
  ): FormGroup {
    return this.formBuilder.group({
      id: new FormControl(id),
      purchaseInvoiceId: new FormControl(purchaseInvoiceId),
      salesInvoiceId: new FormControl(salesInvoiceId),
      productId: new FormControl(null),
      productName: new FormControl(''),
      serviceId: new FormControl(serviceId),
      serviceName: new FormControl(serviceName),
      quantity: new FormControl(quantity, [Validators.required]),
      price: new FormControl(price, [Validators.required]),
    });
  }

  resetInvoiceProducts() {
    var missinginvoiceProducts = this.invoiceForm.controls[
      'product'
    ].value.filter(
      (item1) =>
        !this.invoiceForm.controls['invoiceProducts'].value.some(
          (item2) => item2.productId === item1.id
        )
    );

    for (var i = 0, len = missinginvoiceProducts.length; i < len; i++) {
      this.invoiceProducts = this.invoiceForm.get(
        'invoiceProducts'
      ) as FormArray;
      this.invoiceProducts.push(
        this.createinvoiceProductItem(
          missinginvoiceProducts[i].id,
          missinginvoiceProducts[i].productName,
          missinginvoiceProducts[i].stock,
          0,
          0,
          0,
          1,
          null
        )
      );
    }
    var missingProducts = this.invoiceForm.controls[
      'invoiceProducts'
    ].value.filter(
      (item1) =>
        item1.productId != null &&
        !this.invoiceForm.controls['product'].value.some(
          (item2) => item2.id === item1.productId
        )
    );

    for (var i = 0, len = missingProducts.length; i < len; i++) {
      var deletedItem = this.invoiceForm.controls[
        'invoiceProducts'
      ].value.filter((item) => item.productId == missingProducts[i].productId);
      var deletedIndex = this.invoiceProducts.value
        .map(function (e) {
          return e.productId;
        })
        .indexOf(deletedItem[0].productId);
      this.invoiceProducts.removeAt(deletedIndex);
      if (this.invoiceId) {
        this.deletedInvoiceProductOrServicesIds.push(deletedItem[0].id);
      }
    }
    this.invoiceProductItems =
      this.invoiceForm.controls['invoiceProducts'].value;
    this.invoiceProductItemsSource.next(this.invoiceProductItems);
    this.updateTotalInvoiceAmount();
  }

  resetInvoiceServices() {
    var missinginvoiceServices = this.invoiceForm.controls[
      'service'
    ].value.filter(
      (item1) =>
        !this.invoiceForm.controls['invoiceProducts'].value.some(
          (item2) => item2.serviceId === item1.id
        )
    );

    for (var i = 0, len = missinginvoiceServices.length; i < len; i++) {
      this.invoiceProducts = this.invoiceForm.get(
        'invoiceProducts'
      ) as FormArray;
      this.invoiceProducts.push(
        this.createinvoiceServiceItem(
          missinginvoiceServices[i].id,
          missinginvoiceServices[i].name,
          0,
          0,
          0,
          1,
          null
        )
      );
    }
    var missingServices = this.invoiceForm.controls[
      'invoiceProducts'
    ].value.filter(
      (item1) =>
        item1.serviceId != null &&
        !this.invoiceForm.controls['service'].value.some(
          (item2) => item2.id === item1.serviceId
        )
    );

    for (var i = 0, len = missingServices.length; i < len; i++) {
      var deletedItem = this.invoiceForm.controls[
        'invoiceProducts'
      ].value.filter((item) => item.serviceId == missingServices[i].serviceId);
      var deletedIndex = this.invoiceProducts.value
        .map(function (e) {
          return e.serviceId;
        })
        .indexOf(deletedItem[0].serviceId);
      this.invoiceProducts.removeAt(deletedIndex);
      if (this.invoiceId) {
        this.deletedInvoiceProductOrServicesIds.push(deletedItem[0].id);
      }
    }
    this.invoiceProductItems =
      this.invoiceForm.controls['invoiceProducts'].value;
    this.invoiceProductItemsSource.next(this.invoiceProductItems);

    this.updateTotalInvoiceAmount();
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control) => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  onSubmit() {
    this.markFormGroupTouched(this.invoiceForm);
    if (
      this.invoiceForm.controls['service'].value.length == 0 &&
      this.invoiceForm.controls['product'].value.length == 0
    ) {
      this.toastr.error('You must insert at least one product or service');
    }

    if (this.invoiceForm.valid) {
      this.collectFormData();
      this.saveInvoice();
    }
  }

  saveInvoice() {
    this.invoiceEditor.deletedIds = this.deletedInvoiceProductOrServicesIds;
    this.invoiceService
      .saveInvoice(this.invoiceType, this.invoiceEditor, this.invoiceId)
      .subscribe(
        (res) => {
          this.toastr.success('Invoice saved successfully');
          this.router.navigateByUrl(
            `/${
              this.invoiceType == invoiceType.Purchase
                ? `purchase-invoice/view/${res.id}`
                : `sales-invoice/view/${res.id}`
            }`
          );
        },
        (err) => {
          console.log(err);
        }
      );
  }

  getSuppliersLookup() {
    this.invoiceService.getSuppliersLookup().subscribe(
      (res) => {
        this.customers = res;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getClientsLookup() {
    this.invoiceService.getClientsLookup().subscribe(
      (res) => {
        this.customers = res;
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getProductsLookup() {
    this.invoiceService.getProductsLookup().subscribe(
      (res) => {
        this.products = res;
        this.getServicesLookup();
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getServicesLookup() {
    this.invoiceService.getServicesLookup().subscribe(
      (res) => {
        this.services = res;
        this.invoiceForm.controls['service'].value;
        if (this.invoiceId > 0) {
          this.loadInvoice();
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }
  updateTotalInvoiceAmount() {
    if (this.invoiceForm.controls['invoiceProducts'].value.length > 0) {
      this.totalInvoiceAmount = this.invoiceForm.controls[
        'invoiceProducts'
      ].value
        .map((a) => a.price * a.quantity)
        .reduce(function (a, b) {
          return a + b;
        });
    }
  }

  isTax(e) {
    if (e.target.checked && this.invoiceType == invoiceType.Purchase) {
      this.invoiceForm.controls['invoiceNumber'].setValidators(
        Validators.required
      );
      this.invoiceForm.controls['invoiceNumber'].updateValueAndValidity();
      this.invoiceForm.controls['invoiceNumber'].enable();
    } else {
      this.invoiceForm.controls['invoiceNumber'].setValidators(null);
      this.invoiceForm.controls['invoiceNumber'].updateValueAndValidity();
      this.invoiceForm.controls['invoiceNumber'].disable();
    }
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    this.formData.append('file', fileToUpload, fileToUpload.name);
  };
  attachArchieveId(e) {
    this.ArchiveId = e;
  }
}
