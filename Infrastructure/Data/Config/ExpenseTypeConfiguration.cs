using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
    {
        public void Configure(EntityTypeBuilder<ExpenseType> builder)
        {

            builder.ToTable("ExpenseType");

            builder.Property(e => e.Id).ValueGeneratedOnAdd(); ;

            builder.Ignore(e => e.CreateUserId);
            builder.Ignore(e => e.UpdateUserId);
            builder.Ignore(e => e.CreateDate);
            builder.Ignore(e => e.UpdateDate);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}