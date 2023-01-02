import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  ICustomerEditor,
  ICustomerLookup,
} from 'src/app/shared/models/customer';
import { customerType } from 'src/app/shared/models/Enum';
import { Ilookup } from 'src/app/shared/models/lookup';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'app-customer-editor',
  templateUrl: './customer-editor.component.html',
  styleUrls: ['./customer-editor.component.scss'],
})
export class CustomerEditorComponent implements OnInit {
  customerForm: FormGroup;
  customerEditor: ICustomerEditor;
  customer: ICustomerLookup;
  customerType: customerType;
  customerId = 0;
  countries: Ilookup[];
  cities: Ilookup[];

  constructor(
    private customerService: CustomerService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.customerId = +this.activatedroute.snapshot.paramMap.get('id');
    this.activatedroute.data.subscribe((data) => {
      this.customerType = data.type;
    });
    this.createCustomerForm();
    this.getCountriesLookup();
  }

  createCustomerForm() {
    this.customerForm = this.formBuilder.group({
      id: new FormControl(0),
      name: new FormControl('', [Validators.required]),
      city: new FormControl(null, [Validators.required]),
      country: new FormControl(null, [Validators.required]),
      address: new FormControl(''),
      taxReferenceNumber: new FormControl(''),
      representativeName: new FormControl('', [Validators.required]),
      email: new FormControl('', [
        Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$'),
      ]),
      mobileNumber: new FormControl('', [Validators.required]),
    });
  }

  collectFormData() {
    const model: ICustomerEditor = {
      id: this.customerForm.controls['id'].value,
      name: this.customerForm.controls['name'].value,
      cityId: this.customerForm.controls['city'].value.id,
      address: this.customerForm.controls['address'].value,
      taxReferenceNumber:
        this.customerForm.controls['taxReferenceNumber'].value,
      representativeName:
        this.customerForm.controls['representativeName'].value,
      email: this.customerForm.controls['email'].value,
      mobileNumber: this.customerForm.controls['mobileNumber'].value,
    };
    this.customerEditor = model;
  }
  assignFormData(customer: ICustomerLookup) {
    this.customerForm.controls['id'].setValue(customer.id);
    this.customerForm.controls['name'].setValue(customer.name);
    this.customerForm.controls['address'].setValue(customer.address);
    this.customerForm.controls['country'].setValue(
      this.countries.filter((x) => x.id == customer.countryId)[0]
    );
    this.getCitiesByCountryId(
      this.countries.filter((x) => x.id == customer.countryId)[0]
    );
    this.customerForm.controls['taxReferenceNumber'].setValue(
      customer.taxReferenceNumber
    );
    this.customerForm.controls['representativeName'].setValue(
      customer.representativeName
    );
    this.customerForm.controls['email'].setValue(customer.email);
    this.customerForm.controls['mobileNumber'].setValue(customer.mobileNumber);
  }
  loadCustomer() {
    this.customerService
      .getCustomerById(
        this.customerType,
        +this.activatedroute.snapshot.paramMap.get('id')
      )
      .subscribe(
        (res) => {
          this.customer = res;
          if (this.customer) this.assignFormData(this.customer);
        },
        (err) => {
          console.log(err.message);
        }
      );
  }

  onSubmit() {
    this.markFormGroupTouched(this.customerForm);
    if (this.customerForm.valid) {
      this.customerService
        .customerExist(
          this.customerType,
          this.customerForm.controls['name'].value,
          this.customerForm.controls['id'].value,
          this.customerForm.controls['taxReferenceNumber'].value
        )
        .subscribe(
          (res) => {
            if (!res.body) {
              this.collectFormData();
              this.saveCustomer();
            } else {
              this.toastr.error(
                `${
                  this.customerType == customerType.Client
                    ? 'Client'
                    : 'Supplier'
                } is already exist`
              );
            }
          },
          (err) => {
            console.log(err.message);
          }
        );
    }
  }

  saveCustomer() {
    this.customerService
      .saveCustomer(this.customerType, this.customerEditor, this.customerId)
      .subscribe(
        (res) => {
          this.toastr.success(
            `${
              this.customerType == customerType.Client ? 'Client' : 'Supplier'
            } saved successfully`
          );
          this.router.navigateByUrl(
            `/${
              this.customerType == customerType.Client ? `client` : `supplier`
            }`
          );
        },
        (err) => {
          console.log(err);
        }
      );
  }

  getCountriesLookup() {
    this.customerService.getCountriesLookup().subscribe(
      (res) => {
        this.countries = res;

        if (this.customerId > 0) {
          this.loadCustomer();
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getCitiesByCountryId(e) {
    if (e) {
      this.customerService.getCitiesByCountryIdLookup(e.id).subscribe(
        (res) => {
          this.cities = res;
          if (this.customerId > 0) {
            this.customerForm.controls['city'].setValue(
              this.cities.filter((x) => x.id == this.customer.cityId)[0]
            );
          }
        },
        (error) => {
          console.log(error);
        }
      );
    } else {
      this.cities = [];
    }
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control) => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
