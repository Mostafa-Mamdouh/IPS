import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IClaims, IUser } from 'src/app/shared/models/user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss'],
})
export class UserDetailComponent implements OnInit {
  user: IUser;
  userPermissions: any;
  constructor(
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService,
    private activatedroute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    this.userService
      .getUserById(+this.activatedroute.snapshot.paramMap.get('id'))
      .subscribe(
        (res) => {
          this.user = res;
          this.userPermissions = this.user.claims.reduce((r, a) => {
            r[a.type] = [...(r[a.type] || []), a];
            return r;
          }, {});
        },
        (err) => {
          console.log(err.message);
        }
      );
  }
}
