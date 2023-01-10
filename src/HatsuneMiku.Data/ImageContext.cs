using HatsuneMiku.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HatsuneMiku.Data;

public class ImageContext : DbContext
{
    public DbSet<ImageEntity> Images { get; set; }

    public ImageContext(DbContextOptions<ImageContext> options)
        : base(options)
    {
    }
}