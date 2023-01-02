export interface IExpense {
  id: number;
  expenseTypeId: number;
  expenseType: string;
  description: string;
  transactionDate: Date;
  amount: number;
  paymentMethod: number;
  transferNumber: string;
  chequeNumber: string;
  createdBy: string;
  createDate: Date;
}

export interface IExpenseEditor {
  id: number;
  expenseTypeId: number;
  description: string;
  transactionDate: Date;
  amount: number;
  paymentMethod: number;
  chequeNumber: string;
  transferNumber: string;
}

export class ExpenseParams {
  transactionFromDate: Date = null;
  transactionToDate: Date = null;
  sort = null;
  pageNumber = 1;
  pageSize = 10;
  search: string = null;
}
