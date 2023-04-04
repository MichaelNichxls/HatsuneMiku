using HatsuneMiku.Data.Entities.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Image;

public sealed class DuckDuckGoImageResultEntityConfiguration : IEntityTypeConfiguration<DuckDuckGoImageResultEntity>
{
    public void Configure(EntityTypeBuilder<DuckDuckGoImageResultEntity> builder) =>
        builder
            .Property(r => r.Title)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}