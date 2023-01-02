using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SystemArchiveConfiguration : IEntityTypeConfiguration<SystemArchive>
    {
        public void Configure(EntityTypeBuilder<SystemArchive> builder)
        {

            builder.ToTable("SystemArchive");


            builder.Ignore(e => e.CreateUserId);
            builder.Ignore(e => e.UpdateUserId);
            builder.Ignore(e => e.CreateDate);
            builder.Ignore(e => e.UpdateDate);
            builder.Ignore(e => e.Deleted);


            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(e => e.FilePath)
                .IsRequired()
                .HasMaxLength(500);


        }
    }
}