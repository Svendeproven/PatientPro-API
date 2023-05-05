using exam_api_project.models.Dtos;
using exam_api_project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Controllers;
/// <summary>
/// Represents a controller for managing patient-related operations in the API.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    /// <summary>
    /// Initializes a new instance of the PatientController class.
    /// </summary>
    /// <param name="patientService">An implementation of the IPatientService interface.</param>
    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Gets all patients.
    /// </summary>
    /// <returns>An ActionResult containing a list of PatientReadDto objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientReadDto>>> GetAllPatientsAsync()
    {
        try
        {
            var result = await _patientService.GetAllPatientsAsync();
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in GetAllPatientsAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Gets a patient by their social security number.
    /// </summary>
    /// <param name="socialSecurityNumber">The social security number of the patient.</param>
    /// <returns>An ActionResult containing a PatientReadDto object.</returns>
    [HttpGet("{socialSecurityNumber}")]
    public async Task<ActionResult<PatientReadDto>> GetPatientBySocialSecurityNumber(string socialSecurityNumber)
    {
        try
        {
            var result = await _patientService.GetPatientBySocialSecurityNumberAsync(socialSecurityNumber);
            // return 404 status code if patient is not found
            if (result == null)
                return NotFound($"Patient with social security number {socialSecurityNumber} not found.");
            // return 200 status code if patient is found
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in GetPatientBySocialSecurityNumber: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Creates a new patient.
    /// </summary>
    /// <param name="patient">A PatientWriteDto object containing patient information.</param>
    /// <returns>An ActionResult containing the created PatientReadDto object.</returns>
    [HttpPost]
    public async Task<ActionResult<PatientReadDto>> CreatePatientAsync([FromBody] PatientWriteDto patient)
    {
        try
        {
            var result = await _patientService.CreatePatientAsync(patient);
            // return 201 status code if patient is created
            return CreatedAtAction("GetPatientBySocialSecurityNumber",
                new { socialSecurityNumber = result.SocialSecurityNumber },
                result);
        }
        catch (InvalidOperationException e)
        {
            // return 409 status code if patient already exists
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in CreatePatientAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


    /// <summary>
    /// Updates a patient by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient.</param>
    /// <param name="patient">A PatientWriteDto object containing updated patient information.</param>
    /// <returns>An ActionResult containing the updated PatientReadDto object.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientReadDto>> UpdatePatientByIdAsync(int id,
        [FromBody] PatientWriteDto patient)
    {
        try
        {
            var result = await _patientService.UpdatePatientByIdAsync(id, patient);
            // return 404 status code if patient is not found
            if (result == null) return NotFound($"Patient with ID {id} not found.");
            // return 200 status code if patient is updated
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in UpdatePatientByIdAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Deletes a patient by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient.</param>
    /// <returns>An ActionResult indicating the result of the operation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatientByIdAsync(int id)
    {
        try
        {
            var result = await _patientService.DeletePatientAsync(id);
            // return 404 status code if patient is not found
            if (result == null) return NotFound($"Patient with ID {id} not found.");
            // return 204 status code if patient is deleted
            return NoContent();
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in DeletePatientByIdAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}