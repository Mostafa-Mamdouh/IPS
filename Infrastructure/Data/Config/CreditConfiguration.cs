using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CreditConfiguration : IEntityTypeConfiguration<Credit>
    {
        public void Configure(EntityTypeBuilder<Credit> builder)
        {

            builder.ToTable("Credit");
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.BankCredit)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(e => e.CashCredit)
            .IsRequired()
            .HasDefaultValue(0);

            builder.HasOne(d => d.CreateUser)
             .WithMany()
             .HasForeignKey(d => d.CreateUserId)
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("FK_Credit_AspNetUsers");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Credit_AspNetUsers1");
        }
    }
}