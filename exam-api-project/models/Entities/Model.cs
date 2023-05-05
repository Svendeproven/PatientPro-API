using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

/// <summary>
///     This class is The base class for all objects that is entities
/// </summary>
public abstract class Model
{
    [Key] public int Id { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}