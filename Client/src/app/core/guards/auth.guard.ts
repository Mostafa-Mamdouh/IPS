import { Injectable } from '@angular/core';

import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { Observable, of } from 'rxjs';

import { AccountService } from 'src/app/account/account.service';
import { SecurityService } from '../security/security.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private securityService: SecurityService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const permission = route.data.Permission;
    var auth = this.accountService.securityObject;
    if (auth.isAuthenticated && this.securityService.hasClaim(permission)) {
      return true;
    }
    this.accountService.resetSecurityObject();
    this.router.navigate(['account/login'], {
      queryParams: { returnUrl: state.url },
    });
  }
}
