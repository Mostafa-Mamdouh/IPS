using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Reports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
             Create PROCEDURE [dbo].[Sp_ProductSearch] 
             @SearchKeyword nvarchar(500) = null, 
             @CreateFromDate Date = NULL, 
             @CreateToDate Date = NULL, 
             @CategoryID int = null, 
             @Page INT = 1 , 
             @Size INT = 10 , 
             @SortBy NVARCHAR(20) = 'CreateDate', 
             @SortDirection NVARCHAR(10) = 'DESC' 
             AS 
             BEGIN 
             SELECT 
             dbo.Product.Id, 
             dbo.Product.[Name] AS ProductName, 
             dbo.Product.Code, 
             dbo.Category.[Name] AS CategoryName, 
             dbo.Product.[Description], 
            ISNULL((SELECT SUM(PurchaseInvoiceProduct.Quantity) FROM dbo.PurchaseInvoiceProduct 
             WHERE dbo.PurchaseInvoiceProduct.ProductId = dbo.Product.Id and PurchaseInvoiceProduct.Deleted!=1),0)  AS Inbound, 
            ISNULL((SELECT SUM(SalesInvoiceProduct.Quantity) FROM dbo.SalesInvoiceProduct 
             WHERE dbo.SalesInvoiceProduct.ProductId = dbo.Product.Id and SalesInvoiceProduct.Deleted!=1),0)  AS Outbound, 
             (
			ISNULL((SELECT SUM(PurchaseInvoiceProduct.Quantity) FROM dbo.PurchaseInvoiceProduct 
             WHERE dbo.PurchaseInvoiceProduct.ProductId = dbo.Product.Id and PurchaseInvoiceProduct.Deleted!=1),0)  -  
            ISNULL((SELECT SUM(SalesInvoiceProduct.Quantity) FROM dbo.SalesInvoiceProduct 
             WHERE dbo.SalesInvoiceProduct.ProductId = dbo.Product.Id and SalesInvoiceProduct.Deleted!=1) ,0) 
             ) AS Stock, 
             dbo.Product.CreateDate, 
             dbo.Product.UpdateDate, 
             dbo.Product.CreateUserId, 
             dbo.Product.UpdateUserId, 
             dbo.Product.Deleted,
		     concat(AspNetUsers.firstName , ' ', AspNetUsers.lastName) as CreatedBy,
			 ((select COUNT(PurchaseInvoiceProduct.Id) from PurchaseInvoiceProduct where ProductId=Product.Id) + 
			 (select COUNT(SalesInvoiceProduct.Id) from SalesInvoiceProduct where ProductId=Product.Id)
			 ) as TotalInvoicesCount,
             COUNT(*) OVER() AS TotalCount ,
             CONCAT(Product.Name , ' => ',Product.Code) as DisplayValue
             from dbo.Product 
             LEFT JOIN dbo.Category 
             ON Category.Id = Product.CategoryId 
             LEFT JOIN AspNetUsers 
			 on AspNetUsers.Id=Product.CreateUserId
			 
             WHERE 
			 dbo.Product.Deleted=0 and
             (@CreateFromDate is null or (dbo.Product.CreateDate >= @CreateFromDate)) 
             and(@CreateToDate is null or (dbo.Product.CreateDate <= @CreateToDate)) 
             and(@CategoryID IS  NULL OR(dbo.Product.CategoryId = @CategoryID)) 
             and(@SearchKeyword is null or (dbo.Product.[Name] like '%' + @SearchKeyword + '%' or dbo.Product.Code like '%' + @SearchKeyword + '%' or dbo.Category.[Name] like '%' + @SearchKeyword + '%' or dbo.Product.Id like '%' + @SearchKeyword + '%' or AspNetUsers.FirstName like '%' + @SearchKeyword + '%'  or AspNetUsers.LastName like '%' + @SearchKeyword + '%')) 
             ORDER BY 
             CASE WHEN @SortDirection = 'ASC' THEN 
             CASE 
             WHEN @SortBy = 'CreateDate' THEN dbo.Product.CreateDate 
             WHEN @SortBy = 'Id' THEN Product.Id 
             WHEN @SortBy = 'ProductName' THEN Product.[Name] 
             END 
             END ASC
              , CASE WHEN @SortDirection = 'DESC' THEN 
             CASE 
             WHEN @SortBy = 'CreateDate' THEN dbo.Product.CreateDate 
             WHEN @SortBy = 'Id' THEN Product.Id 
             WHEN @SortBy = 'ProductName' THEN Product.[Name] 
             END 
             END DESC 
             OFFSET (@Page -1) * @Size ROWS 
             FETCH NEXT @Size ROWS ONLY END
            ");
            migrationBuilder.Sql(@"
           CREATE VIEW [dbo].[vTotalPurchasePrice] AS

            SELECT 
            
            PurchaseInvoiceId,
            ISNULL(SUM(Price * Quantity ),0) AS TotalPrice,
            ISNULL((SUM(Price * Quantity) * dbo.PurchaseInvoice.Tax)/100,0) TaxAmount,
            ISNULL((SUM(Price * Quantity ) *dbo.PurchaseInvoice.[Transfer])/100,0) TransferAmount,
            ISNULL(SUM(Price * Quantity),0) +
            ISNULL((SUM(Price * Quantity) *dbo.PurchaseInvoice.Tax)/100,0) -
            ISNULL((SUM(Price * Quantity) *dbo.PurchaseInvoice.[Transfer])/100,0)+
            ISNULL(dbo.PurchaseInvoice.AdditionalFees,0) AS TotalInvoice
            FROM dbo.PurchaseInvoiceProduct
            LEFT JOIN dbo.PurchaseInvoice
            ON PurchaseInvoice.Id = PurchaseInvoiceProduct.PurchaseInvoiceId
            where PurchaseInvoiceProduct.Deleted=0 and PurchaseInvoice.Deleted=0
            
            GROUP BY PurchaseInvoiceId,Tax,[Transfer],AdditionalFees
            ");
            migrationBuilder.Sql(@"
           CREATE VIEW [dbo].[vTotalProductPurchasePrice] AS

             SELECT 
             
             PurchaseInvoiceId,
             
             ISNULL(SUM(Price * Quantity),0) AS TotalProductPrice
             
             
             FROM dbo.PurchaseInvoiceProduct
             LEFT JOIN dbo.PurchaseInvoice
             ON PurchaseInvoice.Id = PurchaseInvoiceProduct.PurchaseInvoiceId
             WHERE ServiceId IS NULL and PurchaseInvoiceProduct.Deleted=0
             
             GROUP BY PurchaseInvoiceId  
            ");
            migrationBuilder.Sql(@"
           CREATE VIEW [dbo].[vTotalServicePurchasePrice] AS
            SELECT 
            PurchaseInvoiceId,
            ISNULL(SUM(Price*Quantity),0) AS TotalServicePrice
            FROM dbo.PurchaseInvoiceProduct
            LEFT JOIN dbo.PurchaseInvoice
            ON PurchaseInvoice.Id = PurchaseInvoiceProduct.PurchaseInvoiceId
            WHERE ProductId IS NULL and PurchaseInvoiceProduct.Deleted=0
            GROUP BY PurchaseInvoiceId  
            ");
            migrationBuilder.Sql(@"
            Create VIEW [dbo].[vTotalSalesPrice] AS
           SELECT 
            SalesInvoiceId,
            ISNULL(SUM(Price * Quantity),0) AS TotalPrice,
            ISNULL((SUM(Price * Quantity)* dbo.SalesInvoice.Tax)/100,0) TaxAmount,
            ISNULL((SUM(Price * Quantity)*dbo.SalesInvoice.[Transfer])/100,0) TransferAmount,
            ISNULL(SUM(Price * Quantity),0) +
            ISNULL((SUM(Price * Quantity)*dbo.SalesInvoice.Tax)/100,0) -
            ISNULL((SUM(Price * Quantity)*dbo.SalesInvoice.[Transfer])/100,0)+
            ISNULL(dbo.SalesInvoice.Transportation,0) AS TotalInvoice,
			CreateDate,
			SalesInvoice.InvoiceDate
            FROM dbo.SalesInvoiceProduct
            LEFT JOIN dbo.SalesInvoice
            ON SalesInvoice.Id = SalesInvoiceProduct.SalesInvoiceId
            where  SalesInvoiceProduct.Deleted=0 and SalesInvoice.Deleted=0
            GROUP BY SalesInvoiceId ,Tax,[Transfer],Transportation,CreateDate,InvoiceDate
            GO

            ");
            migrationBuilder.Sql(@"
          CREATE VIEW [dbo].[vTotalProductSalesPrice] AS
          SELECT 
          SalesInvoiceId,
          ISNULL(SUM(Price * Quantity),0) AS TotalProductPrice
          FROM dbo.SalesInvoiceProduct
          LEFT JOIN dbo.SalesInvoice
          ON SalesInvoice.Id = SalesInvoiceProduct.SalesInvoiceId
          WHERE ServiceId IS NULL and SalesInvoiceProduct.Deleted=0
          GROUP BY SalesInvoiceId 
            ");
            migrationBuilder.Sql(@"
          CREATE VIEW [dbo].[vTotalServiceSalesPrice] AS
          SELECT 
          SalesInvoiceId,
          ISNULL(SUM(Price * Quantity),0) AS TotalServicePrice
          FROM dbo.SalesInvoiceProduct
          LEFT JOIN dbo.SalesInvoice
          ON SalesInvoice.Id = SalesInvoiceProduct.SalesInvoiceId
          WHERE ProductId IS NULL and SalesInvoiceProduct.Deleted=0
          GROUP BY SalesInvoiceId 
            ");
            migrationBuilder.Sql(@"
          CREATE VIEW [dbo].[vPurchasePayment]
             AS
             
               SELECT
             PurchaseInvoice.Id as PurchaseInvoiceId,
             ISNULL( SUM(dbo.PurchaseInvoicePayment.Amount),0) AS TotalPaid, 
             ISNULL((SELECT TotalInvoice FROM [dbo].[vTotalPurchasePrice] WHERE PurchaseInvoiceId =  PurchaseInvoice.Id),0)
             - ISNULL( SUM(dbo.PurchaseInvoicePayment.Amount),0) AS Remaining,
             
             CASE
                 WHEN ISNULL( SUM(dbo.PurchaseInvoicePayment.Amount),0)=0 THEN 'Un Paid'
                 WHEN ISNULL((SELECT round(TotalInvoice,2) FROM [dbo].[vTotalPurchasePrice] WHERE PurchaseInvoiceId = PurchaseInvoice.Id),0)
             - ISNULL(round( SUM(dbo.PurchaseInvoicePayment.Amount),2),0)=0  THEN 'Fully Paid '
                 ELSE 'Partially Paid'
             END AS PaymentStatus,
			 PurchaseInvoice.CreateDate
             FROM 
             dbo.PurchaseInvoice  LEFT JOIN dbo.PurchaseInvoicePayment
             ON PurchaseInvoice.Id = PurchaseInvoicePayment.PurchaseInvoiceId
             
             where PurchaseInvoice.Deleted=0
             
             GROUP BY PurchaseInvoice.Id,PurchaseInvoice.CreateDate
            ");
            migrationBuilder.Sql(@"

            CREATE VIEW [dbo].[vSalesPayment]
             AS
             
               SELECT
             SalesInvoice.Id as SalesInvoiceId,
             ISNULL( SUM(dbo.SalesInvoicePayment.Amount),0) AS TotalPaid ,
             ISNULL((SELECT TotalInvoice FROM [dbo].[vTotalSalesPrice] WHERE SalesInvoiceId = SalesInvoice.Id),0)
             - ISNULL( SUM(dbo.SalesInvoicePayment.Amount),0) AS Remaining,
             
             CASE
                 WHEN ISNULL( SUM(dbo.SalesInvoicePayment.Amount),0)=0 THEN 'Un Paid'
                 WHEN ISNULL((SELECT round(TotalInvoice,0) FROM [dbo].[vTotalSalesPrice] WHERE SalesInvoiceId = SalesInvoice.Id),0)
             - ISNULL( round(SUM(dbo.SalesInvoicePayment.Amount),0),0)=0  THEN 'Fully Paid '
                 ELSE 'Partially Paid'
             END AS PaymentStatus,
			 SalesInvoice.CreateDate
             
             FROM 
             dbo.SalesInvoice  LEFT JOIN dbo.SalesInvoicePayment
             ON SalesInvoice.Id = SalesInvoicePayment.SalesInvoiceId
             where SalesInvoice.Deleted=0
             GROUP BY SalesInvoice.Id, SalesInvoice.CreateDate
            ");
            migrationBuilder.Sql(@"
            Create PROCEDURE [dbo].[Sp_PurchaseInvoiceReport] 
             @InvoiceFromDate Date = NULL, 
             @InvoiceToDate DATE = NULL
             AS 
             BEGIN 
             SELECT  
			 dbo.PurchaseInvoice.Id,
			 dbo.Supplier.[Name] AS SupplierName,
             dbo.Supplier.TaxReferenceNumber AS TaxReferenceNumber,
			 dbo.PurchaseInvoice.InvoiceDate,
			 dbo.PurchaseInvoice.InvoiceNumber,
		     COALESCE( STUFF( (SELECT DISTINCT ','+Name 
             FROM dbo.Product 
		     INNER JOIN dbo.PurchaseInvoiceProduct
			 ON PurchaseInvoiceProduct.ProductId = Product.Id
			 INNER JOIN dbo.PurchaseInvoice s
			 ON s.Id = PurchaseInvoiceProduct.PurchaseInvoiceId
			 WHERE s.Id=dbo.PurchaseInvoice.Id
             FOR XML PATH('')
             ), 1, 1, ''
             ), '') AS Products,

		    COALESCE(STUFF( (SELECT DISTINCT ','+Name 
            FROM dbo.[Service] 
			INNER JOIN dbo.PurchaseInvoiceProduct
			ON PurchaseInvoiceProduct.ServiceId = dbo.[Service].Id
			INNER JOIN dbo.PurchaseInvoice s
			ON s.Id = PurchaseInvoiceProduct.PurchaseInvoiceId
			WHERE s.Id=dbo.PurchaseInvoice.Id
            FOR XML PATH('')
            ), 1, 1, ''
            ), '') AS Services,
			dbo.PurchaseInvoice.Note,
			ISNULL((SELECT TotalProductPrice FROM [dbo].[vTotalProductPurchasePrice] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0) AS TotalProductCost,
			ISNULL((SELECT TotalServicePrice FROM [dbo].[vTotalServicePurchasePrice] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0) AS TotalServiceCost,
			ISNULL((SELECT TotalPrice FROM [dbo].[vTotalPurchasePrice] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0)AS TotalCost,
			ISNULL(dbo.PurchaseInvoice.Tax,0) AS TaxPercentage,
			isNull((SELECT TaxAmount FROM [dbo].[vTotalPurchasePrice] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0)AS TaxAmount,
			ISNULL(dbo.PurchaseInvoice.[Transfer],0) AS TransferPercentage,
			ISNULL((SELECT TransferAmount FROM [dbo].[vTotalPurchasePrice] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0)AS TransferAmount,
			ISNULL(dbo.PurchaseInvoice.AdditionalFees,0) AS AdditionalFees,
			ISNULL((SELECT round(TotalInvoice,2) FROM [dbo].[vTotalPurchasePrice] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0)AS TotalInvoice,
			dbo.PurchaseInvoice.CreateDate,
			CONCAT(dbo.AspNetUsers.FirstName,' ',dbo.AspNetUsers.LastName ) AS CreatedBy,
			COALESCE((SELECT PaymentStatus FROM [dbo].[vPurchasePayment] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),'Un Paid')AS PaymentStatus,
			ISNULL((SELECT round(TotalPaid,2) FROM [dbo].[vPurchasePayment] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0)AS TotalPaid,
			ISNULL((SELECT round(Remaining,2) FROM [dbo].[vPurchasePayment] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),ISNULL((SELECT TotalInvoice FROM [dbo].[vTotalPurchasePrice] WHERE PurchaseInvoiceId = dbo.PurchaseInvoice.Id),0))AS Remaining,
			CASE
               WHEN dbo.PurchaseInvoice.Deleted=1 THEN 'Cancelled'
               ELSE 'Active'
			END AS InvoiceValidaty,
            CASE
               WHEN dbo.PurchaseInvoice.IsTax=1 THEN N'ضريبية'
               ELSE N'غير ضريبية'
			 END AS [Type]

			FROM dbo.PurchaseInvoice
			LEFT JOIN dbo.Supplier ON Supplier.Id = PurchaseInvoice.SupplierId
			LEFT JOIN dbo.AspNetUsers ON AspNetUsers.Id = PurchaseInvoice.CreateUserId

			where 
			    (@InvoiceFromDate is null or (dbo.PurchaseInvoice.InvoiceDate >= @InvoiceFromDate)) 
             and(@InvoiceToDate is null or (dbo.PurchaseInvoice.InvoiceDate <= @InvoiceToDate)) 
            END 

             ");
            migrationBuilder.Sql(@"
               Create PROCEDURE [dbo].[Sp_SalesInvoiceReport] 
             @InvoiceFromDate Date = NULL, 
             @InvoiceToDate DATE = NULL
             AS 
             BEGIN 
             SELECT  
			 dbo.SalesInvoice.Id,
			 dbo.Client.[Name] AS ClientName,
             dbo.Client.TaxReferenceNumber AS TaxReferenceNumber,
			 dbo.SalesInvoice.InvoiceDate,
             dbo.SalesInvoice.InvoiceNumber,
			 dbo.SalesInvoice.BrokerName,
		     COALESCE( STUFF( (SELECT DISTINCT ','+Name 
             FROM dbo.Product 
		     INNER JOIN dbo.SalesInvoiceProduct
			 ON SalesInvoiceProduct.ProductId = Product.Id
			 INNER JOIN dbo.SalesInvoice s
			 ON s.Id = SalesInvoiceProduct.SalesInvoiceId
			 WHERE s.Id=dbo.SalesInvoice.Id
             FOR XML PATH('')
             ), 1, 1, ''
             ), '') AS Products,

		    COALESCE(STUFF( (SELECT DISTINCT ','+Name 
            FROM dbo.[Service] 
			INNER JOIN dbo.SalesInvoiceProduct
			ON SalesInvoiceProduct.ServiceId = dbo.[Service].Id
			INNER JOIN dbo.SalesInvoice s
			ON s.Id = SalesInvoiceProduct.SalesInvoiceId
			WHERE s.Id=dbo.SalesInvoice.Id
            FOR XML PATH('')
            ), 1, 1, ''
            ), '') AS Services,
			dbo.SalesInvoice.Note,
			ISNULL((SELECT TotalProductPrice FROM [dbo].[vTotalProductSalesPrice] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0) AS TotalProductCost,
			ISNULL((SELECT TotalServicePrice FROM [dbo].[vTotalServiceSalesPrice] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0) AS TotalServiceCost,
			ISNULL((SELECT TotalPrice FROM [dbo].[vTotalSalesPrice] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0)AS TotalCost,
			ISNULL(dbo.SalesInvoice.Tax,0) AS TaxPercentage,
			ISNULL((SELECT TaxAmount FROM [dbo].[vTotalSalesPrice] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0)AS TaxAmount,
			ISNULL(dbo.SalesInvoice.[Transfer],0) AS TransferPercentage,
			ISNULL((SELECT TransferAmount FROM [dbo].[vTotalSalesPrice] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0)AS TransferAmount,
			ISNULL(dbo.SalesInvoice.Transportation,0) AS Transportation,
			ISNULL((SELECT round(TotalInvoice,2) FROM [dbo].[vTotalSalesPrice] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0)AS TotalInvoice,
			dbo.SalesInvoice.CreateDate,
			CONCAT(dbo.AspNetUsers.FirstName,' ',dbo.AspNetUsers.LastName ) AS CreatedBy,
			COALESCE((SELECT PaymentStatus FROM [dbo].[vSalesPayment] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),'Un Paid')AS PaymentStatus,
			ISNULL((SELECT round(TotalPaid,2) FROM [dbo].[vSalesPayment] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0)AS TotalPaid,
			ISNULL((SELECT round(Remaining,2) FROM [dbo].[vSalesPayment] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),ISNULL((SELECT TotalInvoice FROM [dbo].[vTotalSalesPrice] WHERE SalesInvoiceId = dbo.SalesInvoice.Id),0))AS Remaining,
			CASE
               WHEN dbo.SalesInvoice.Deleted=1 THEN 'Cancelled'
               ELSE 'Active'
			END AS InvoiceValidaty,
            CASE
               WHEN dbo.SalesInvoice.IsTax=1 THEN N'ضريبية'
               ELSE N'غير ضريبية'
			 END AS [Type]
			FROM dbo.SalesInvoice
			LEFT JOIN dbo.Client ON Client.Id = SalesInvoice.ClientId
			LEFT JOIN dbo.AspNetUsers ON AspNetUsers.Id = SalesInvoice.CreateUserId
			where 
			    (@InvoiceFromDate is null or (dbo.SalesInvoice.InvoiceDate >= @InvoiceFromDate)) 
             and(@InvoiceToDate is null or (dbo.SalesInvoice.InvoiceDate <= @InvoiceToDate)) 
            END 
             ");
            migrationBuilder.Sql(@"
             Create PROCEDURE [dbo].[SP_BankLatestTransactions] 

             @Page INT = 1 , 
             @Size INT = 10 
    
             AS 
             BEGIN 
		

            select COUNT(*) over() as Total_Count, CAST(ROW_NUMBER() OVER (ORDER BY tt.paymentId) AS INT) Id,* from ( select   PurchaseInvoicePayment.Id as PaymentId,
		   'Purchasing' as [Type],
		    PurchaseInvoice.InvoiceNumber as PurchaseInvoiceNumber,
			Null as SalesInvoiceNumber,
		    PurchaseInvoice.Id as InvoiceId,
		   '' as BrokerName,
		   '' as [Description],

		   PurchaseInvoicePayment.CreateDate as [Date],
		   0 as Credit,
		   PurchaseInvoicePayment.Amount as Debit,
		   (
		   IsNull((select BankCredit from Credit),0)-
		   ISNULL((select sum(p.Amount) from PurchaseInvoicePayment p where p.CreateDate <= PurchaseInvoicePayment.CreateDate and (p.PaymentMethod =2 or p.PaymentMethod =3) ),0 ) + 
		   ISNULL((select sum(s.Amount) from SalesInvoicePayment s where s.CreateDate <= PurchaseInvoicePayment.CreateDate and (s.PaymentMethod =2 or s.PaymentMethod =3)),0 ) -
		   ISNULL((select sum(e.Amount) from Expense e where e.CreateDate <= PurchaseInvoicePayment.CreateDate and (e.PaymentMethod =2 or e.PaymentMethod =3)),0 ) 

		   ) as [Current],
		   PurchaseInvoicePayment.CreateUserId,
		   PurchaseInvoicePayment.UpdateUserId,
		   PurchaseInvoicePayment.CreateDate,
		   PurchaseInvoicePayment.UpdateDate,
		   PurchaseInvoicePayment.Deleted
		   
		   from 
		   PurchaseInvoicePayment inner join PurchaseInvoice
		   on PurchaseInvoicePayment.PurchaseInvoiceId=PurchaseInvoice.Id

		   where (PurchaseInvoicePayment.PaymentMethod =2 or PurchaseInvoicePayment.PaymentMethod =3)

		    union all (
			select Expense.Id as PaymentId,
			'Expenses' as [Type],
            '' as PurchaseInvoiceNumber,
			Null as SalesInvoiceNumber,
		    Expense.Id as InvoiceId,
		    '' as BrokerName,
		    Expense.[Description] as [Description],
			Expense.CreateDate as [Date],
			0 as Credit,
			Expense.Amount as Debit,
			(
		   IsNull((select CashCredit from Credit),0)-
		   ISNULL((select sum(p.Amount) from PurchaseInvoicePayment p where p.CreateDate <= Expense.CreateDate and (p.PaymentMethod =2 or p.PaymentMethod=3)),0 ) + 
		   ISNULL((select sum(s.Amount) from SalesInvoicePayment s where s.CreateDate <= Expense.CreateDate and (s.PaymentMethod =2 or s.PaymentMethod=3)),0 ) -
		   ISNULL((select sum(e.Amount) from Expense e where e.CreateDate <= Expense.CreateDate and (e.PaymentMethod=2 or e.PaymentMethod=3)),0 ) 

		   ) as [Current],
		   Expense.CreateUserId,
		   Expense.UpdateUserId,
		   Expense.CreateDate,
		   Expense.UpdateDate,
		   Expense.Deleted
				   

			from 
			Expense
			 where (Expense.PaymentMethod=2 or Expense.PaymentMethod=3) 
			)
		   union all (
		      select  SalesInvoicePayment.Id as PaymentId,
		   'Sales' as [Type],
		 	'' as PurchaseInvoiceNumber,
			SalesInvoice.InvoiceNumber as SalesInvoiceNumber,
		   SalesInvoice.Id as InvoiceId,
		   SalesInvoice.BrokerName as BrokerName,
		   '' as [Description],
		   SalesInvoicePayment.CreateDate as [Date],
		   SalesInvoicePayment.Amount as Credit,
		   0 as Debit,
		   			(
			IsNull((select BankCredit from Credit),0)-
		   ISNULL((select sum(p.Amount) from PurchaseInvoicePayment p where p.CreateDate <= SalesInvoicePayment.CreateDate and (p.PaymentMethod =2 or p.PaymentMethod =3)),0 ) + 
		   ISNULL((select sum(s.Amount) from SalesInvoicePayment s where s.CreateDate <= SalesInvoicePayment.CreateDate and (s.PaymentMethod =2 or s.PaymentMethod =3)),0 )-
		   ISNULL((select sum(e.Amount) from Expense e where e.CreateDate <= SalesInvoicePayment.CreateDate and (e.PaymentMethod =2 or e.PaymentMethod =3)),0 ) 

		   ) as [Current],
		   SalesInvoicePayment.CreateUserId,
		   SalesInvoicePayment.UpdateUserId,
		   SalesInvoicePayment.CreateDate,
		   SalesInvoicePayment.UpdateDate,
		   SalesInvoicePayment.Deleted
		   
		   from 
		   SalesInvoicePayment inner join SalesInvoice
		   on SalesInvoicePayment.SalesInvoiceId=SalesInvoice.Id

		   where (SalesInvoicePayment.PaymentMethod =2 or SalesInvoicePayment.PaymentMethod =3) 
		   )) tt 
		   order by  tt.[Date] desc
		   OFFSET (@Page -1) * @Size ROWS 
           FETCH NEXT @Size ROWS ONLY END
           ");
            migrationBuilder.Sql(@"
             Create  PROCEDURE [dbo].[SP_CashLatestTransactions]

	
             @Page INT = 1 , 
             @Size INT = 10 
    
             AS 
             BEGIN 
            select COUNT(*) over() as Total_Count, CAST(ROW_NUMBER() OVER (ORDER BY tt.paymentId) AS INT) Id,* from ( select   PurchaseInvoicePayment.Id as PaymentId,
		   'Purchasing' as [Type],
		    PurchaseInvoice.InvoiceNumber as PurchaseInvoiceNumber,
			Null as SalesInvoiceNumber,
		    PurchaseInvoice.Id as InvoiceId,
		   '' as BrokerName,
		   '' as [Description],

		   PurchaseInvoicePayment.CreateDate as [Date],
		   0 as Credit,
		   PurchaseInvoicePayment.Amount as Debit,
		   (
		   IsNull((select BankCredit from Credit),0)-
		   ISNULL((select sum(p.Amount) from PurchaseInvoicePayment p where p.CreateDate <= PurchaseInvoicePayment.CreateDate and (p.PaymentMethod =2 or p.PaymentMethod =3) ),0 ) + 
		   ISNULL((select sum(s.Amount) from SalesInvoicePayment s where s.CreateDate <= PurchaseInvoicePayment.CreateDate and (s.PaymentMethod =2 or s.PaymentMethod =3)),0 ) -
		   ISNULL((select sum(e.Amount) from Expense e where e.CreateDate <= PurchaseInvoicePayment.CreateDate and (e.PaymentMethod =2 or e.PaymentMethod =3)),0 ) 

		   ) as [Current],
		   PurchaseInvoicePayment.CreateUserId,
		   PurchaseInvoicePayment.UpdateUserId,
		   PurchaseInvoicePayment.CreateDate,
		   PurchaseInvoicePayment.UpdateDate,
		   PurchaseInvoicePayment.Deleted
		   
		   from 
		   PurchaseInvoicePayment inner join PurchaseInvoice
		   on PurchaseInvoicePayment.PurchaseInvoiceId=PurchaseInvoice.Id

		   where (PurchaseInvoicePayment.PaymentMethod =2 or PurchaseInvoicePayment.PaymentMethod =3)

		    union all (
			select Expense.Id as PaymentId,
			'Expenses' as [Type],
			'' as PurchaseInvoiceNumber,
			Null as SalesInvoiceNumber,
		    Expense.Id as InvoiceId,
		    '' as BrokerName,
		    Expense.[Description] as [Description],
			Expense.CreateDate as [Date],
			0 as Credit,
			Expense.Amount as Debit,
			(
		   IsNull((select CashCredit from Credit),0)-
		   ISNULL((select sum(p.Amount) from PurchaseInvoicePayment p where p.CreateDate <= Expense.CreateDate and (p.PaymentMethod =2 or p.PaymentMethod=3)),0 ) + 
		   ISNULL((select sum(s.Amount) from SalesInvoicePayment s where s.CreateDate <= Expense.CreateDate and (s.PaymentMethod =2 or s.PaymentMethod=3)),0 ) -
		   ISNULL((select sum(e.Amount) from Expense e where e.CreateDate <= Expense.CreateDate and (e.PaymentMethod=2 or e.PaymentMethod=3)),0 ) 

		   ) as [Current],
		   Expense.CreateUserId,
		   Expense.UpdateUserId,
		   Expense.CreateDate,
		   Expense.UpdateDate,
		   Expense.Deleted
				   

			from 
			Expense
			 where (Expense.PaymentMethod=2 or Expense.PaymentMethod=3) 
			)
		   union all (
		      select  SalesInvoicePayment.Id as PaymentId,
		   'Sales' as [Type],
		   '' as PurchaseInvoiceNumber,
			SalesInvoice.InvoiceNumber as SalesInvoiceNumber,
		   SalesInvoice.Id as InvoiceId,
		   SalesInvoice.BrokerName as BrokerName,
		   '' as [Description],
		   SalesInvoicePayment.CreateDate as [Date],
		   SalesInvoicePayment.Amount as Credit,
		   0 as Debit,
		   			(
			IsNull((select BankCredit from Credit),0)-
		   ISNULL((select sum(p.Amount) from PurchaseInvoicePayment p where p.CreateDate <= SalesInvoicePayment.CreateDate and (p.PaymentMethod =2 or p.PaymentMethod =3)),0 ) + 
		   ISNULL((select sum(s.Amount) from SalesInvoicePayment s where s.CreateDate <= SalesInvoicePayment.CreateDate and (s.PaymentMethod =2 or s.PaymentMethod =3)),0 )-
		   ISNULL((select sum(e.Amount) from Expense e where e.CreateDate <= SalesInvoicePayment.CreateDate and (e.PaymentMethod =2 or e.PaymentMethod =3)),0 ) 

		   ) as [Current],
		   SalesInvoicePayment.CreateUserId,
		   SalesInvoicePayment.UpdateUserId,
		   SalesInvoicePayment.CreateDate,
		   SalesInvoicePayment.UpdateDate,
		   SalesInvoicePayment.Deleted
		   
		   from 
		   SalesInvoicePayment inner join SalesInvoice
		   on SalesInvoicePayment.SalesInvoiceId=SalesInvoice.Id

		   where (SalesInvoicePayment.PaymentMethod =2 or SalesInvoicePayment.PaymentMethod =3) 
		   )) tt 
		   order by  tt.[Date] desc
		   OFFSET (@Page -1) * @Size ROWS 
           FETCH NEXT @Size ROWS ONLY END
           

           ");
            migrationBuilder.Sql(@"
             Create PROCEDURE [dbo].[Sp_DashboardAmounts] 
             AS 
             BEGIN 
			 			 
			 			 select 
             0 as Id ,
             0 as CreateUserId ,
             0 as UpdateUserId ,
             getdate() as CreateDate ,
             getdate() as UpdateDate ,
             CAST(0 AS bit) as  Deleted ,
			 (select isNull( sum( [Remaining]),0) from [dbo].[vSalesPayment]) as UnpaidSalesAmount,
			 (select isNull(sum( [Remaining]),0) from [dbo].[vPurchasePayment]) as UnpaidPurchaseAmount,
			 (select isNull(sum( TotalInvoice),0) from [dbo].[vTotalSalesPrice] where  MONTH([vTotalSalesPrice].InvoiceDate)=MONTH(GETDATE()) and Year([vTotalSalesPrice].InvoiceDate)=Year(GETDATE())) as TotalSalesAmount
			 End
           
           ");
            migrationBuilder.Sql(@"

               CREATE FUNCTION [dbo].[MonthlyTotalInvoice] (
               	@month int
               )
               RETURNS decimal AS
               BEGIN
                RETURN  (  select ISNULL( SUM(t.TotalInvoice),0) AS TotalInvoice  from (
                           select   ISNULL(SUM(Price * Quantity),0) +
                           ISNULL((SUM(Price * Quantity)*dbo.SalesInvoice.Tax)/100,0) -
                           ISNULL((SUM(Price * Quantity)*dbo.SalesInvoice.[Transfer])/100,0)+
                           ISNULL(dbo.SalesInvoice.Transportation,0) AS TotalInvoice
                           FROM dbo.SalesInvoice
                           LEFT JOIN dbo.SalesInvoiceProduct
                           ON SalesInvoice.Id = SalesInvoiceProduct.SalesInvoiceId
                           where  SalesInvoiceProduct.Deleted=0 and SalesInvoice.Deleted=0
               			and MONTH( SalesInvoice.CreateDate)=@month and YEAR(SalesInvoice.CreateDate)=YEAR(GETDATE())
               		    GROUP BY SalesInvoice.Id ,Tax,[Transfer],Transportation ) t)
               END;
               GO
           ");
            migrationBuilder.Sql(@"
                  CREATE FUNCTION [dbo].[MonthlyTotalPaid] (
                  	@month int
                  )
                  RETURNS decimal AS
                  BEGIN
                   RETURN  ( select ISNULL( SUM(t.TotalPaid),0) AS TotalPaid  from (
                               SELECT
                               ISNULL( SUM(dbo.SalesInvoicePayment.Amount),0) AS TotalPaid 
                               FROM 
                               dbo.SalesInvoice  LEFT JOIN dbo.SalesInvoicePayment
                               ON SalesInvoice.Id = SalesInvoicePayment.SalesInvoiceId
                               where SalesInvoice.Deleted=0 and MONTH(SalesInvoice.CreateDate)=@month and YEAR(SalesInvoice.CreateDate)=YEAR(GETDATE())
                               GROUP BY SalesInvoice.Id)t)
                  END;
                  GO
");
            migrationBuilder.Sql(@"

Create PROCEDURE [dbo].[Sp_LineChart] 
     
             AS 
             BEGIN 
			 select
             1 Id,
			 1 CreateUserId,
			 1 UpdateUserId,
			 getdate() CreateDate,
			 getdate() UpdateDate,
			 CAST(0 AS bit) as  Deleted ,
			 [dbo].[MonthlyTotalInvoice] (1) as JanInvoice,
			 [dbo].[MonthlyTotalInvoice] (2) as FebInvoice,
			 [dbo].[MonthlyTotalInvoice] (3) as MarInvoice,
			 [dbo].[MonthlyTotalInvoice] (4) as AprInvoice,
			 [dbo].[MonthlyTotalInvoice] (5) as MayInvoice,
			 [dbo].[MonthlyTotalInvoice] (6) as JunInvoice,
			 [dbo].[MonthlyTotalInvoice] (7) as JulInvoice,
			 [dbo].[MonthlyTotalInvoice] (8) as AugInvoice,
			 [dbo].[MonthlyTotalInvoice] (9) as SepInvoice,
			 [dbo].[MonthlyTotalInvoice] (10) as OctInvoice,
			 [dbo].[MonthlyTotalInvoice] (11) as NovInvoice,
			 [dbo].[MonthlyTotalInvoice] (12) as DecInvoice,
			 [dbo].[MonthlyTotalPaid] (1) as JanPaid  ,
			 [dbo].[MonthlyTotalPaid] (2) as FebPaid  ,
			 [dbo].[MonthlyTotalPaid] (3) as MarPaid  ,
			 [dbo].[MonthlyTotalPaid] (4) as AprPaid  ,
			 [dbo].[MonthlyTotalPaid] (5) as MayPaid  ,
			 [dbo].[MonthlyTotalPaid] (6) as JunPaid  ,
			 [dbo].[MonthlyTotalPaid] (7) as JulPaid  ,
			 [dbo].[MonthlyTotalPaid] (8) as AugPaid  ,
			 [dbo].[MonthlyTotalPaid] (9) as SepPaid  ,
			 [dbo].[MonthlyTotalPaid] (10) as OctPaid ,
			 [dbo].[MonthlyTotalPaid] (11) as NovPaid ,
			 [dbo].[MonthlyTotalPaid] (12) as DecPaid

			end
GO
");
            migrationBuilder.Sql(@"

              create PROCEDURE SP_GetSalesInvoiceNumber
              
        AS
        BEGIN
        	-- SET NOCOUNT ON added to prevent extra result sets from
        	-- interfering with SELECT statements.
        	SET NOCOUNT ON;
        
        		select	 top 1 InvoiceNumber +1 as SalesInvoiceNumber ,Id,CreateUserId,UpdateUserId,CreateDate,UpdateDate,Deleted
        			
        			from SalesInvoice order by InvoiceNumber desc 
        END
        GO");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.SP_ProductSearch");
            migrationBuilder.Sql(@"
        drop view vTotalPurchasePrice;
        ");
            migrationBuilder.Sql(@"
        drop view vTotalProductPurchasePrice;
        ");
            migrationBuilder.Sql(@"
        drop view vTotalServicePurchasePrice;
        ");
            migrationBuilder.Sql(@"
        drop view vTotalSalesPrice;
        ");
            migrationBuilder.Sql(@"
        drop view vTotalProductSalesPrice;
        ");
            migrationBuilder.Sql(@"
        drop view vTotalServiceSalesPrice;
        ");
            migrationBuilder.Sql(@"
        drop view vPurchasePayment;
        ");
            migrationBuilder.Sql(@"
        drop view vSalesPayment;
        ");
            migrationBuilder.Sql(@"
        drop PROCEDURE [dbo].[Sp_PurchaseInvoiceReport];
        ");
            migrationBuilder.Sql(@"
        drop PROCEDURE [dbo].[Sp_SalesInvoiceReport];
        ");
            migrationBuilder.Sql(@"
        drop PROCEDURE [dbo].[SP_BankLatestTransactions];
        ");
            migrationBuilder.Sql(@"
        drop PROCEDURE [dbo].[SP_CashLatestTransactions];
        ");
            migrationBuilder.Sql(@"
        drop PROCEDURE [dbo].[Sp_DashboardAmounts];
        ");
            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[MonthlyTotalInvoice];");
            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[MonthlyTotalPaid];");
            migrationBuilder.Sql("DROP PROCEDURE dbo.Sp_LineChart");
            migrationBuilder.Sql("DROP PROCEDURE dbo.SP_GetSalesInvoiceNumber");


        }
    }
}
