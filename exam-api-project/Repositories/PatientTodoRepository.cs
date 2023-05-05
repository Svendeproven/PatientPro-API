using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Utilities;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
/// CRUD for PatientTodo repository
/// </summary>
public class PatientTodoRepository : IPatientTodoRepository
{
    private readonly ExamContext _dbContext;

    /// <summary>
    ///     Constructor for dependency injection
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PatientTodoRepository(ExamContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously creates a new patient todos record in the database.
    /// </summary>
    /// <param name="patientTodo">The patient todos model to create.</param>
    /// <returns>The created <see cref="PatientTodoModel" />.</returns>
    public async Task<PatientTodoModel> CreatePatientTodoAsync(PatientTodoModel patientTodo)
    {
        try
        {
            // create the patient medicine
            var result = await _dbContext.PatientTodos.AddAsync(patientTodo);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient medicine could not be added to the database
            throw new InvalidOperationException("Error creating patient todo.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a patient todos record by its ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient todos record to retrieve.</param>
    /// <returns>The <see cref="PatientTodoModel" /> with the specified ID.</returns>
    public async Task<PatientTodoModel> GetPatientTodoByIdAsync(int id)
    {
        try
        {
            // Get the patient Todos from the database
            var result = await _dbContext.PatientTodos.FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient todos. with id: {id}", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves all patient todos records from the database.
    /// </summary>
    /// <param name="filters">A list of filters to apply on the query.</param>
    /// <returns>An enumerable of <see cref="PatientTodoModel" /> containing all patient todos records.</returns>
    public async Task<IEnumerable<PatientTodoModel>> GetAllPatientTodosAsync(List<Filter> filters = null)
    {
        try
        {
            // Get the patient Todos from the database
            var query = _dbContext.PatientTodos
                .Include(x => x.PatientMedicine)
                .ThenInclude(x => x.Patient)
                .Include(x => x.PatientMedicine)
                .ThenInclude(x => x.Medicine)
                .AsQueryable();

            // Filter the patient Todos by the specified properties
            query = query.FilterByProperties(filters);
            // return the patient Todos
            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient todos. with filter: {filters}", e);
        }
    }

    /// <summary>
    ///     Asynchronously updates an existing patient todos record in the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient todos record to update.</param>
    /// <param name="patientTodo">The patient todos model with updated information.</param>
    /// <returns>The updated <see cref="PatientTodoModel" />.</returns>
    public async Task<PatientTodoModel> UpdatePatientTodoByIdAsync(int id, PatientTodoModel patientTodo)
    {
        try
        {
            // Get the patient Todos from the database
            var result = await _dbContext.PatientTodos.FirstOrDefaultAsync(p => p.Id == id);
            // Return null if no patient Todos were found
            if (result == null) return null;
            // Else return the patient Todos
            result.Done = patientTodo.Done;
            result.PlannedTimeAtDay = patientTodo.PlannedTimeAtDay;
            _dbContext.PatientTodos.Update(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient todos. with id: {id}", e);
        }
    }

    /// <summary>
    ///     Asynchronously deletes a patient todos record by its ID from the database.
    /// </summary>
    /// <param name="id">The ID of the patient todos record to delete.</param>
    /// <returns>The deleted <see cref="PatientTodoModel"/>.</returns>
    public async Task<PatientTodoModel> DeletePatientTodoAsync(int id)
    {
        try
        {
            // Get the patient Todos from the database
            var result = await _dbContext.PatientTodos.FirstOrDefaultAsync(p => p.Id == id);
            // Return null if no patient Todos were found
            if (result == null) return null;
            // Else return the patient Todos
            _dbContext.PatientTodos.Remove(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the patient journal could not be fetched from the database
            throw new InvalidOperationException($"Error fetching patient todos. with id: {id}", e);
        }
    }
}