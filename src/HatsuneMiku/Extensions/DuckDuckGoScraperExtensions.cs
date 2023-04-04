using GScraper.DuckDuckGo;
using HatsuneMiku.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HatsuneMiku.Extensions;

public static class DuckDuckGoScraperExtensions
{
    public static async Task<object> GetEncapsulatedDataAsync(this DuckDuckGoScraper scraper, Type targetType)
    {
        object instance                         = Activator.CreateInstance(targetType)!;
        IEnumerable<PropertyInfo> queryProps    = targetType.GetProperties().Where(p => p.IsDefined(typeof(ImageQueryAttribute)));

        foreach (PropertyInfo prop in queryProps.Where(p => p.IsDefined(typeof(FromDuckDuckGoAttribute))))
        {
            var (query, safeSearch)                                 = prop.GetCustomAttribute<ImageQueryAttribute>()!;
            var (time, size, color, type, layout, license, region)  = prop.GetCustomAttribute<FromDuckDuckGoAttribute>()!;

            IEnumerable<DuckDuckGoImageResult> results = await scraper.GetImagesAsync(query, safeSearch, time, size, color, type, layout, license, region).ConfigureAwait(false);

            prop.SetValue(instance, results);
        }

        return instance;
    }

    public static async Task<T> GetEncapsulatedDataAsync<T>(this DuckDuckGoScraper scraper) =>
        (T)await GetEncapsulatedDataAsync(scraper, typeof(T)).ConfigureAwait(false);
}