using GScraper;
using HatsuneMiku.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HatsuneMiku.Data.Entities.Image;

public class ImageQueryEntity : Entity
{
    [Required]
    public string Query { get; set; }
    public ImageType ImageType { get; set; } = ImageType.Any;
    public SafeSearchLevel SafeSearchLevel { get; set; } = SafeSearchLevel.Moderate;

    // Rename
    // IEnumerable?
    public List<ImageQueryResultEntity> ImageResults { get; set; } = new();
}