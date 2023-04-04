using HatsuneMiku.Data.Entities.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Image;

public sealed class BraveImageResultEntityConfiguration : IEntityTypeConfiguration<BraveImageResultEntity>
{
    public void Configure(EntityTypeBuilder<BraveImageResultEntity> builder) =>
        builder
            .Property(r => r.Title)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}