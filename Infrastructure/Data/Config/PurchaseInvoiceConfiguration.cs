using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PurchaseInvoiceConfiguration : IEntityTypeConfiguration<PurchaseInvoice>
    {
        public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
        {

            builder.ToTable("PurchaseInvoice");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.AdditionalFees).HasColumnType("decimal(10, 2)");

            builder.Property(e => e.ArchiveId).HasColumnName("ArchiveID");

            builder.Property(e => e.CreateDate).HasColumnType("datetime");

            builder.Property(e => e.InvoiceDate).HasColumnType("datetime");

            builder.Property(e => e.InvoiceNumber).HasColumnType("nvarchar(150)");

            builder.Property(e => e.Note).HasMaxLength(1000);

            builder.Property(e => e.Tax).HasColumnType("decimal(4, 2)");

            builder.Property(e => e.Transfer).HasColumnType("decimal(4, 2)");

            builder.Property(e => e.UpdateDate).HasColumnType("datetime");

            builder.Property(e => e.IsTax).HasColumnType("bit");


            builder.HasOne(d => d.Archive)
                .WithMany(p => p.PurchaseInvoices)
                .HasForeignKey(d => d.ArchiveId)
                .HasConstraintName("FK_PurchaseInvoice_SystemArchive");


            builder.HasOne(d => d.Supplier)
                .WithMany(p => p.PurchaseInvoices)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseInvoice_Supplier");

            builder.HasOne(d => d.CreateUser)
                .WithMany()
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseInvoice_AspNetUsers");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseInvoice_AspNetUsers1");


        }
    }
}