using DSharpPlus.CommandsNext;
using GScraper;
using HatsuneMiku.Services;
using HatsuneMiku.Shared.Enums;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.Image;

public abstract class BaseImageCommandModule : BaseCommandModule
{
    // Rename
    private bool _initialized = false;

    // Rename all
    protected IImageService ImageService { get; }

    // public?
    // Pack into struct or record
    // or IOptions
    // abstract?
    protected string Query { get; }
    protected ImageType ImageType { get; }
    protected SafeSearchLevel SafeSearchLevel { get; }

    // Duplicate, null or empty query check, maybe
    public BaseImageCommandModule(IImageService imageService, string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate)
    {
        ImageService = imageService;
        Query = query;
        ImageType = imageType;
        SafeSearchLevel = safeSearchLevel;
    }

    //[DSharpPlus.CommandsNext.Attributes.CheckBase]
    public override async Task BeforeExecutionAsync(CommandContext ctx)
    {
        // May or may not be necessary
        if (_initialized)
            return;

        await ImageService.GetOrAddImageResultsAsync(Query, ImageType, SafeSearchLevel).ConfigureAwait(false);

        _initialized = true;
    }
}