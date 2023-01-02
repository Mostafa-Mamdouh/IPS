export interface IProductLookup {
  id: number;
  createUserId: number;
  updateUserId: number;
  createDate: Date;
  updateDate: Date;
  deleted: boolean;
  productName: string;
  code: string;
  categoryName: string;
  description: string;
  inbound: number;
  outbound: number;
  stock: number;
  createdBy: string;
  totalCount: number;
  displayValue: string;
}

export interface IInventoryEditor {
  id: number;
  name: string;
  code: string;
  description: string;
  categoryId: number;
}
