export class InventoryParams {
  createFromDate: Date=null;
  createToDate: Date=null;
  categoryId:number=null;
  sort = 'CreateDate';
  SortDirection='Desc';
  pageNumber = 1;
  pageSize = 10;
  search: string=null;
}
