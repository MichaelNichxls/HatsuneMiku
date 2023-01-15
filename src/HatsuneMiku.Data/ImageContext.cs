using HatsuneMiku.Data.Entities.Image;
using Microsoft.EntityFrameworkCore;

namespace HatsuneMiku.Data;

public class ImageContext : DbContext
{
    public DbSet<ImageQueryEntity> ImageQueries { get; set; }
    public DbSet<ImageResultEntity> ImageResults { get; set; }
    public DbSet<ImageQueryResultEntity> ImageQueryResults { get; set; }

    public ImageContext(DbContextOptions<ImageContext> options)
        : base(options)
    {
    }
}