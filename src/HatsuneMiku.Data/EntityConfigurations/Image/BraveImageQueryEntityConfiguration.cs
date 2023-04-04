using HatsuneMiku.Data.Entities.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Image;

public sealed class BraveImageQueryEntityConfiguration : IEntityTypeConfiguration<BraveImageQueryEntity>
{
    public void Configure(EntityTypeBuilder<BraveImageQueryEntity> builder) =>
        builder
            .Property(q => q.Query)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}