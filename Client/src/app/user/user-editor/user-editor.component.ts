import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IClaims, IUser, IUserEditor } from 'src/app/shared/models/user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user-editor',
  templateUrl: './user-editor.component.html',
  styleUrls: ['./user-editor.component.scss'],
})
export class UserEditorComponent implements OnInit {
  userForm: FormGroup;
  userEditor: IUserEditor;
  userClaims: IClaims[] = [];
  selectedClaims: IClaims[] = [];
  rolesEditor: string[] = [
    'Users',
    'Clients',
    'Suppliers',
    'Inventory',
    'Expenses',
    'Purchasing',
    'Sales',
    'Transaction',
  ];
  claimsEditor: IClaims[] = [
    { type: 'Users', value: 'AddUser', displayName: 'Add User' },
    { type: 'Users', value: 'EditUser', displayName: 'Edit User' },
    { type: 'Users', value: 'ViewUser', displayName: 'List/View Users' },
    {
      type: 'Users',
      value: 'ActivateUser',
      displayName: 'Activate/Deactivate User',
    },

    { type: 'Clients', value: 'AddClient', displayName: 'Add Client' },
    { type: 'Clients', value: 'EditClient', displayName: 'Edit Client' },
    { type: 'Clients', value: 'ViewClient', displayName: 'List/View Clients' },
    { type: 'Clients', value: 'DeleteClient', displayName: 'Delete Client' },

    { type: 'Suppliers', value: 'AddSupplier', displayName: 'Add Supplier' },
    { type: 'Suppliers', value: 'EditSupplier', displayName: 'Edit Supplier' },
    {
      type: 'Suppliers',
      value: 'ViewSupplier',
      displayName: 'List/View Suppliers',
    },
    {
      type: 'Suppliers',
      value: 'DeleteSupplier',
      displayName: 'Delete Supplier',
    },

    { type: 'Inventory', value: 'AddProduct', displayName: 'Add Product' },
    { type: 'Inventory', value: 'EditProduct', displayName: 'Edit Product' },
    {
      type: 'Inventory',
      value: 'ViewProduct',
      displayName: 'List/View Products',
    },
    {
      type: 'Inventory',
      value: 'DeleteProduct',
      displayName: 'Delete Product',
    },

    { type: 'Inventory', value: 'AddService', displayName: 'Add Service' },
    { type: 'Inventory', value: 'EditService', displayName: 'Edit Service' },
    {
      type: 'Inventory',
      value: 'ViewService',
      displayName: 'List/View Services',
    },
    {
      type: 'Inventory',
      value: 'DeleteService',
      displayName: 'Delete Service',
    },

    { type: 'Expenses', value: 'AddExpense', displayName: 'Add Expense' },
    { type: 'Expenses', value: 'EditExpense', displayName: 'Edit Expense' },
    {
      type: 'Expenses',
      value: 'ViewExpense',
      displayName: 'List/View Expenses',
    },
    { type: 'Expenses', value: 'DeleteExpense', displayName: 'Delete Expense' },

    {
      type: 'Purchasing',
      value: 'AddPurchase',
      displayName: 'Add Purchasing Invoice',
    },
    {
      type: 'Purchasing',
      value: 'EditPurchase',
      displayName: 'Edit Purchasing Invoice',
    },
    {
      type: 'Purchasing',
      value: 'ViewPurchase',
      displayName: 'List/View Purchasing Invoices',
    },
    {
      type: 'Purchasing',
      value: 'CancelPurchase',
      displayName: 'Cancel Purchasing Invoice',
    },
    {
      type: 'Purchasing',
      value: 'AddPurchasePayment',
      displayName: 'Add Purchasing Invoice Payment',
    },
    {
      type: 'Purchasing',
      value: 'EditPurchasePayment',
      displayName: 'Edit Purchasing Invoice Payment',
    },
    {
      type: 'Purchasing',
      value: 'DeletePurchasePayment',
      displayName: 'Delete Purchasing Invoice Payment',
    },

    { type: 'Sales', value: 'AddSales', displayName: 'Add Sales Invoice' },
    { type: 'Sales', value: 'EditSales', displayName: 'Edit Sales Invoice' },
    {
      type: 'Sales',
      value: 'ViewSales',
      displayName: 'List/View Sales Invoices',
    },
    {
      type: 'Sales',
      value: 'CancelSales',
      displayName: 'Cancel Sales Invoice',
    },
    {
      type: 'Sales',
      value: 'AddSalesPayment',
      displayName: 'Add Sales Invoice Payment',
    },
    {
      type: 'Sales',
      value: 'EditSalesPayment',
      displayName: 'Edit Sales Invoice Payment',
    },
    {
      type: 'Sales',
      value: 'DeleteSalesPayment',
      displayName: 'Delete Sales Invoice Payment',
    },
    {
      type: 'Transaction',
      value: 'ViewTransaction',
      displayName: 'List/View Transactions',
    },
  ];
  userId = 0;
  user: IUser;
  constructor(
    private userService: UserService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.createUserForm();
    if (+this.activatedroute.snapshot.paramMap.get('id')) {
      this.userId = +this.activatedroute.snapshot.paramMap.get('id');
      this.loadUser();
    }
  }

  createUserForm() {
    this.userForm = this.formBuilder.group({
      id: new FormControl(0),
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      email: new FormControl('', [
        Validators.required,
        Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$'),
      ]),
      isActive: new FormControl(true, [Validators.required]),
      roles: new FormControl(null, [Validators.required]),
      claims: new FormControl(null, [Validators.required]),
    });
  }

  GetClaims(e) {
    this.selectedClaims = this.claimsEditor.filter((x) => e.includes(x.type));
    this.userForm.controls['claims'].setValue(this.selectedClaims);
  }

  collectFormData() {
    const model: IUserEditor = {
      userId: this.userForm.controls['id'].value,
      email: this.userForm.controls['email'].value,
      firstName: this.userForm.controls['firstName'].value,
      lastName: this.userForm.controls['lastName'].value,
      isActive: this.userForm.controls['isActive'].value,
      claims: this.userForm.controls['claims'].value,
    };
    this.userEditor = model;
  }
  assignFormData(user: IUser) {
    this.userForm.controls['id'].setValue(user.userId);
    this.userForm.controls['email'].setValue(user.email);
    this.userForm.controls['firstName'].setValue(user.firstName);
    this.userForm.controls['lastName'].setValue(user.lastName);
    this.userForm.controls['isActive'].setValue(user.isActive);
    this.userForm.controls['roles'].patchValue(user.claims.map((x) => x.type));
    this.selectedClaims = this.claimsEditor.filter((x) =>
      user.claims.map((x) => x.type).includes(x.type)
    );
    this.userForm.controls['claims'].patchValue(user.claims);
  }

  loadUser() {
    this.userService
      .getUserById(+this.activatedroute.snapshot.paramMap.get('id'))
      .subscribe(
        (res) => {
          this.user = res;
          if (this.user) this.assignFormData(this.user);
        },
        (err) => {
          console.log(err.message);
        }
      );
  }
  onSubmit() {
    this.markFormGroupTouched(this.userForm);

    if (this.userForm.valid) {
      this.userService
        .userExist(this.userForm.controls['email'].value, this.userId)
        .subscribe(
          (res) => {
            var emailExists = res.body;
            if (!emailExists) {
              this.collectFormData();
              this.saveUser();
            } else {
              this.toastr.error('Email already exists');
            }
          },
          (err) => {
            console.log(err.message);
          }
        );
    }
  }

  saveUser() {
    this.userService.saveUser(this.userEditor, this.userId).subscribe(
      (res) => {
        this.toastr.success(`user saved successfully`);
        this.router.navigateByUrl(`/users`);
      },
      (err) => {
        console.log(err);
      }
    );
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
