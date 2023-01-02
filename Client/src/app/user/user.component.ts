import {
  Component,
  ElementRef,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { IUser, UserParams } from '../shared/models/user';
import { UserService } from './user.service';
import * as moment from 'moment';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
})
export class UserComponent implements OnInit {
  public totalItems = 64;
  @ViewChild('search', { static: false }) searchTerm: ElementRef;

  users: IUser[] = [];
  userParams = new UserParams();
  exportParams = new UserParams();
  totalCount: number;
  deletemodalRef: BsModalRef;
  deleteusermodalRef: BsModalRef;

  constructor(
    private userService: UserService,
    private activatedroute: ActivatedRoute,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.userService.getUsers(this.userParams).subscribe(
      (res) => {
        this.users = res.data;

        this.userParams.pageNumber = res.pageIndex;
        this.userParams.pageSize = res.pageSize;
        this.totalCount = res.count;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  onPageChanged(event: any) {
    if (this.userParams.pageNumber !== event) {
      this.userParams.pageNumber = event;
      this.getUsers();
    }
  }

  onSearch() {
    this.userParams.search = this.searchTerm.nativeElement.value;
    this.userParams.pageNumber = 1;
    this.getUsers();
  }

  Export() {
    this.exportParams.pageNumber = 1;
    this.exportParams.pageSize = 1000000;

    this.userService.exportUsers(this.exportParams).subscribe(
      (data: Blob) => {
        var fileName = 'Users_list_' + Date.now() + '';
        FileSaver.saveAs(data, fileName);
      },
      (err: any) => {
        console.log(`Unable to export file`);
      }
    );
  }

  openDeleteModal(template: TemplateRef<any>) {
    this.deletemodalRef = this.modalService.show(template);
  }
  openDeleteUserModal(template: TemplateRef<any>) {
    this.deleteusermodalRef = this.modalService.show(template);
  }

  ActivateUser(id: number, isActive: boolean) {
    this.userService.activateUser(id, isActive).subscribe(
      () => {
        this.deletemodalRef.hide();
        this.toastr.success(
          `USer has been ${isActive ? 'DeActivated' : 'Activated'} successfully`
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

  DeleteUser(id: number) {
    this.userService.deleteUser(id).subscribe(
      (res) => {
        debugger;
        console.log(res);
        this.deleteusermodalRef.hide();
        this.toastr.success(`User has been deleted successfully`);
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
