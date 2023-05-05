using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

/// <summary>
///     Model class for PatientMedicine table in database
/// </summary>
public class PatientMedicineModel : Model
{
    [Required] public int PatientModelId { get; set; } // Foreign key property

    public PatientModel Patient { get; set; }

    [Required] public int MedicineModelId { get; set; } // Foreign key property

    public MedicineModel Medicine { get; set; }

    [Required] public double Amount { get; set; }

    [Required] public string Unit { get; set; }

    [Required] public List<PatientTodoModel> Todos { get; set; }
}