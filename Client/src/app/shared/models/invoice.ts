export interface IInvoicePayment {
  id: number;
  purchaseInvoiceId: number;
  salesInvoiceId: number;
  type: number;
  amount: number;
  paymentDate: Date;
  paymentMethod: number;
  chequeNumber: string;
  transferNumber: string;
  archiveId: number;
  filePath: string;
  fileName: string;
  createUserId: number;
  createdBy: string;
  createDate: Date;
  deleted: boolean;
}
export interface IInvoicePaymentEditor {
  id: number;
  purchaseInvoiceId: number;
  salesInvoiceId: number;
  type: number;
  amount: number;
  paymentDate: Date;
  paymentMethod: number;
  chequeNumber: string;
  transferNumber: string;
  archiveId: number;
}

export interface IInvoiceProduct {
  id: number;
  purchaseInvoiceId: number;
  salesInvoiceId: number;
  productId: number;
  productName: string;
  serviceId: number;
  serviceName: string;
  quantity: number;
  price: number;
  deleted: boolean;
}

export interface IInvoice {
  id: number;
  supplierId: number;
  clientId: number;
  supplierName: string;
  clientName: string;
  taxReferenceNumber: number;
  invoiceDate: Date;
  salesInvoiceNumber: number;
  purchaseInvoiceNumber: string;
  brokerName: string;
  note: string;
  tax: number;
  transfer: number;
  additionalFees: number;
  transportaion: number;
  archiveId: number;
  filePath: string;
  fileName: string;
  createUserId: number;
  createdBy: string;
  createDate: Date;
  totalInvoice: number;
  totalPaid: number;
  deleted: number;
  isTax: boolean;
  invoicePayments: IInvoicePayment[];
  invoiceProducts: IInvoiceProduct[];
}
export interface IInvoiceEditor {
  id: number;
  supplierId: number;
  clientId: number;
  invoiceDate: Date;
  salesInvoiceNumber: number;
  purchaseInvoiceNumber: string;
  brokerName: string;
  note: string;
  tax: number;
  transfer: number;
  additionalFees: number;
  transportation: number;
  archiveId: number;
  isTax: boolean;
  invoiceProducts: IInvoiceProduct[];
  deletedIds: number[];
}

export interface IInvoiceList {
  id: number;
  createUserId: number;
  updateUserId: number;
  createDate: Date;
  updateDate: Date;
  salesInvoiceNumber: number;
  purchaseInvoiceNumber: string;
  deleted: boolean;
  productName: string;
  code: string;
  categoryName: string;
  description: string;
  inbound: number;
  outbound: number;
  stock: number;
  totalCount: number;
  totalInvoice: number;
  totalPaid: number;
}
