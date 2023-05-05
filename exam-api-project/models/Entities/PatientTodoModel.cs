using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

/// <summary>
///     Model class for PatientTodo table in database
///     This class holds 1 to many relation to PatientMedicineModel
/// </summary>
public class PatientTodoModel : Model
{
    [Required] public int PatientMedicineModelId { get; set; } // Foreign key
    public PatientMedicineModel PatientMedicine { get; set; }
    public bool Done { get; set; }
    public int? UserModelId { get; set; } // Foreign key
    public UserModel User { get; set; }
    [Required] public DateTime PlannedTimeAtDay { get; set; }
    [Required] public int PatientModelId { get; set; }
    
}