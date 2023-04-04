using HatsuneMiku.Data.Entities.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Image;

// Make IImageQuery an abstract class or something
public sealed class GoogleImageQueryEntityConfiguration : IEntityTypeConfiguration<GoogleImageQueryEntity>
{
    public void Configure(EntityTypeBuilder<GoogleImageQueryEntity> builder) =>
        builder
            .Property(q => q.Query)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}
