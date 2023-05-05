using exam_api_project.models.Dtos;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers
{

    /// <summary>
    ///     Represents a controller for managing department-related operations in the API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;


        /// <summary>
        /// Initializes a new instance of the DepartmentController class.
        /// </summary>
        /// <param name="departmentService">An implementation of the IDepartmentService interface.</param>
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Retrieves all departments with optional query string filtering.
        /// </summary>
        /// <returns>An ActionResult containing a list of DepartmentReadDto objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentReadDto>>> GetAllDepartmentsAsync()
        {
            try
            {
                var filters = QueryStringParser.Parse(HttpContext.Request.QueryString.Value);
                var result = await _departmentService.GetAllDepartmentsAsync(filters);
                // Return 204 No Content if the result is empty or null
                if (result == null || !result.Any()) return NoContent();
                // Return 200 OK with the result
                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                // Return 500 Internal Server Error with the exception message
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Retrieves a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department to retrieve.</param>
        /// <returns>An ActionResult containing a DepartmentReadDto object.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentReadDto>> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var result = await _departmentService.GetDepartmentByIdAsync(id);
                // Return 404 Not Found if the result is null
                if (result == null) return NotFound($"Department with ID {id} not found.");
                // Return 200 OK with the result
                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                // Return 500 Internal Server Error with the exception message
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="department">A DepartmentWriteDto object containing the department data.</param>
        /// <returns>An ActionResult containing the created DepartmentReadDto object.</returns>
        [HttpPost]
        public async Task<ActionResult<DepartmentReadDto>> CreateNewDepartmentAsync(
            [FromBody] DepartmentWriteDto department)
        {
            try
            {
                var result =
                    await _departmentService.CreateDepartmentAsync(department);
                // Return 201 Created with the created department
                return CreatedAtAction("GetDepartmentById", new { result.Id }, result);
            }
            catch (ArgumentNullException e)
            {
                // Return invalid argument exception
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                // Return 500 Internal Server Error with the exception message
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Updates a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department to update.</param>
        /// <param name="department">A DepartmentWriteDto object containing the updated department data.</param>
        /// <returns>An ActionResult containing the updated DepartmentReadDto object.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentReadDto>> UpdateApartmentByIdAsync(int id,
            [FromBody] DepartmentWriteDto department)
        {
            try
            {
                var result =
                    await _departmentService.UpdateDepartmentByIdAsync(department, id);
                // Return 404 Not Found if the result is null
                if (result == null) return NotFound($"Department with ID {id} not found.");
                // Return 200 OK with the result
                return Ok(result);
            }
            catch (ArgumentNullException e)
            {
                // Return invalid argument exception
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Deletes a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department to delete.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartmentByIdAsync(int id)
        {
            try
            {
                var department = await _departmentService.DeleteDepartmentByIdAsync(id);
                // Return 404 Not Found if the result is null
                if (department == null) return NotFound($"Department with ID {id} not found.");
                // return no content if delete was successful
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace);
                // Return 500 Internal Server Error with the exception message
                return StatusCode(500, e.Message);
            }
        }
    }
}