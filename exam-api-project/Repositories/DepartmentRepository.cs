using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Utilities;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
///     CRUD for Department repository
/// </summary>
public class DepartmentRepository : IDepartmentRepository
{
    private readonly ExamContext _dbContext;

    /// <summary>
    ///     Constructor for dependency injection
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public DepartmentRepository(ExamContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously creates a new department in the database.
    /// </summary>
    /// <param name="department">The department model to create.</param>
    /// <returns>The created <see cref="DepartmentModel" />.</returns>
    public async Task<DepartmentModel> CreateDepartmentAsync(DepartmentModel department)
    {
        try
        {
            // add the department to the database
            var newDepartment = _dbContext.Departments.Add(department);
            await _dbContext.SaveChangesAsync();
            return newDepartment.Entity;
        }
        catch (Exception e)
        {
            // Throw an exception if the department could not be added to the database
            throw new InvalidOperationException("Error creating department.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a department by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <returns>The <see cref="DepartmentModel" /> with the specified ID.</returns>
    public async Task<DepartmentModel> GetDepartmentByIdAsync(int id)
    {
        try
        {
            // Find the department by id
            // var department = await _dbContext.Departments.FindAsync(id);
            var query = _dbContext.Departments
                .Include(x => x.Patients)
                .Include(x => x.Users)
                .AsQueryable();
            
            var department = await query.FirstOrDefaultAsync(x => x.Id == id);

            return department;
        }
        catch (Exception e)
        {
            // Throw an exception if the department could not be fetched from the database
            throw new InvalidOperationException($"Error fetching department with ID {id}.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves all departments from the database.
    /// </summary>
    /// <returns>An IEnumerable of <see cref="DepartmentModel" /> containing all departments.</returns>
    // Get all departments
    public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync(List<Filter> filters = null)
    {
        try
        {
            // Get all departments from the database
            var departments = _dbContext.Departments
                .Include(x => x.Patients)
                .Include(x => x.Users)
                .AsQueryable();
            departments.FilterByProperties(filters);
            return await departments.ToListAsync();
        }
        catch (Exception e)
        {
            // Throw an exception if the departments could not be fetched from the database
            throw new InvalidOperationException("Error fetching departments.", e);
        }
    }

    /// <summary>
    ///     Asynchronously updates an existing department in the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the department to update.</param>
    /// <param name="department">The department model with updated information.</param>
    /// <returns>The updated <see cref="DepartmentModel" />, or null if the department does not exist.</returns>
    public async Task<DepartmentModel> UpdateDepartmentByIdAsync(int id, DepartmentModel department)
    {
        try
        {
            // Find the department by id
            var result = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);
            // If the department does not exist, return null
            if (result == null) return null;
            // else update the department
            result.Title = department.Title ?? result.Title;
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the department could not be updated in the database
            throw new InvalidOperationException($"Error updating department with ID {id}.", e);
        }
    }

    /// <summary>
    ///     Asynchronously deletes a department by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the department to delete.</param>
    /// <returns>The deleted <see cref="DepartmentModel" />, or null if the department does not exist.</returns>
    public async Task<DepartmentModel> DeleteDepartmentByIdAsync(int id)
    {
        try
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);
            // If the department does not exist, return null
            if (department == null) return null;
            // else delete the department
            var result = _dbContext.Departments.Remove(department).Entity;
            await _dbContext.SaveChangesAsync();
            return result;
        }

        catch (Exception e)
        {
            // Throw an exception if the department could not be deleted from the database
            throw new InvalidOperationException($"Error deleting department with ID {id}.", e);
        }
    }
}