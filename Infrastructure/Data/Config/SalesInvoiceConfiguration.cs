using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SalesInvoiceConfiguration : IEntityTypeConfiguration<SalesInvoice>
    {
        public void Configure(EntityTypeBuilder<SalesInvoice> builder)
        {

            builder.ToTable("SalesInvoice");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.BrokerName).HasMaxLength(50);

            builder.Property(e => e.InvoiceNumber).HasColumnType("int");

            builder.Property(e => e.CreateDate).HasColumnType("datetime");

            builder.Property(e => e.InvoiceDate).HasColumnType("datetime");

            builder.Property(e => e.Note).HasMaxLength(1000);

            builder.Property(e => e.Tax).HasColumnType("decimal(4, 2)");

            builder.Property(e => e.Transfer).HasColumnType("decimal(4, 2)");

            builder.Property(e => e.Transportation).HasColumnType("decimal(10, 2)");

            builder.Property(e => e.UpdateDate).HasColumnType("datetime");
            builder.Property(e => e.IsTax).HasColumnType("bit");


            builder.HasOne(d => d.Client)
                .WithMany(p => p.SalesInvoices)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesInvoice_Client");

            builder.HasOne(d => d.CreateUser)
                .WithMany()
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesInvoice_AspNetUsers");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesInvoice_AspNetUsers1");


        }
    }
}