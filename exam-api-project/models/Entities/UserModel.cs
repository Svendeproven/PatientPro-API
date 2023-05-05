using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

/// <summary>
///     Entity for a user
/// </summary>
public class UserModel : Model
{
    [Required] [MaxLength(200)] public string Name { get; set; }

    [Required] [MaxLength(200)] public string Email { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(200)]
    public string Password { get; set; }

    public string JobTitle { get; set; }

    public string Role { get; set; } = "user";


    public int? DepartmentModelId { get; set; }

    public List<DeviceModel> Devices { get; set; }
}