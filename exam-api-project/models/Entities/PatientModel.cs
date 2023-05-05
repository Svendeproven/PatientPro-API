using System.ComponentModel.DataAnnotations;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace exam_api_project.models.Entities;

/// <summary>
///     Model class for Patient table in database
/// </summary>
public class PatientModel : Model
{
    [Required] [MaxLength(200)] public string Name { get; set; }
    [EncryptColumn] // Encrypts the column in the database
    [Required] [MaxLength(64)] public string SocialSecurityNumber { get; set; }
    [Required] public int DepartmentModelId { get; set; }
}