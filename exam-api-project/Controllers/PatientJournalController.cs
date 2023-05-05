using exam_api_project.models.Dtos;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers;
/// <summary>
/// Represents a controller for managing patient journal-related operations in the API.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PatientJournalController : ControllerBase
{
    private readonly IPatientJournalService _patientJournalService;

    /// <summary>
    /// Initializes a new instance of the PatientJournalController class.
    /// </summary>
    /// <param name="patientJournalService">An implementation of the IPatientJournalService interface.</param>
    public PatientJournalController(IPatientJournalService patientJournalService)
    {
        _patientJournalService = patientJournalService;
    }

    /// <summary>
    /// Gets all patient journals.
    /// </summary>
    /// <returns>An ActionResult containing a list of PatientJournalReadDto objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientJournalReadDto>>> GetAllPatientJournalsAsync()
    {
        try
        {
            var filters = QueryStringParser.Parse(HttpContext.Request.QueryString.Value);
            var result = await _patientJournalService.GetAllJournalsAsync(filters);
            if (result == null || !result.Any())
                // Return 204 No Content if the result is empty or null
                return NoContent();
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
    /// Gets a patient journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient journal.</param>
    /// <returns>An ActionResult containing a PatientJournalReadDto object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientJournalReadDto>> GetPatientJournalByIdAsync(int id)
    {
        try
        {
            var result = await _patientJournalService.GetJournalByIdAsync(id);
            // Return 404 Not Found if the result is null
            if (result == null) return NotFound($"Patient journal with ID {id} not found.");
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
    /// Creates a new patient journal.
    /// </summary>
    /// <param name="patientJournal">A PatientJournalWriteDto object containing patient journal information.</param>
    /// <returns>An ActionResult containing the created PatientJournalReadDto object.</returns>
    [HttpPost]
    public async Task<ActionResult<PatientJournalReadDto>> CreateNewPatientJournalAsync(
        [FromBody] PatientJournalWriteDto patientJournal)
    {
        try
        {
            var result = await _patientJournalService.CreateJournalAsync(patientJournal);
            // return created 201 status code
            return CreatedAtAction("GetPatientJournalById", new { id = result.Id }, result);
        }
        catch (Exception e)
        {
            Log.Error(e.StackTrace);
            // Return 500 Internal Server Error with the exception message
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Updates a patient journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient journal.</param>
    /// <param name="patientJournal">A PatientJournalWriteDto object containing updated patient journal information.</param>
    /// <returns>An ActionResult containing the updated PatientJournalReadDto object.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientJournalReadDto>> UpdatePatientJournalByIdAsync(int id,
        [FromBody] PatientJournalWriteDto patientJournal)
    {
        try
        {
            var result = await _patientJournalService.UpdateJournalByIdAsync(id, patientJournal);
            // Return 404 Not Found if the result is null
            if (result == null) return NotFound($"Patient journal with ID {id} not found.");
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
    /// Deletes a patient journal by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient journal.</param>
    /// <returns>An ActionResult indicating the result of the operation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatientJournalById(int id)
    {
        try
        {
            var result = await _patientJournalService.DeleteJournalAsync(id);
            // Return 404 Not Found if the result is null
            if (result == null) return NotFound($"Patient journal with ID {id} not found.");
            // Return 204 No Content delete was successful
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