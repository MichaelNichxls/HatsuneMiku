using GScraper;
using GScraper.Brave;
using GScraper.DuckDuckGo;
using GScraper.Google;
using HatsuneMiku.Data;
using HatsuneMiku.Data.Entities.Image;
using HatsuneMiku.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsuneMiku.Services;

public class ImageService : IImageService, IDisposable
{
    private bool _disposed;

    private readonly ImageContext _context;

    public GoogleScraper GoogleScraper { get; } = new();
    public DuckDuckGoScraper DuckDuckGoScraper { get; } = new();
    public BraveScraper BraveScraper { get; } = new();

    public ImageService(ImageContext context) => _context = context;

    // Reorder methods
    // Make helper?
    // Move scraper in separate services?
    private async Task<IEnumerable<IImageResult>> GetImagesAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate)
    {
        IEnumerable<IImageResult> googleImages = await GoogleScraper
            .GetImagesAsync(
                query,
                type: imageType switch
                {
                    ImageType.Any       => GoogleImageType.Any,
                    ImageType.Photo     => GoogleImageType.Photo,
                    ImageType.Animated  => GoogleImageType.Animated,
                    _                   => GoogleImageType.Any
                },
                safeSearch: safeSearchLevel)
            .ConfigureAwait(false);

        IEnumerable<IImageResult> duckDuckGoImages = await DuckDuckGoScraper
            .GetImagesAsync(
                query,
                type: imageType switch
                {
                    ImageType.Any       => DuckDuckGoImageType.All,
                    ImageType.Photo     => DuckDuckGoImageType.Photo,
                    ImageType.Animated  => DuckDuckGoImageType.Gif,
                    _                   => DuckDuckGoImageType.All
                },
                safeSearch: safeSearchLevel)
            .ConfigureAwait(false);

        IEnumerable<IImageResult> braveImages = await BraveScraper
            .GetImagesAsync(
                query,
                type: imageType switch
                {
                    ImageType.Any       => BraveImageType.All,
                    ImageType.Photo     => BraveImageType.Photo,
                    ImageType.Animated  => BraveImageType.AnimatedGifHttps,
                    _                   => BraveImageType.All
                },
                safeSearch: safeSearchLevel)
            .ConfigureAwait(false);

        return googleImages.Concat(duckDuckGoImages).Concat(braveImages);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            GoogleScraper?.Dispose();
            DuckDuckGoScraper?.Dispose();
            BraveScraper?.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Needed?
    private async Task<ImageQueryEntity?> GetImageQueryAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate) =>
        await _context.ImageQueries
            .Where(
                // Define collation
                // Override equality method
                q => q.Query.ToLower() == query.ToLower()
                    && q.ImageType == imageType
                    && q.SafeSearchLevel == safeSearchLevel)
            .Include(q => q.ImageResults)
            .Include(q => q.ImageResults)
                .ThenInclude(r => r.ImageResult)
            //.SingleAsync()
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

    // Needed?
    private async Task<IEnumerable<ImageResultEntity>> GetImageResultsAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate) =>
        await _context.ImageResults
            .Where(
                r => r.ImageQuery.ImageQuery.Query.ToLower() == query.ToLower()
                    && r.ImageQuery.ImageQuery.ImageType == imageType
                    && r.ImageQuery.ImageQuery.SafeSearchLevel == safeSearchLevel)
            .Include(r => r.ImageQuery)
            .Include(r => r.ImageQuery)
                .ThenInclude(q => q.ImageQuery)
            .ToListAsync()
            .ConfigureAwait(false);

    // Rename
    private async Task AddImageQueryAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate)
    {
        // Do this better
        ImageQueryEntity imageQuery = new()
        {
            Query = query,
            ImageType = imageType,
            SafeSearchLevel = safeSearchLevel
        };

        await _context.AddAsync(imageQuery).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    // Rename
    private async Task AddImageResultsAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate)
    {
        List<IImageResult> images = (await GetImagesAsync(query, imageType, safeSearchLevel).ConfigureAwait(false)).ToList()!;
        List<ImageResultEntity> imageResults = new();

        ImageQueryEntity? imageQuery = await GetImageQueryAsync(query, imageType, safeSearchLevel).ConfigureAwait(false);

        if (imageQuery is null)
            return;

        for (int i = 0; i < images.Count; i++)
        {
            // Do this better
            ImageResultEntity imageResult = new()
            {
                Url = images[i].Url,
                Title = images[i].Title,
                Width = images[i].Width,
                Height = images[i].Height
            };

            imageResult.ImageQuery = new ImageQueryResultEntity
            {
                ImageQueryId = imageQuery.Id,
                ImageResultId = imageResult.Id
            };

            imageResults.Add(imageResult);
        }

        // Move down maybe
        await _context.AddRangeAsync(imageResults).ConfigureAwait(false);

        for (int i = 0; i < images.Count; i++)
        {
            imageQuery.ImageResults.Add(new ImageQueryResultEntity
            {
                ImageQueryId = imageQuery.Id,
                ImageResultId = imageResults[i].Id
            });
        }

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    // Rename
    // Do this better
    // Make record or struct if possible
    public async Task<IEnumerable<ImageResultEntity>> GetOrAddImageResultsAsync(string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate)
    {
        ImageQueryEntity? imageQuery = await GetImageQueryAsync(query, imageType, safeSearchLevel).ConfigureAwait(false);

        if (imageQuery is not null)
            return imageQuery.ImageResults.Select(r => r.ImageResult);
        
        await AddImageQueryAsync(query, imageType, safeSearchLevel).ConfigureAwait(false);
        await AddImageResultsAsync(query, imageType, safeSearchLevel).ConfigureAwait(false);

        return (await GetImageQueryAsync(query, imageType, safeSearchLevel).ConfigureAwait(false))!.ImageResults.Select(r => r.ImageResult);
    }

    // Make a method (that'll be more efficient) for getting a random image result
}