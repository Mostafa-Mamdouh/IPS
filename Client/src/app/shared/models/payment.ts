import { invoiceType } from './Enum';

export class IPayment {
    remaingAmount: number;
    invoiceDate: Date;
    salesInvoiceId:number;
    purchaseInvoiceId:number;
    invoiceType:invoiceType;
    invoiceCreatedBy:string;
    deleted:number;
  }
  

