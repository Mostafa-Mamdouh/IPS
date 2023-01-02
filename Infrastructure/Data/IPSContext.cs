using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure.Data
{
    public class IPSContext : DbContext
    {
        public IPSContext( DbContextOptions<IPSContext> options) : base(options)
        {
        }
        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public virtual DbSet<PurchaseInvoiceProduct> PurchaseInvoiceProducts { get; set; }
        public virtual DbSet<PurchaseInvoicePayment> PurchaseInvoicePayments { get; set; }
        public virtual DbSet<SalesInvoice> SalesInvoices { get; set; }
        public virtual DbSet<SalesInvoiceProduct> SalesInvoiceProducts { get; set; }
        public virtual DbSet<SalesInvoicePayment> SalesInvoicePayments { get; set; }
        public virtual DbSet<Sp_ProductSearch> Sp_ProductSearches { get; set; }
        public virtual DbSet<Sp_PurchaseInvoiceReport> Sp_PurchaseInvoiceReports { get; set; }
        public virtual DbSet<Sp_SalesInvoiceReport> Sp_SalesInvoiceReports { get; set; }
        public virtual DbSet<SP_BankLatestTransactions> SP_BankLatestTransactions { get; set; }
        public virtual DbSet<SP_CashLatestTransactions> SP_CashLatestTransactions { get; set; }
        public virtual DbSet<Sp_DashboardAmounts> Sp_DashboardAmounts { get; set; }
        public virtual DbSet<Sp_LineChart> Sp_LineCharts { get; set; }
        public virtual DbSet<SP_GetSalesInvoiceNumber> SP_GetSalesInvoiceNumber { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Sp_ProductSearch>().ToTable(nameof(Sp_ProductSearch), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Sp_PurchaseInvoiceReport>().ToTable(nameof(Sp_PurchaseInvoiceReport), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Sp_SalesInvoiceReport>().ToTable(nameof(Sp_SalesInvoiceReport), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SP_BankLatestTransactions>().ToTable(nameof(SP_BankLatestTransactions), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SP_CashLatestTransactions>().ToTable(nameof(SP_CashLatestTransactions), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Sp_DashboardAmounts>().ToTable(nameof(Sp_DashboardAmounts), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Sp_LineChart>().ToTable(nameof(Sp_LineChart), t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SP_GetSalesInvoiceNumber>().ToTable(nameof(SP_GetSalesInvoiceNumber), t => t.ExcludeFromMigrations());




            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
