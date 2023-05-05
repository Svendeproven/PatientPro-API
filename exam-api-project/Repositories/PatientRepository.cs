using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
/// CRUD for Patient repository
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly ExamContext _dbContext;

    /// <summary>
    ///     Constructor for dependency injection
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PatientRepository(ExamContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <summary>
    ///     Asynchronously creates a new patient record in the database.
    /// </summary>
    /// <param name="patient">The patient model to create.</param>
    /// <returns>The created <see cref="PatientModel" />.</returns>
    public async Task<PatientModel> CreatePatientAsync(PatientModel patient)
    {
        // Check if the patient already exists in the database
        var patientExists = await _dbContext.Patients.AnyAsync(p =>
            p.SocialSecurityNumber == patient.SocialSecurityNumber);
        // Throw an exception if the patient already exists with the same social security number
        if (patientExists)
            throw new InvalidOperationException($"Patient with SSN {patient.SocialSecurityNumber} already exists.");

        try
        {
            // Add the patient to the database and save changes
            var patientEntity = await _dbContext.Patients.AddAsync(patient);
            await _dbContext.SaveChangesAsync();
            return patientEntity.Entity;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient could not be added to the database
            throw new InvalidOperationException("Error creating patient.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a patient record by their Social Security Number from the database.
    /// </summary>
    /// <param name="socialSecurityNumber">The Social Security Number of the patient to retrieve.</param>
    /// <returns>The <see cref="PatientModel" /> with the specified Social Security Number.</returns>
    public async Task<PatientModel> GetPatientBySocialSecurityNumberAsync(string socialSecurityNumber)
    {
        try
        {
            // Find the patient by social security number
            var result =
                await _dbContext.Patients.FirstOrDefaultAsync(p => p.SocialSecurityNumber == socialSecurityNumber);
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient with SSN {socialSecurityNumber}.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves all patient records from the database.
    /// </summary>
    /// <returns>An enumerable of <see cref="PatientModel" /> containing all patient records.</returns>
    public async Task<IEnumerable<PatientModel>> GetAllPatientsAsync()
    {
        try
        {
            // Get all patients from the database
            var res = await _dbContext.Patients.FirstOrDefaultAsync();
            var result = await _dbContext.Patients.ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patients could not be fetched from the database
            throw new InvalidOperationException("Error fetching patients.", e);
        }
    }

    /// <summary>
    ///     Asynchronously updates an existing patient record in the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient record to update.</param>
    /// <param name="patient">The patient model with updated information.</param>
    /// <returns>The updated <see cref="PatientModel" />.</returns>
    public async Task<PatientModel> UpdatePatientByIdAsync(int id, PatientModel patient)
    {
        // Check if the patient data is valid before updating it in the database
        var result = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
        // Return null if the patient does not exist
        if (result == null) return null;
        // Else Update the patient data
        result.Name = patient.Name ?? result.Name;
        result.SocialSecurityNumber = patient.SocialSecurityNumber ?? result.SocialSecurityNumber;
        await _dbContext.SaveChangesAsync();
        return result;
    }

    /// <summary>
    ///     Asynchronously deletes a patient record by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient record to delete.</param>
    /// <returns>The deleted <see cref="PatientModel" />.</returns>
    public async Task<PatientModel> DeletePatientAsync(int id)
    {
        // Check if the patient exists before deleting it from the database
        var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
        // Return null if the patient does not exist
        if (patient == null) return null;
        // Delete the patient from the database
        var result = _dbContext.Patients.Remove(patient).Entity;
        await _dbContext.SaveChangesAsync();
        return result;
    }
}