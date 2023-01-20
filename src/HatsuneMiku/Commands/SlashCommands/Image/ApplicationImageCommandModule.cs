using DSharpPlus.SlashCommands;
using GScraper;
using HatsuneMiku.Services;
using HatsuneMiku.Shared.Enums;
using System.Threading.Tasks;

namespace HatsuneMiku.Commands.SlashCommands.Image;

public abstract class ApplicationImageCommandModule : ApplicationCommandModule
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
    public ApplicationImageCommandModule(IImageService imageService, string query, ImageType imageType = ImageType.Any, SafeSearchLevel safeSearchLevel = SafeSearchLevel.Moderate)
    {
        ImageService = imageService;
        Query = query;
        ImageType = imageType;
        SafeSearchLevel = safeSearchLevel;
    }

    //[DSharpPlus.CommandsNext.Attributes.CheckBase]
    public override async Task<bool> BeforeSlashExecutionAsync(InteractionContext ctx)
    {
        // May or may not be necessary
        if (_initialized)
            return true;

        await ImageService.GetOrAddImageResultsAsync(Query, ImageType, SafeSearchLevel).ConfigureAwait(false);

        // Shut up :)
        return _initialized = true;
    }
}