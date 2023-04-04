using HatsuneMiku.Data.Entities.Music;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatsuneMiku.Data.EntityConfigurations.Music;

public sealed class ProducerEntityConfiguration : IEntityTypeConfiguration<ProducerEntity>
{
    public void Configure(EntityTypeBuilder<ProducerEntity> builder) =>
        builder
            .Property(p => p.Name)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");
}