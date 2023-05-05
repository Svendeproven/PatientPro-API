using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

/// <summary>
///     Model class for Device table in database
/// </summary>
public class DeviceModel : Model
{
    [Required] [MaxLength(264)] public string Token { get; set; }

    [Required] public int UserModelId { get; set; } // Foreign key

}