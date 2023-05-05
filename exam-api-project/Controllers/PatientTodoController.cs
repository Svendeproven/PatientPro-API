using exam_api_project.models.Dtos;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers;

/// <summary>
///     Represents a controller for managing patient to-do-related operations in the API.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PatientTodoController : ControllerBase
{
    private readonly IPatientTodoService _patientTodoService;

    /// <summary>
    ///     Initializes a new instance of the PatientTodoController class.
    /// </summary>
    /// <param name="patientTodoService">An implementation of the IPatientTodoService interface.</param>
    public PatientTodoController(IPatientTodoService patientTodoService)
    {
        _patientTodoService = patientTodoService;
    }

    /// <summary>
    ///     Gets all patient to-dos.
    /// </summary>
    /// <returns>An ActionResult containing a list of PatientTodoReadDto objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientTodoReadDto>>> GetAllPatientTodoAsync()
    {
        try
        {
            // C
            var filters = QueryStringParser.Parse(Request.QueryString.Value);
            var patientTodos = await _patientTodoService.GetAllPatientTodosAsync(filters);
            return Ok(patientTodos);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error GetAllPatientTodoAsync: {Message}", e.Message);
            return StatusCode(500, "An error occurred while Get All Patient Todo .");
        }
    }

    /// <summary>
    ///     Gets a patient to-do by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to-do.</param>
    /// <returns>An ActionResult containing a PatientTodoReadDto object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientTodoReadDto>> GetPatientTodoByIdAsync(int id)
    {
        try
        {
            var patientTodo = await _patientTodoService.GetPatientTodoByIdAsync(id);
            return Ok(patientTodo);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error GetPatientTodoByIdAsync: {Message}", e.Message);
            return StatusCode(500, "An error occurred while Get Patient Todo By Id.");
        }
    }

    /// <summary>
    ///     Creates a new patient to-do.
    /// </summary>
    /// <param name="patientTodo">A PatientTodoWriteDto object containing patient to-do information.</param>
    /// <returns>An ActionResult containing the created PatientTodoReadDto object.</returns>
    [HttpPost]
    public async Task<ActionResult<PatientTodoReadDto>> CreatePatientTodoAsync(
        [FromBody] PatientTodoWriteDto patientTodo)
    {
        try
        {
            var patientTodoReadDto = await _patientTodoService.CreatePatientTodoAsync(patientTodo);
            // return created 201 status code
            return CreatedAtAction("GetPatientTodoById", new { patientTodoReadDto.Id }, patientTodoReadDto);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error creating patient to-do: {Message}", e.Message);
            return StatusCode(500, "An error occurred while creating patient to-do.");
        }
    }


    /// <summary>
    ///     Updates a patient to-do by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to-do.</param>
    /// <param name="patientTodo">A PatientTodoWriteDto object containing updated patient to-do information.</param>
    /// <returns>An ActionResult containing the updated PatientTodoReadDto object.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientTodoReadDto>> UpdatePatientTodoAsync(int id,
        [FromBody] PatientTodoWriteDto patientTodo)
    {
        try
        {
            var patientTodoReadDto = await _patientTodoService.UpdatePatientTodoByIdAsync(id, patientTodo);
            return Ok(patientTodoReadDto);
        }
        catch (InvalidOperationException e)
        {
            Log.Error("Error in UpdatePatientTodoAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in UpdatePatientTodoAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


    /// <summary>
    ///     Deletes a patient to-do by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient to-do.</param>
    /// <returns>An ActionResult containing the deleted PatientTodoReadDto object.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<PatientTodoReadDto>> DeletePatientTodoById(int id)
    {
        try
        {
            var patientTodoReadDto = await _patientTodoService.DeletePatientTodoAsync(id);
            return Ok(patientTodoReadDto);
        }
        catch (InvalidOperationException e)
        {
            Log.Error("Error in DeletePatientTodoById: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in DeletePatientTodoById: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}