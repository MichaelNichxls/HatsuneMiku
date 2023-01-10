using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HatsuneMiku.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using DescriptionAttribute = DSharpPlus.CommandsNext.Attributes.DescriptionAttribute;

namespace HatsuneMiku.Commands.Nsfw;

// Hidden
// RequireGuild?
[RequireGuild, RequireNsfw]
[EditorBrowsable(EditorBrowsableState.Never)]
public class PopATittyCommand : BaseCommandModule
{
    //public IEnumerable<GoogleImageResult> Images { private get; init; }

    private readonly IEnumerable<string> _images;

    public PopATittyCommand(IDictionary<ImageType, IEnumerable<string>> images) =>
        _images = images[ImageType.PhotoNsfw];

    // Seed Random
    [Hidden]
    [Command("popatitty"), Aliases("popatit", "pop", "titty", "tit", "mikutitty", "mikutit")]
    [Description("Hatsune Miku pops a titty")]
    public async Task PopATitty(CommandContext ctx) =>
        await ctx.Channel.SendMessageAsync(new DiscordEmbedBuilder().WithImageUrl(_images.ElementAt(new Random().Next(_images.Count())))).ConfigureAwait(false);
}