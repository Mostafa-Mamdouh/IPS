import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IPagination, Pagination } from '../shared/models/pagination';
import {
  IChangePassword,
  IResetPassword,
  IUser,
  IUserEditor,
  UserParams,
} from '../shared/models/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination();
  users: IUser[] = [];
  user: IUser;

  userEditor: IUser;
  constructor(private http: HttpClient) {}

  activateUser(id: number, isActive: boolean) {
    let params = new HttpParams();
    params = params.append('id', id.toString());
    params = params.append('isActive', `${isActive}`);

    return this.http.get<IUser>(this.baseUrl + `users/activate`, {
      observe: 'response',
      params,
    });
  }

  changePasswors(values: IChangePassword) {
    return this.http
      .post<IUser>(this.baseUrl + `users/change-password`, values)
      .pipe(
        map((response) => {
          this.user = response;
          return this.user;
        })
      );
  }
  resetPassword(values: IResetPassword) {
    return this.http
      .post<IUser>(this.baseUrl + `users/forget-password`, values)
      .pipe(
        map((response) => {
          this.user = response;
          return this.user;
        })
      );
  }
  getUserById(id: number) {
    return this.http.get<IUser>(this.baseUrl + `users/` + id);
  }
  getUsers(userParams: UserParams) {
    let params = new HttpParams();

    if (userParams.search) {
      params = params.append('search', userParams.search);
    }

    params = params.append('sort', userParams.sort);
    params = params.append('pageIndex', userParams.pageNumber.toString());
    params = params.append('pageSize', userParams.pageSize.toString());

    return this.http
      .get<IPagination>(this.baseUrl + `users`, {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }
  exportUsers(userParams: UserParams) {
    let params = new HttpParams();

    params = params.append('pageIndex', userParams.pageNumber.toString());
    params = params.append('pageSize', userParams.pageSize.toString());
    return this.http
      .get(this.baseUrl + `users/export`, {
        responseType: 'blob',
        params: params,
      })
      .pipe();
  }
  saveUser(values: IUserEditor, id: number) {
    return this.http
      .post<IUser>(
        this.baseUrl + `users/${id > 0 ? 'update' : 'create'}`,
        values
      )
      .pipe(
        map((response) => {
          this.user = response;
          return this.user;
        })
      );
  }
  deleteUser(id: number) {
    return this.http.delete<IUser>(this.baseUrl + `users/${id}`).pipe(
      map((response) => {
        this.user = response;
        return this.user;
      })
    );
  }

  userExist(email: string, id: number) {
    let params = new HttpParams();
    params = params.append('email', email);
    params = params.append('id', id.toString());

    return this.http
      .get<boolean>(this.baseUrl + `users/emailexists`, {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          return response;
        })
      );
  }
}
