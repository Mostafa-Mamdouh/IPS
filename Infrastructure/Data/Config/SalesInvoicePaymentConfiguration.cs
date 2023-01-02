using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SalesInvoicePaymentConfiguration : IEntityTypeConfiguration<SalesInvoicePayment>
    {
        public void Configure(EntityTypeBuilder<SalesInvoicePayment> builder)
        {

            builder.ToTable("SalesInvoicePayment");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Amount).HasColumnType("decimal(20, 4)");

            builder.Property(e => e.ChequeNumber).HasMaxLength(200);

            builder.Property(e => e.CreateDate).HasColumnType("datetime");

            builder.Property(e => e.PaymentDate).HasColumnType("datetime");

            builder.Property(e => e.TransferNumber).HasMaxLength(200);

            builder.Property(e => e.UpdateDate).HasColumnType("datetime");

            builder.HasOne(d => d.Archive)
                .WithMany(p => p.SalesInvoicePayments)
                .HasForeignKey(d => d.ArchiveId)
                .HasConstraintName("FK_SalesInvoicePayment_SystemArchive");

            builder.HasOne(d => d.CreateUser)
                .WithMany()
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesInvoicePayment_AspNetUsers");

            builder.HasOne(d => d.SalesInvoice)
                .WithMany(p => p.SalesInvoicePayments)
                .HasForeignKey(d => d.SalesInvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesInvoicePayment_SalesInvoice");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesInvoicePayment_AspNetUsers1");


        }
    }
}