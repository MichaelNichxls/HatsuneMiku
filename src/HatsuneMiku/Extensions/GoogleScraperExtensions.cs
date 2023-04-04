using GScraper;
using GScraper.Google;
using HatsuneMiku.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HatsuneMiku.Extensions;

public static class GoogleScraperExtensions
{
    public static async Task<object> GetEncapsulatedDataAsync(this GoogleScraper scraper, Type targetType)
    {
        object instance                         = Activator.CreateInstance(targetType)!;
        IEnumerable<PropertyInfo> queryProps    = targetType.GetProperties().Where(p => p.IsDefined(typeof(ImageQueryAttribute)));

        foreach (PropertyInfo prop in queryProps.Where(p => p.IsDefined(typeof(FromGoogleAttribute))))
        {
            var (query, safeSearch)                             = prop.GetCustomAttribute<ImageQueryAttribute>()!;
            var (size, color, type, time, license, language)    = prop.GetCustomAttribute<FromGoogleAttribute>()!;
            
            IEnumerable<GoogleImageResult> results = await scraper.GetImagesAsync(query, safeSearch, size, color, type, time, license, language).ConfigureAwait(false);
            
            prop.SetValue(instance, results);
        }

        return instance;
    }

    public static async Task<T> GetEncapsulatedDataAsync<T>(this GoogleScraper scraper) =>
        (T)await GetEncapsulatedDataAsync(scraper, typeof(T)).ConfigureAwait(false);
}