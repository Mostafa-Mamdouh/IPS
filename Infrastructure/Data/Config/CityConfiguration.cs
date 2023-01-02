using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {

            builder.ToTable("City");

            builder.HasIndex(e => e.CountryId, "IX_City_CountryId");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Ignore(e => e.CreateUserId);
            builder.Ignore(e => e.UpdateUserId);
            builder.Ignore(e => e.CreateDate);
            builder.Ignore(e => e.UpdateDate);
            builder.Ignore(e => e.Deleted);


            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(d => d.Country)
                .WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");
        }
    }
}