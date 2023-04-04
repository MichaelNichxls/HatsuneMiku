using HatsuneMiku.Data.Entities.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Image;

public sealed class DuckDuckGoImageQueryEntityConfiguration : IEntityTypeConfiguration<DuckDuckGoImageQueryEntity>
{
    public void Configure(EntityTypeBuilder<DuckDuckGoImageQueryEntity> builder) =>
        builder
            .Property(q => q.Query)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}