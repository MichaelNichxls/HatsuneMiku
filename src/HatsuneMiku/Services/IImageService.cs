using GScraper;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HatsuneMiku.Services;

public interface IImageService
{
    // Rename
    Task AddAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate);
    Task<IEnumerable<ImageResultEntity>> GetAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate);
}