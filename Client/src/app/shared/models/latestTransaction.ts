export interface IlatestTransaction {
  id: number;
  paymentId: number;
  createUserId: number;
  updateUserId: number;
  createDate: Date;
  updateDate: Date;
  deleted: boolean;
  type: string;
  purchaseInvoiceNumber: string;
  salesInvoiceNumber: number;
  invoiceId: number;
  brokerName: string;
  description: string;
  date: Date;
  credit: number;
  debit: number;
  current: number;
  total_Count: number;
}

export class LatestTransactionParams {
  pageNumber = 1;
  pageSize = 10;
}
