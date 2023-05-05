using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Utilities;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
///     CRUD for PatientJournal repository
/// </summary>
public class PatientJournalRepository : IPatientJournalRepository
{
    private readonly ExamContext _dbContext;

    /// <summary>
    ///     Constructor for dependency injection
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PatientJournalRepository(ExamContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously creates a new patient journal in the database.
    /// </summary>
    /// <param name="patientJournal">The patient journal model to create.</param>
    /// <returns>The created <see cref="PatientJournalModel" />.</returns>
    public async Task<PatientJournalModel> CreatePatientJournalAsync(PatientJournalModel patientJournal)
    {
        try
        {
            // Add the patient journal to the database
            var result = await _dbContext.PatientJournals.AddAsync(patientJournal);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be added to the database
            throw new InvalidOperationException("Error creating patient journal.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a patient journal by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient journal to retrieve.</param>
    /// <returns>The <see cref="PatientJournalModel" /> with the specified ID.</returns>
    public async Task<PatientJournalModel> GetPatientJournalByIdAsync(int id)
    {
        try
        {
            var result = await _dbContext.PatientJournals.FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient journal with ID {id}.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves all patient journals from the database.
    /// </summary>
    /// <returns>An enumerable of <see cref="PatientJournalModel" /> containing all patient journals.</returns>
    // Get all patient journals
    public async Task<IEnumerable<PatientJournalModel>> GetAllPatientJournalsAsync(List<Filter> filters = null)
    {
        try
        {
            // Fetch all patient journals from the database
            var query = _dbContext.PatientJournals.AsQueryable();
            query = query.FilterByProperties(filters);
            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journals could not be fetched from the database
            throw new InvalidOperationException("Error fetching all patient journals.", e);
        }
    }

    /// <summary>
    ///     Asynchronously updates an existing patient journal in the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient journal to update.</param>
    /// <param name="patientJournal">The patient journal model with updated information.</param>
    /// <returns>The updated <see cref="PatientJournalModel" />, or null if the patient journal does not exist.</returns>
    public async Task<PatientJournalModel> UpdatePatientJournalByIdAsync(int id, PatientJournalModel patientJournal)
    {
        try
        {
            var result = await _dbContext.PatientJournals.FirstOrDefaultAsync(p => p.Id == id);
            // Check if the patient journal exists if not return null
            if (result == null) return null;
            // else update the patient journal and save the changes
            result.Description = patientJournal.Description;
            result.PatientModelId = patientJournal.PatientModelId;
            _dbContext.PatientJournals.Update(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be updated
            throw new InvalidOperationException($"Error updating patient journal with ID {id}.", e);
        }
    }

    /// <summary>
    ///     Asynchronously deletes a patient journal by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient journal to delete.</param>
    /// <returns>The deleted <see cref="PatientJournalModel" />, or null if the patient journal does not exist.</returns>
    public async Task<PatientJournalModel> DeletePatientJournalAsync(int id)
    {
        try
        {
            var result = await _dbContext.PatientJournals.FirstOrDefaultAsync(p => p.Id == id);
            // Check if the patient journal exists if not return null
            if (result == null) return null;
            // else delete the patient journal and save the changes
            _dbContext.PatientJournals.Remove(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be deleted
            throw new InvalidOperationException($"Error deleting patient journal with ID {id}.", e);
        }
    }
}