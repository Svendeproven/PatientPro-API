using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

/// <summary>
///     Model class for Department table in database
/// </summary>
public class MedicineModel : Model
{
    [Required] [MaxLength(200)] public string Title { get; set; }
    [MaxLength(512)] public string Description { get; set; }
    [Required] [MaxLength(200)] public string ActiveSubstance { get; set; }
    [Required] public decimal PricePrMg { get; set; }
}