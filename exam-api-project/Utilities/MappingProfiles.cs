using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;

namespace exam_api_project.Utilities;

/// <summary>
///     Class for registering profiles for AutoMapper
/// </summary>
public class MappingProfiles : Profile
{
    /// <summary>
    ///     Constructor for registering profiles for AutoMapper
    /// </summary>
    public MappingProfiles()
    {
        // Source -> Target
        CreateMap<UserModel, UserReadDto>();
        CreateMap<UserWriteDto, UserModel>();

        // Source -> Target (Reverse)
        CreateMap<MedicineModel, MedicineReadDto>();
        CreateMap<MedicineWriteDto, MedicineModel>();
        // Source -> Target (Reverse)
        CreateMap<PatientModel, PatientReadDto>();
        CreateMap<PatientWriteDto, PatientModel>();

        CreateMap<DepartmentModel, DepartmentReadDto>();
        CreateMap<DepartmentWriteDto, DepartmentModel>();

        CreateMap<PatientTodoModel, PatientTodoReadDto>();
        CreateMap<PatientTodoWriteDto, PatientTodoModel>();

        CreateMap<PatientMedicineModel, PatientMedicineReadDto>();
        CreateMap<PatientMedicineWriteDto, PatientMedicineModel>();
        
        CreateMap<PatientJournalModel, PatientJournalReadDto>();
        CreateMap<PatientJournalWriteDto, PatientJournalModel>();
    }
}