using System.ComponentModel.DataAnnotations;

namespace exam_api_project.models.Entities;

public class PatientJournalModel : Model
{
    public string Description { get; set; }
   
    [Required]
    public int PatientModelId { get; set; } 
    public PatientModel Patient { get; set; } 
}