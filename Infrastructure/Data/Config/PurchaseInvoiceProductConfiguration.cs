using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PurchaseInvoiceProductConfiguration : IEntityTypeConfiguration<PurchaseInvoiceProduct>
    {
        public void Configure(EntityTypeBuilder<PurchaseInvoiceProduct> builder)
        {

            builder.ToTable("PurchaseInvoiceProduct");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();


            builder.Ignore(e => e.CreateUserId);
            builder.Ignore(e => e.UpdateUserId);
            builder.Ignore(e => e.CreateDate);
            builder.Ignore(e => e.UpdateDate);

            builder.Property(e => e.Price).HasColumnType("decimal(20, 4)");

            builder.HasOne(d => d.Product)
                .WithMany(p => p.PurchaseInvoiceProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_PurchaseInvoiceProduct_Product");

            builder.HasOne(d => d.PurchaseInvoice)
                .WithMany(p => p.PurchaseInvoiceProducts)
                .HasForeignKey(d => d.PurchaseInvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseInvoiceProduct_PurchaseInvoice");

            builder.HasOne(d => d.Service)
                .WithMany(p => p.PurchaseInvoiceProducts)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_PurchaseInvoiceProduct_Service");


        }
    }
}