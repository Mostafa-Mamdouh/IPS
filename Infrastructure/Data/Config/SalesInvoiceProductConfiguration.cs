using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SalesInvoiceProductConfiguration : IEntityTypeConfiguration<SalesInvoiceProduct>
    {
        public void Configure(EntityTypeBuilder<SalesInvoiceProduct> builder)
        {

            builder.ToTable("SalesInvoiceProduct");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Ignore(e => e.CreateUserId);
            builder.Ignore(e => e.UpdateUserId);
            builder.Ignore(e => e.CreateDate);
            builder.Ignore(e => e.UpdateDate);


            builder.Property(e => e.Price).HasColumnType("decimal(20, 4)");

            builder.HasOne(d => d.Product)
                .WithMany(p => p.SalesInvoiceProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_SalesInvoiceProduct_Product");

            builder.HasOne(d => d.SalesInvoice)
                .WithMany(p => p.SalesInvoiceProducts)
                .HasForeignKey(d => d.SalesInvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesInvoiceProduct_SalesInvoice");

            builder.HasOne(d => d.Service)
                .WithMany(p => p.SalesInvoiceProducts)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_SalesInvoiceProduct_Service");


        }
    }
}