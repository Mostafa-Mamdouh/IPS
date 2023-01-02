using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {

            builder.ToTable("Client");

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

            builder.HasOne(d => d.City)
                .WithMany(c=>c.Clients)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_City");

            builder.HasOne(d => d.CreateUser)
                .WithMany()
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_AspNetUsers");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_AspNetUsers1");


        }
    }
}