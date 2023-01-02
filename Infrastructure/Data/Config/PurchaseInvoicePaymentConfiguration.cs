using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PurchaseInvoicePaymentConfiguration : IEntityTypeConfiguration<PurchaseInvoicePayment>
    {
        public void Configure(EntityTypeBuilder<PurchaseInvoicePayment> builder)
        {

            builder.ToTable("PurchaseInvoicePayment");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Amount).HasColumnType("decimal(20, 4)");

            builder.Property(e => e.ChequeNumber).HasMaxLength(200);

            builder.Property(e => e.CreateDate).HasColumnType("datetime");

            builder.Property(e => e.PaymentDate).HasColumnType("datetime");

            builder.Property(e => e.TransferNumber).HasMaxLength(200);

            builder.Property(e => e.UpdateDate).HasColumnType("datetime");

            builder.HasOne(d => d.Archive)
                .WithMany(p => p.PurchaseInvoicePayments)
                .HasForeignKey(d => d.ArchiveId)
                .HasConstraintName("FK_PurchaseInvoicePayment_SystemArchive");

            builder.HasOne(d => d.CreateUser)
                .WithMany()
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseInvoicePayment_AspNetUsers");

            builder.HasOne(d => d.PurchaseInvoice)
                .WithMany(p => p.PurchaseInvoicePayments)
                .HasForeignKey(d => d.PurchaseInvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseInvoicePayment_PurchaseInvoice");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseInvoicePayment_AspNetUsers1");


        }
    }
}