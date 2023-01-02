import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from './account/account.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'IPS';

  constructor(
    private accountService: AccountService,
    private route: Router,
    private Location: Location
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    if (token) {
      this.accountService.loadCurrentUser(token).subscribe(
        (res) => {
          console.log('Loaded User ' + res);
        },
        (error) => {
          this.route.navigateByUrl('/account/login', {
            skipLocationChange: true,
          });
        }
      );
    } else {
      if (this.Location.path().includes('change-password')) {
        this.route.navigateByUrl(this.Location.path(), {
          skipLocationChange: true,
        });
      } else {
        this.route.navigateByUrl('/account/login', {
          skipLocationChange: true,
        });
      }
    }
  }
}
