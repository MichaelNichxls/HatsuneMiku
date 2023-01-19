using GScraper;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HatsuneMiku.Services;

public interface IImageService
{
    // Rename
    //Task AddAsync(string query, ImageType imageType, SafeSearchLevel safeSearchLevel);
    Task<IEnumerable<ImageResultEntity>> GetOrAddImageResultsAsync(string query, ImageType imageType, SafeSearchLevel safeSearchLevel);
}