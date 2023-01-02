using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {

            builder.ToTable("Supplier");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Address).HasMaxLength(200);

            builder.Property(e => e.CreateDate).HasColumnType("datetime");

            builder.Property(e => e.Email)
                .HasMaxLength(50);

            builder.Property(e => e.MobileNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.RepresentativeName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.TaxReferenceNumber).HasMaxLength(200);

            builder.Property(e => e.UpdateDate).HasColumnType("datetime");

            builder.HasOne(d => d.City)
                .WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_City");

            builder.HasOne(d => d.CreateUser)
                .WithMany()
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_AspNetUsers");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_AspNetUsers1");


        }
    }
}