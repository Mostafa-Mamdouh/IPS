using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {

            builder.ToTable("Country");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Ignore(e => e.CreateUserId);
            builder.Ignore(e => e.UpdateUserId);
            builder.Ignore(e => e.CreateDate);
            builder.Ignore(e => e.UpdateDate);
            builder.Ignore(e => e.Deleted);


            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}