using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Lookup & Attachment
            CreateMap<Country, LookupDto>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

            CreateMap<City, LookupDto>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

            CreateMap<Category, LookupDto>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

            CreateMap<ExpenseType, LookupDto>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));


            CreateMap<SystemArchive, AttachmentDto>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.FileName, o => o.MapFrom(s => s.FileName))
               .ForMember(d => d.FilePath, o => o.MapFrom(s => s.FilePath));


            #endregion
            #region Client
            CreateMap<Client, ClientToReturnDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CityId, o => o.MapFrom(s => s.CityId))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City != null ? s.City.Name : null))
                .ForMember(d => d.CountryId, o => o.MapFrom(s => s.City != null ? s.City.CountryId : 0))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.TaxReferenceNumber, o => o.MapFrom(s => s.TaxReferenceNumber))
                .ForMember(d => d.RepresentativeName, o => o.MapFrom(s => s.RepresentativeName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.MobileNumber, o => o.MapFrom(s => s.MobileNumber))
                .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreateUser.FirstName + " " + s.CreateUser.LastName));



            CreateMap<ClientDto, Client>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CityId, o => o.MapFrom(s => s.CityId))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.TaxReferenceNumber, o => o.MapFrom(s => s.TaxReferenceNumber))
                .ForMember(d => d.RepresentativeName, o => o.MapFrom(s => s.RepresentativeName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.MobileNumber, o => o.MapFrom(s => s.MobileNumber));
            #endregion
            #region Supplier
            CreateMap<Supplier, SupplierToReturnDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CityId, o => o.MapFrom(s => s.CityId))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City != null ? s.City.Name : null))
                .ForMember(d => d.CountryId, o => o.MapFrom(s => s.City != null ? s.City.CountryId : 0))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.TaxReferenceNumber, o => o.MapFrom(s => s.TaxReferenceNumber))
                .ForMember(d => d.RepresentativeName, o => o.MapFrom(s => s.RepresentativeName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.MobileNumber, o => o.MapFrom(s => s.MobileNumber))
                .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreateUser.FirstName + " " + s.CreateUser.LastName));



            CreateMap<SupplierDto, Supplier>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CityId, o => o.MapFrom(s => s.CityId))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.TaxReferenceNumber, o => o.MapFrom(s => s.TaxReferenceNumber))
                .ForMember(d => d.RepresentativeName, o => o.MapFrom(s => s.RepresentativeName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.MobileNumber, o => o.MapFrom(s => s.MobileNumber));
            #endregion
            #region Product
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));

            CreateMap<ProductDto, Product>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
            #endregion
            #region Service
            CreateMap<Service, ServiceToReturnDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreateUser.FirstName + " " + s.CreateUser.LastName))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));


            CreateMap<ServiceDto, Service>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
            #endregion
            #region Invoice
            CreateMap<PurchaseInvoicePayment, InvoicePaymentToReturnDto>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.PurchaseInvoiceId, o => o.MapFrom(s => s.PurchaseInvoiceId))
               .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
               .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod))
               .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
               .ForMember(d => d.ChequeNumber, o => o.MapFrom(s => s.ChequeNumber))
               .ForMember(d => d.TransferNumber, o => o.MapFrom(s => s.TransferNumber))
               .ForMember(d => d.ArchiveId, o => o.MapFrom(s => s.ArchiveId))
               .ForMember(d => d.FilePath, o => o.MapFrom(s => s.ArchiveId != null ? s.Archive.FilePath : null))
               .ForMember(d => d.FileName, o => o.MapFrom(s => s.ArchiveId != null ? s.Archive.FileName : null))
               .ForMember(d => d.CreateUserId, o => o.MapFrom(s => s.CreateUserId))
               .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
               .ForMember(d => d.Deleted, o => o.MapFrom(s => s.Deleted))

               ;
            CreateMap<SalesInvoicePayment, InvoicePaymentToReturnDto>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.SalesInvoiceId, o => o.MapFrom(s => s.SalesInvoiceId))
               .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
               .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod))
               .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
               .ForMember(d => d.ChequeNumber, o => o.MapFrom(s => s.ChequeNumber))
               .ForMember(d => d.TransferNumber, o => o.MapFrom(s => s.TransferNumber))
               .ForMember(d => d.ArchiveId, o => o.MapFrom(s => s.ArchiveId))
               .ForMember(d => d.FilePath, o => o.MapFrom(s => s.ArchiveId != null ? s.Archive.FilePath : null))
               .ForMember(d => d.FileName, o => o.MapFrom(s => s.ArchiveId != null ? s.Archive.FileName : null))
               .ForMember(d => d.CreateUserId, o => o.MapFrom(s => s.CreateUserId))
               .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
               .ForMember(d => d.Deleted, o => o.MapFrom(s => s.Deleted))

            ;

            CreateMap<PurchaseInvoiceProduct, InvoiceProductToReturnDto>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.PurchaseInvoiceId, o => o.MapFrom(s => s.PurchaseInvoiceId))
              .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
              .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
              .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.ServiceId))
              .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service != null ? s.Service.Name : null))
              .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
              .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
              .ForMember(d => d.Deleted, o => o.MapFrom(s => s.Deleted))


              ;

            CreateMap<SalesInvoiceProduct, InvoiceProductToReturnDto>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.SalesInvoiceId, o => o.MapFrom(s => s.SalesInvoiceId))
              .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
              .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
              .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.ServiceId))
              .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service != null ? s.Service.Name : null))
              .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
              .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
              .ForMember(d => d.Deleted, o => o.MapFrom(s => s.Deleted))

              ;

            CreateMap<InvoicePaymentDto, PurchaseInvoicePayment>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
              .ForMember(d => d.PurchaseInvoiceId, o => o.MapFrom(s => s.PurchaseInvoiceId))
              .ForMember(d => d.PaymentDate, o => o.MapFrom(s => s.PaymentDate))
              .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod))
              .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
              .ForMember(d => d.ArchiveId, o => o.MapFrom(s => s.ArchiveId))
              .ForMember(d => d.TransferNumber, o => o.MapFrom(s => s.TransferNumber))
              .ForMember(d => d.ChequeNumber, o => o.MapFrom(s => s.ChequeNumber))
             ;

            CreateMap<InvoicePaymentDto, SalesInvoicePayment>()
             .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
             .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
             .ForMember(d => d.SalesInvoiceId, o => o.MapFrom(s => s.SalesInvoiceId))
             .ForMember(d => d.PaymentDate, o => o.MapFrom(s => s.PaymentDate))
             .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod))
             .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
             .ForMember(d => d.ArchiveId, o => o.MapFrom(s => s.ArchiveId))
             .ForMember(d => d.TransferNumber, o => o.MapFrom(s => s.TransferNumber))
             .ForMember(d => d.ChequeNumber, o => o.MapFrom(s => s.ChequeNumber))
            ;





            CreateMap<PurchaseInvoice, InvoiceToReturnDto>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.SupplierId, o => o.MapFrom(s => s.SupplierId))
              .ForMember(d => d.SupplierName, o => o.MapFrom(s => s.Supplier.Name))
              .ForMember(d => d.InvoiceDate, o => o.MapFrom(s => s.InvoiceDate))
              .ForMember(d => d.PurchaseInvoiceNumber, o => o.MapFrom(s => s.InvoiceNumber))
              .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
              .ForMember(d => d.Tax, o => o.MapFrom(s => s.Tax))
              .ForMember(d => d.Transfer, o => o.MapFrom(s => s.Transfer))
              .ForMember(d => d.AdditionalFees, o => o.MapFrom(s => s.AdditionalFees))
              .ForMember(d => d.ArchiveId, o => o.MapFrom(s => s.ArchiveId))
              .ForMember(d => d.FilePath, o => o.MapFrom(s => s.ArchiveId != null ? s.Archive.FilePath : null))
              .ForMember(d => d.FileName, o => o.MapFrom(s => s.ArchiveId != null ? s.Archive.FileName : null))
              .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
              .ForMember(d => d.CreateUserId, o => o.MapFrom(s => s.CreateUserId))
              .ForMember(d => d.Deleted, o => o.MapFrom(s => s.Deleted))
              .ForMember(d => d.InvoicePayments, o => o.MapFrom(s => s.PurchaseInvoicePayments))
              .ForMember(d => d.InvoiceProducts, o => o.MapFrom(s => s.PurchaseInvoiceProducts))
              .ForMember(d => d.TotalInvoice, o => o.MapFrom(s => (s.PurchaseInvoiceProducts.Sum(x => (x.Quantity * x.Price))) +
               (((s.PurchaseInvoiceProducts.Sum(x => (x.Quantity * x.Price))) * s.Tax) / 100) -
               (((s.PurchaseInvoiceProducts.Sum(x => (x.Quantity * x.Price))) * s.Transfer) / 100) +
               (s.AdditionalFees == null ? 0 : s.AdditionalFees)

              ))
              .ForMember(d => d.TotalPaid, o => o.MapFrom(s => s.PurchaseInvoicePayments.Sum(x => (x.Amount))))





              ;
            CreateMap<SalesInvoice, InvoiceToReturnDto>()
             .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
             .ForMember(d => d.ClientId, o => o.MapFrom(s => s.ClientId))
             .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client.Name))
             .ForMember(d => d.TaxReferenceNumber, o => o.MapFrom(s => s.Client.TaxReferenceNumber))
             .ForMember(d => d.InvoiceDate, o => o.MapFrom(s => s.InvoiceDate))
             .ForMember(d => d.SalesInvoiceNumber, o => o.MapFrom(s => s.InvoiceNumber))
             .ForMember(d => d.BrokerName, o => o.MapFrom(s => s.BrokerName))
             .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
             .ForMember(d => d.Tax, o => o.MapFrom(s => s.Tax))
             .ForMember(d => d.Transfer, o => o.MapFrom(s => s.Transfer))
             .ForMember(d => d.Transportaion, o => o.MapFrom(s => s.Transportation))
             .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
             .ForMember(d => d.CreateUserId, o => o.MapFrom(s => s.CreateUserId))
             .ForMember(d => d.Deleted, o => o.MapFrom(s => s.Deleted))
             .ForMember(d => d.InvoicePayments, o => o.MapFrom(s => s.SalesInvoicePayments))
             .ForMember(d => d.InvoiceProducts, o => o.MapFrom(s => s.SalesInvoiceProducts))
             .ForMember(d => d.TotalInvoice, o => o.MapFrom(s => (s.SalesInvoiceProducts.Sum(x => (x.Quantity * x.Price))) +
               (((s.SalesInvoiceProducts.Sum(x => (x.Quantity * x.Price))) * s.Tax) / 100) -
               (((s.SalesInvoiceProducts.Sum(x => (x.Quantity * x.Price))) * s.Transfer) / 100) +
               (s.Transportation == null ? 0 : s.Transportation)
              ))
              .ForMember(d => d.TotalPaid, o => o.MapFrom(s => s.SalesInvoicePayments.Sum(x => (x.Amount))))


             ;

            CreateMap<InvoiceProductDto, PurchaseInvoiceProduct>()
             .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
             .ForMember(d => d.PurchaseInvoiceId, o => o.MapFrom(s => s.PurchaseInvoiceId))
             .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
             .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.ServiceId))
             .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
             .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))

      ;

            CreateMap<InvoiceProductDto, SalesInvoiceProduct>()
             .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
             .ForMember(d => d.SalesInvoiceId, o => o.MapFrom(s => s.SalesInvoiceId))
             .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId))
             .ForMember(d => d.ServiceId, o => o.MapFrom(s => s.ServiceId))
             .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
             .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
      ;
            CreateMap<PurchaseInvoiceDto, PurchaseInvoice>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.SupplierId, o => o.MapFrom(s => s.SupplierId))
              .ForMember(d => d.InvoiceDate, o => o.MapFrom(s => s.InvoiceDate))
              .ForMember(d => d.InvoiceNumber, o => o.MapFrom(s => s.PurchaseInvoiceNumber))
              .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
              .ForMember(d => d.Tax, o => o.MapFrom(s => s.Tax))
              .ForMember(d => d.Transfer, o => o.MapFrom(s => s.Transfer))
              .ForMember(d => d.AdditionalFees, o => o.MapFrom(s => s.AdditionalFees))
              .ForMember(d => d.ArchiveId, o => o.MapFrom(s => s.ArchiveId))
              .ForMember(d => d.PurchaseInvoiceProducts, o => o.MapFrom(s => s.invoiceProducts))

           ;
            CreateMap<SalesInvoiceDto, SalesInvoice>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.ClientId, o => o.MapFrom(s => s.ClientId))
              .ForMember(d => d.InvoiceDate, o => o.MapFrom(s => s.InvoiceDate))
              .ForMember(d => d.InvoiceNumber, o => o.MapFrom(s => s.SalesInvoiceNumber))
              .ForMember(d => d.BrokerName, o => o.MapFrom(s => s.BrokerName))
              .ForMember(d => d.Note, o => o.MapFrom(s => s.Note))
              .ForMember(d => d.Tax, o => o.MapFrom(s => s.Tax))
              .ForMember(d => d.Transfer, o => o.MapFrom(s => s.Transfer))
              .ForMember(d => d.Transportation, o => o.MapFrom(s => s.Transportation))
              .ForMember(d => d.SalesInvoiceProducts, o => o.MapFrom(s => s.invoiceProducts))

           ;
            #endregion
            #region Users
            CreateMap<Claim, ClaimsDto>()
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Value))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => Enumeration.GetEnumDescription((Permissions)Enum.Parse(typeof(Permissions), s.Value, true))));


            CreateMap<ClaimsDto, Claim>()
               .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
               .ForMember(d => d.Value, o => o.MapFrom(s => s.Value));

            CreateMap<AppUser, UserDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.CreateDate))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
                .ForMember(d => d.EmailConfirmed, o => o.MapFrom(s => s.EmailConfirmed))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));


            CreateMap<UserDto, AppUser>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive));
            #endregion

            #region Expense
            CreateMap<Expense, ExpenseToReturnDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.ExpenseTypeId, o => o.MapFrom(s => s.ExpenseTypeId))
                .ForMember(d => d.ExpenseType, o => o.MapFrom(s => s.ExpenseType.Name))
                .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod))
                .ForMember(d => d.ChequeNumber, o => o.MapFrom(s => s.ChequeNumber))
                .ForMember(d => d.TransferNumber, o => o.MapFrom(s => s.TransferNumber))
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreateUser.FirstName+" "+s.CreateUser.LastName))
                .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
                .ForMember(d => d.TransactionDate, o => o.MapFrom(s => s.TransactionDate));

            CreateMap<ExpenseDto, Expense>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.ExpenseTypeId, o => o.MapFrom(s => s.ExpenseTypeId))
                .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod))
                .ForMember(d => d.ChequeNumber, o => o.MapFrom(s => s.ChequeNumber))
                .ForMember(d => d.TransferNumber, o => o.MapFrom(s => s.TransferNumber))
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.TransactionDate, o => o.MapFrom(s => s.TransactionDate));
            #endregion
            #region Credit
            CreateMap<Credit, CreditDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.BankCredit, o => o.MapFrom(s => s.BankCredit))
                .ForMember(d => d.CashCredit, o => o.MapFrom(s => s.CashCredit))
                .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreateUser.FirstName + " " + s.CreateUser.LastName))
                .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate));

            CreateMap<CreditDto, Credit>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.BankCredit, o => o.MapFrom(s => s.BankCredit))
                .ForMember(d => d.CashCredit, o => o.MapFrom(s => s.CashCredit));
            #endregion

            #region Dashboard
            CreateMap<Sp_DashboardAmounts, DashBoardAmountsDto>()
              .ForMember(d => d.UnpaidPurchaseAmount, o => o.MapFrom(s => s.UnpaidPurchaseAmount))
              .ForMember(d => d.UnpaidSalesAmount, o => o.MapFrom(s => s.UnpaidSalesAmount))
              .ForMember(d => d.TotalSalesAmount, o => o.MapFrom(s => s.TotalSalesAmount));

            CreateMap<Sp_LineChart, LineChartDto>()
               .ForMember(d => d.JanInvoice, o => o.MapFrom(s => s.JanInvoice))
               .ForMember(d => d.FebInvoice, o => o.MapFrom(s => s.FebInvoice))
               .ForMember(d => d.MarInvoice, o => o.MapFrom(s => s.MarInvoice))
               .ForMember(d => d.AprInvoice, o => o.MapFrom(s => s.AprInvoice))
               .ForMember(d => d.MayInvoice, o => o.MapFrom(s => s.MayInvoice))
               .ForMember(d => d.JunInvoice, o => o.MapFrom(s => s.JunInvoice))
               .ForMember(d => d.JulInvoice, o => o.MapFrom(s => s.JulInvoice))
               .ForMember(d => d.AugInvoice, o => o.MapFrom(s => s.AugInvoice))
               .ForMember(d => d.SepInvoice, o => o.MapFrom(s => s.SepInvoice))
               .ForMember(d => d.OctInvoice, o => o.MapFrom(s => s.OctInvoice))
               .ForMember(d => d.NovInvoice, o => o.MapFrom(s => s.NovInvoice))
               .ForMember(d => d.DecInvoice, o => o.MapFrom(s => s.DecInvoice))
               .ForMember(d => d.JanPaid, o => o.MapFrom(s => s.JanPaid))
               .ForMember(d => d.FebPaid, o => o.MapFrom(s => s.FebPaid))
               .ForMember(d => d.MarPaid, o => o.MapFrom(s => s.MarPaid))
               .ForMember(d => d.AprPaid, o => o.MapFrom(s => s.AprPaid))
               .ForMember(d => d.MayPaid, o => o.MapFrom(s => s.MayPaid))
               .ForMember(d => d.JunPaid, o => o.MapFrom(s => s.JunPaid))
               .ForMember(d => d.JulPaid, o => o.MapFrom(s => s.JulPaid))
               .ForMember(d => d.AugPaid, o => o.MapFrom(s => s.AugPaid))
               .ForMember(d => d.SepPaid, o => o.MapFrom(s => s.SepPaid))
               .ForMember(d => d.OctPaid, o => o.MapFrom(s => s.OctPaid))
               .ForMember(d => d.NovPaid, o => o.MapFrom(s => s.NovPaid))
               .ForMember(d => d.DecPaid, o => o.MapFrom(s => s.DecPaid));






            #endregion
        }
    }
}