using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {

            builder.ToTable("Expense");
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.ExpenseTypeId);
            builder.Property(e => e.Description).HasMaxLength(1000);
            builder.Property(e => e.TransactionDate).HasColumnType("datetime");
            builder.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.TransferNumber).HasMaxLength(200);
            builder.Property(e => e.ChequeNumber).HasMaxLength(200);
            builder.Property(e => e.CreateDate).HasColumnType("datetime");
            builder.Property(e => e.UpdateDate).HasColumnType("datetime");

            builder.HasOne(d => d.ExpenseType)
                 .WithMany(p => p.Expenses)
                 .HasForeignKey(d => d.ExpenseTypeId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_Expense_Type");

            builder.HasOne(d => d.CreateUser)
                .WithMany()
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_AspNetUsers");

            builder.HasOne(d => d.UpdateUser)
                .WithMany()
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_AspNetUsers1");


        }
    }
}