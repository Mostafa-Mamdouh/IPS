import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;

  securityObject: IUser = new IUser();

  private currentUserSource = new BehaviorSubject<IUser>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private route: Router) {}
  getCurrentUserValue() {
    return this.currentUserSource.value;
  }
  loadCurrentUser(token: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get(this.baseUrl + 'account', { headers }).pipe(
      map((user: IUser) => {
        if (user.userId > 0) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
          Object.assign(this.securityObject, user);
        } else {
          this.currentUserSource.next(null);
          this.route.navigateByUrl('/account/login', {
            skipLocationChange: true,
          });
        }
      })
    );
  }

  login(values: any) {
    this.resetSecurityObject();
    return this.http.post(this.baseUrl + 'account/login', values).pipe(
      map((user: IUser) => {
        if (user.emailConfirmed) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
          Object.assign(this.securityObject, user);
        }
        return user;
      })
    );
  }

  logout() {
    this.resetSecurityObject();
    this.currentUserSource.next(null);
    this.route.navigateByUrl('/account/login');
  }

  resetSecurityObject(): void {
    this.currentUserSource.next(null);
    localStorage.removeItem('token');
    this.securityObject.isAuthenticated = false;
    this.securityObject.claims = [];
    this.securityObject = new IUser();
    this.securityObject.isAuthenticated = false;
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }
}
