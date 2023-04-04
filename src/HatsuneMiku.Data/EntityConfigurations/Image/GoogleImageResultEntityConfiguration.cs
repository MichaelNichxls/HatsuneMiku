using HatsuneMiku.Data.Entities.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Image;

public sealed class GoogleImageResultEntityConfiguration : IEntityTypeConfiguration<GoogleImageResultEntity>
{
    public void Configure(EntityTypeBuilder<GoogleImageResultEntity> builder) =>
        builder
            .Property(r => r.Title)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}