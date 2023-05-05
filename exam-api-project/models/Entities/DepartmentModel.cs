using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

/// <summary>
///     Model class for Department table in database
/// </summary>
public class DepartmentModel : Model
{
    [Required] [MaxLength(64)] public string Title { get; set; }

    public List<UserModel> Users { get; set; }
    public List<PatientModel> Patients { get; set; }
}