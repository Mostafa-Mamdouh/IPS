export interface ICustomerLookup {
    id: number;
    name: string;
    city: string;
    cityId: number;
    countryId: number;
    address: string;
    taxReferenceNumber: string;
    representativeName: string;
    email: string;
    mobileNumber: string;
    cancelled: boolean;
    createdBy:string;
    createDate:Date;
}

export class CustomerParams {
    createFromDate: Date=null;
    createToDate: Date=null;
    invoiceFromDate: Date=null;
    invoiceToDate: Date=null;
    sort = null;
    pageNumber = 1;
    pageSize = 10;
    search: string=null;
  }

  
  export interface ICustomerEditor {
    id: number;
    name: string;
    cityId: number;
    address: string;
    taxReferenceNumber: string;
    representativeName: string;
    email: string;
    mobileNumber: string;
}