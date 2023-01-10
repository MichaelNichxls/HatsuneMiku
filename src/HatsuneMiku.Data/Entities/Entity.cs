using System.ComponentModel.DataAnnotations;

namespace HatsuneMiku.Data.Entities;

public abstract class Entity
{
    [Key]
    public int Id { get; set; }
}