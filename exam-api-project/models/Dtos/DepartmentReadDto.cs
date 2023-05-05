using exam_api_project.models.Entities;

namespace exam_api_project.models.Dtos
{

    public record DepartmentReadDto(
        int Id,
        string Title,
        List<UserReadDto> Users,
        List<PatientReadDto> Patients
    );
}