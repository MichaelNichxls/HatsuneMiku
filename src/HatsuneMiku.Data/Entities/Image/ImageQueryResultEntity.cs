using System.ComponentModel.DataAnnotations.Schema;

namespace HatsuneMiku.Data.Entities.Image;

public class ImageQueryResultEntity : Entity
{
    public int ImageQueryId { get; set; }
    public int ImageResultId { get; set; }

    [ForeignKey(nameof(ImageQueryId))]
    public ImageQueryEntity ImageQuery { get; set; }

    [ForeignKey(nameof(ImageResultId))]
    public ImageResultEntity ImageResult { get; set; }
}