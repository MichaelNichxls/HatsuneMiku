using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Data.Entities.Music;
using Microsoft.EntityFrameworkCore;

using ColorConverter = HatsuneMiku.Data.Converters.ColorConverter;

namespace HatsuneMiku.Data;

public sealed class MediaContext : DbContext
{
    public DbSet<GoogleImageQueryEntity> GoogleImageQueries { get; set; }
    public DbSet<GoogleImageResultEntity> GoogleImageResults { get; set; }
    public DbSet<GoogleImageQueryResultEntity> GoogleImageQueryResults { get; set; }

    public DbSet<DuckDuckGoImageQueryEntity> DuckDuckGoImageQueries { get; set; }
    public DbSet<DuckDuckGoImageResultEntity> DuckDuckGoImageResults { get; set; }
    public DbSet<DuckDuckGoImageQueryResultEntity> DuckDuckGoImageQueryResults { get; set; }

    public DbSet<BraveImageQueryEntity> BraveImageQueries { get; set; }
    public DbSet<BraveImageResultEntity> BraveImageResults { get; set; }
    public DbSet<BraveImageQueryResultEntity> BraveImageQueryResults { get; set; }

    public DbSet<SongEntity> Songs { get; set; }
    public DbSet<ProducerEntity> Producers { get; set; }
    public DbSet<SongProducerEntity> SongProducers { get; set; }

    public MediaContext(DbContextOptions<MediaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(MediaContext).Assembly);

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder
            .Properties<Color>()
            .HaveConversion<ColorConverter>();
}