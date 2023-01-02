import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { inventoryType } from 'src/app/shared/models/Enum';
import { Ilookup } from 'src/app/shared/models/lookup';
import { IInventoryEditor } from 'src/app/shared/models/product';
import { InventoryService } from '../inventory.service';

@Component({
  selector: 'app-inventory-editor',
  templateUrl: './inventory-editor.component.html',
  styleUrls: ['./inventory-editor.component.scss'],
})
export class InventoryEditorComponent implements OnInit {
  inventoryForm: FormGroup;
  inventoryEditor: IInventoryEditor;
  inventoryType: inventoryType;
  inventoryId = 0;
  categories: Ilookup[];
  constructor(
    private inventoryService: InventoryService,
    private router: Router,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.inventoryId = +this.activatedroute.snapshot.paramMap.get('id');
    this.activatedroute.data.subscribe((data) => {
      this.inventoryType = data.type;
    });
    this.createInventoryForm();

    if (this.inventoryType == inventoryType.Product) {
      this.inventoryForm.controls['code'].setValidators(Validators.required);
      this.inventoryForm.controls['code'].updateValueAndValidity();
    }
    this.getCategoriesLookup();
  }

  createInventoryForm() {
    this.inventoryForm = this.formBuilder.group({
      id: new FormControl(0),
      name: new FormControl('', [Validators.required]),
      code: new FormControl(''),
      description: new FormControl(''),
      category: new FormControl(null, [Validators.required]),
    });
  }

  collectFormData() {
    const model: IInventoryEditor = {
      id: this.inventoryForm.controls['id'].value,
      name: this.inventoryForm.controls['name'].value,
      code:
        this.inventoryType == inventoryType.Product
          ? this.inventoryForm.controls['code'].value
          : '',
      description: this.inventoryForm.controls['description'].value,
      categoryId: this.inventoryForm.controls['category'].value.id,
    };
    this.inventoryEditor = model;
  }
  assignFormData(inventory: IInventoryEditor) {
    this.inventoryForm.controls['id'].setValue(inventory.id);
    this.inventoryForm.controls['category'].setValue(
      this.categories.filter((x) => x.id == inventory.categoryId)[0]
    );
    this.inventoryForm.controls['name'].setValue(inventory.name);

    if (this.inventoryType == inventoryType.Product) {
      this.inventoryForm.controls['code'].setValue(inventory.code);
    }
    this.inventoryForm.controls['description'].setValue(inventory.description);
  }
  loadInventory() {
    this.inventoryService
      .getInventoryById(
        this.inventoryType,
        +this.activatedroute.snapshot.paramMap.get('id')
      )
      .subscribe(
        (res) => {
          this.inventoryEditor = res;
          if (this.inventoryEditor) this.assignFormData(this.inventoryEditor);
        },
        (err) => {
          console.log(err.message);
        }
      );
  }
  onSubmit() {
    this.markFormGroupTouched(this.inventoryForm);

    if (this.inventoryForm.valid) {
      this.inventoryService
        .inventoryExist(
          this.inventoryType,
          this.inventoryForm.controls['category'].value.id,
          this.inventoryForm.controls['code'].value,
          this.inventoryForm.controls['id'].value,
          this.inventoryForm.controls['name'].value
        )
        .subscribe(
          (res) => {
            if (!res.body) {
              this.collectFormData();
              this.saveInventory();
            } else {
              this.toastr.error(
                `${
                  this.inventoryType == inventoryType.Product
                    ? 'Product'
                    : 'Service'
                }  is already exist`
              );
            }
          },
          (err) => {
            console.log(err.message);
          }
        );
    }
  }

  saveInventory() {
    this.inventoryService
      .saveInventory(this.inventoryType, this.inventoryEditor, this.inventoryId)
      .subscribe(
        (res) => {
          this.toastr.success('Product saved successfully');
          this.router.navigateByUrl(
            `/${
              this.inventoryType == inventoryType.Product
                ? `product`
                : `service`
            }`
          );
        },
        (err) => {
          console.log(err);
        }
      );
  }

  getCategoriesLookup() {
    this.inventoryService.getCategoriesLookup().subscribe(
      (res) => {
        this.categories = res;
        if (this.inventoryId > 0) {
          this.loadInventory();
        }
      },
      (error) => {
        console.log(error);
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
