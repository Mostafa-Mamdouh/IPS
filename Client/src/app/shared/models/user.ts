export class IUser {
  userId: number;
  isAuthenticated: boolean;
  isActive: boolean;
  emailConfirmed: boolean;
  firstName: string;
  lastName: string;
  email: string;
  token: string;
  displayName: string;
  createdBy: string;
  creationDate: Date;
  claims: IClaims[];
}
export interface IUserEditor {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  claims: IClaims[];
}

export interface IClaims {
  value: string;
  type: string;
  displayName: string;
}

export interface IChangePassword {
  id: number;
  oldPassword: string;
  newPassword: string;
  token: string;
}
export interface IResetPassword {
  email: string;
}

export class UserParams {
  sort = null;
  pageNumber = 1;
  pageSize = 10;
  search: string = null;
}
