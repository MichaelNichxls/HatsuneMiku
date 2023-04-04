using HatsuneMiku.Data.Entities.Music;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Music;

public sealed class SongEntityConfiguration : IEntityTypeConfiguration<SongEntity>
{
    public void Configure(EntityTypeBuilder<SongEntity> builder) =>
        builder
            .Property(s => s.Title)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}