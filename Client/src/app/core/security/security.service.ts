import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot } from '@angular/router';
import { map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IUser } from 'src/app/shared/models/user';

@Injectable({
  providedIn: 'root',
})
export class SecurityService {
  constructor(private accountService: AccountService, private router: Router) {}
  private isClaimValid(claim: string): boolean {
    let ret: boolean = false;
    let claimValue: string = '';
    let claimType: string = '';
    var auth = this.accountService.securityObject;
    if (auth) {
      if (claim.indexOf(':') >= 0) {
        let words: string[] = claim.split(':');
        claimType = words[0];
        claimValue = words[1];
      } else {
        this.router.navigateByUrl('/account/login', {
          skipLocationChange: true,
        });
      }

      ret =
        auth.claims.find(
          (c) => c.type == claimType && c.displayName == claimValue
        ) != null;
    } else {
      this.router.navigateByUrl('/account/login', {
        skipLocationChange: true,
      });
    }

    return ret;
    // Retrieve security object
  }

  hasClaim(claim: any): boolean {
    let ret: boolean = false;
    // See if an array of values was passed in.
    if (typeof claim === 'string') {
      ret = this.isClaimValid(claim);
    } else {
      let claims: string[] = claim;
      if (claims) {
        for (let index = 0; index < claims.length; index++) {
          ret = this.isClaimValid(claims[index]);
          // If one is successful, then let them in
          if (ret) {
            break;
          }
        }
      }
    }

    return ret;
  }
}
