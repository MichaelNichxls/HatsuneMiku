using GScraper;
using System.ComponentModel.DataAnnotations;

namespace HatsuneMiku.Data.Entities.Image;

public class ImageResultEntity : Entity, IImageResult
{
    // init
    [Required]
    public string Url { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }

    public ImageQueryResultEntity ImageQuery { get; set; }
}