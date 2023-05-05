using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
///     CRUD for Medicine repository
/// </summary>
public class MedicineRepository : IMedicineRepository
{
    private readonly ExamContext _dbContext;

    /// <summary>
    ///     Constructor for dependency injection
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public MedicineRepository(ExamContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously creates a new medicine in the database.
    /// </summary>
    /// <param name="medicine">The medicine model to create.</param>
    /// <returns>The created <see cref="MedicineModel" />.</returns>
    public async Task<MedicineModel> CreateNewMedicineAsync(MedicineModel medicine)
    {
        try
        {
            // Add medicine to database and save changes
            var result = await _dbContext.Medicines.AddAsync(medicine);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception e)
        {
            // Throw an exception if the medicine could not be added to the database
            throw new InvalidOperationException("Error creating medicine.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves all medicines from the database.
    /// </summary>
    /// <returns>A list of <see cref="MedicineModel" /> containing all medicines.</returns>
    public async Task<List<MedicineModel>> GetAllMedicineAsync()
    {
        try
        {
            // gets all medicine in the database
            var medicine = await _dbContext.Medicines.ToListAsync();
            return medicine;
        }
        catch (Exception e)
        {
            // Throw an exception if the medicine could not be fetched from the database
            throw new InvalidOperationException("Error fetching medicine.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a medicine by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the medicine to retrieve.</param>
    /// <returns>The <see cref="MedicineModel" /> with the specified ID.</returns>
    public async Task<MedicineModel> GetMedicineByIdAsync(int id)
    {
        try
        {
            // gets the medicine in the database by id
            var medicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            return medicine;
        }
        catch (Exception e)
        {
            // Throw an exception if the medicine could not be fetched from the database
            throw new InvalidOperationException($"Error fetching medicine with ID {id}.", e);
        }
    }

    /// <summary>
    ///     Asynchronously updates an existing medicine in the database by their ID.
    /// </summary>
    /// <param name="medicineUpdate">The medicine model with updated information.</param>
    /// <param name="id">The ID of the medicine to update.</param>
    /// <returns>The updated <see cref="MedicineModel" />, or null if the medicine does not exist.</returns>
    public async Task<MedicineModel> UpdateExistingMedicineAsync(MedicineModel medicineUpdate, int id)
    {
        try
        {
            // gets the medicine in the database by id and updates it
            var result = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            // Check if medicineUpdate is null and return ArgumentNullException
            if (result == null) return null;
            // Update medicine
            result.Description = medicineUpdate.Description ?? result.Description;
            result.Title = medicineUpdate.Title ?? result.Title;
            result.ActiveSubstance = medicineUpdate.ActiveSubstance ?? result.ActiveSubstance;
            result.PricePrMg = medicineUpdate.PricePrMg != 0 ? medicineUpdate.PricePrMg : result.PricePrMg;
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the medicine could not be updated in the database
            throw new InvalidOperationException($"Error updating medicine with ID {id}.", e);
        }
    }

    /// <summary>
    ///     Asynchronously deletes a medicine by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the medicine to delete.</param>
    /// <returns>The deleted <see cref="MedicineModel" />, or null if the medicine does not exist.</returns>
    public async Task<MedicineModel> DeleteMedicineByIdAsync(int id)
    {
        try
        {
            // gets the medicine in the database by id and deletes it
            var medicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null) return null;
            _dbContext.Medicines.Remove(medicine);
            await _dbContext.SaveChangesAsync();
            return medicine;
        }
        catch (Exception e)
        {
            // Throw an exception if the medicine could not be deleted from the database
            throw new InvalidOperationException($"Error deleting medicine with ID {id}.", e);
        }
    }
}