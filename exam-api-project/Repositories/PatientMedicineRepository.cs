using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Utilities;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
///  CRUD for PatientMedicine repository
/// </summary>
public class PatientMedicineRepository : IPatientMedicineRepository
{
    private readonly ExamContext _dbContext;

    /// <summary>
    ///     Constructor for dependency injection
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PatientMedicineRepository(ExamContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously creates a new patient medicine record in the database.
    /// </summary>
    /// <param name="patientMedicine">The patient medicine model to create.</param>
    /// <returns>The created <see cref="PatientMedicineModel" />.</returns>
    public async Task<PatientMedicineModel> CreatePatientMedicineAsync(PatientMedicineModel patientMedicine)
    {
        try
        {
            // create the patient medicine
            var result = await _dbContext.PatientMedicines.AddAsync(patientMedicine);
            await _dbContext.SaveChangesAsync();
            var res = await _dbContext.PatientMedicines.Include(x => x.Medicine).Include(x => x.Patient)
                .FirstAsync(x => x.Id == result.Entity.Id);
            return res;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient medicine could not be added to the database
            throw new InvalidOperationException("Error creating patient medicine.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a patient medicine record by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient medicine record to retrieve.</param>
    /// <returns>The <see cref="PatientMedicineModel" /> with the specified ID.</returns>
    public async Task<PatientMedicineModel> GetPatientMedicineByIdAsync(int id)
    {
        try
        {
            var result = await _dbContext.PatientMedicines.FirstOrDefaultAsync(pm => pm.Id == id);
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient journal. with id: {id}", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves all patient medicine records from the database, with optional filtering.
    /// </summary>
    /// <param name="filters">A list of filters to apply to the query, or null for no filtering.</param>
    /// <returns>An enumerable of <see cref="PatientMedicineModel" /> containing all patient medicine records.</returns>
    public async Task<IEnumerable<PatientMedicineModel>> GetAllPatientMedicinesAsync(List<Filter> filters = null)
    {
        try
        {
            // Get the query
            var query = _dbContext.PatientMedicines
                .Include(x => x.Patient)
                .Include(x => x.Medicine)
                .AsQueryable();

            // Filter the query if filters are provided
            query = query.FilterByProperties(filters);
            // Return the result
            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journals could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient journals. with filters: {filters}", e);
        }
    }

    /// <summary>
    ///     Asynchronously updates an existing patient medicine record in the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient medicine record to update.</param>
    /// <param name="patientMedicine">The patient medicine model with updated information.</param>
    /// <returns>The updated <see cref="PatientMedicineModel" />, or null if the patient medicine record does not exist.</returns>
    public async Task<PatientMedicineModel> UpdatePatientMedicineByIdAsync(int id, PatientMedicineModel patientMedicine)
    {
        try
        {
            // Get the patient medicine
            var result = await _dbContext.PatientMedicines.FirstOrDefaultAsync(pm => pm.Id == id);
            // Return null if the patient medicine does not exist
            if (result == null) return null;
            // Update the patient medicine
            result.Amount = patientMedicine.Amount;
            result.Unit = patientMedicine.Unit;
            _dbContext.PatientMedicines.Update(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be updated in the database
            throw new InvalidOperationException($"Error updating patient journal. with id: {id}", e);
        }
    }

    /// <summary>
    ///     Asynchronously deletes a patient medicine record by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient medicine record to delete.</param>
    /// <returns>The deleted <see cref="PatientMedicineModel" />, or null if the patient medicine record does not exist.</returns>
    public async Task<PatientMedicineModel> DeletePatientMedicineAsync(int id)
    {
        try
        {
            // Get the patient medicine
            var result = await _dbContext.PatientMedicines.FirstOrDefaultAsync(pm => pm.Id == id);
            // Return null if the patient medicine does not exist
            if (result == null) return null;
            // Delete the patient medicine
            _dbContext.PatientMedicines.Remove(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be deleted from the database
            throw new InvalidOperationException($"Error deleting patient journal. with id: {id}", e);
        }
    }
}