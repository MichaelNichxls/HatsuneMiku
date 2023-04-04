using GScraper;
using GScraper.Brave;
using GScraper.DuckDuckGo;
using GScraper.Google;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HatsuneMiku.Shared.Models;

public sealed class ImagesResults
{
    // SkipNotFoundAttribute

    [ImageQuery("Hatsune Miku Images")]
    [FromGoogle(type: GoogleImageType.Photo)]
    [FromDuckDuckGo(type: DuckDuckGoImageType.Photo)]
    [FromBrave(type: BraveImageType.Photo)]
    public required IEnumerable<IImageResult> Images { get; init; }

    [ImageQuery("Hatsune Miku GIFs")]
    [FromGoogle(type: GoogleImageType.Animated)]
    [FromDuckDuckGo(type: DuckDuckGoImageType.Gif)]
    [FromBrave(type: BraveImageType.AnimatedGifHttps)]
    public required IEnumerable<IImageResult> Gifs { get; init; }

    [ImageQuery("Hatsune Miku Futanari", SafeSearchLevel.Off)]
    [FromGoogle, FromDuckDuckGo, FromBrave]
    public required IEnumerable<IImageResult> Futanari { get; init; }

    [ImageQuery("Hatsune Miku Titties", SafeSearchLevel.Off)]
    [FromGoogle, FromDuckDuckGo, FromBrave]
    public required IEnumerable<IImageResult> Titties { get; init; }

    public static int GetImageCount()
    {
        IEnumerable<PropertyInfo> queryProperties = typeof(ImagesResults).GetProperties().Where(p => p.IsDefined(typeof(ImageQueryAttribute)));

        // Make abstract
        return
            (queryProperties.Count(p => p.IsDefined(typeof(FromGoogleAttribute))) * 100)
                + (queryProperties.Count(p => p.IsDefined(typeof(FromDuckDuckGoAttribute))) * 100)
                + (queryProperties.Count(p => p.IsDefined(typeof(FromBraveAttribute))) * 150);
    }
}