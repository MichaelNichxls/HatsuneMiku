using GScraper.Brave;
using HatsuneMiku.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HatsuneMiku.Extensions;

public static class BraveScraperExtensions
{
    public static async Task<object> GetEncapsulatedDataAsync(this BraveScraper scraper, Type targetType)
    {
        object instance                         = Activator.CreateInstance(targetType)!;
        IEnumerable<PropertyInfo> queryProps    = targetType.GetProperties().Where(p => p.IsDefined(typeof(ImageQueryAttribute)));

        foreach (PropertyInfo prop in queryProps.Where(p => p.IsDefined(typeof(FromBraveAttribute))))
        {
            var (query, safeSearch)                             = prop.GetCustomAttribute<ImageQueryAttribute>()!;
            var (country, size, type, layout, color, license)   = prop.GetCustomAttribute<FromBraveAttribute>()!;

            IEnumerable<BraveImageResult> results = await scraper.GetImagesAsync(query, safeSearch, country, size, type, layout, color, license).ConfigureAwait(false);

            prop.SetValue(instance, results);
        }

        return instance;
    }

    public static async Task<T> GetEncapsulatedDataAsync<T>(this BraveScraper scraper) =>
        (T)await GetEncapsulatedDataAsync(scraper, typeof(T)).ConfigureAwait(false);
}