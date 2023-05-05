using exam_api_project.models.Dtos;
using exam_api_project.Services.Interfaces;
using exam_api_project.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers;

/// <summary>
///     Represents a controller for managing patient medicine-related operations in the API.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PatientMedicineController : ControllerBase
{
    private readonly IPatientMedicineService _patientMedicineService;

    /// <summary>
    ///     Initializes a new instance of the PatientMedicineController class.
    /// </summary>
    /// <param name="patientMedicineService">An implementation of the IPatientMedicineService interface.</param>
    public PatientMedicineController(IPatientMedicineService patientMedicineService)
    {
        _patientMedicineService = patientMedicineService;
    }

    /// <summary>
    ///     Gets all patient medicines.
    /// </summary>
    /// <returns>An ActionResult containing a list of PatientMedicineReadDto objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientMedicineReadDto>>> GetAllPatientMedicinesAsync()
    {
        try
        {
            var filters = QueryStringParser.Parse(Request.QueryString.Value);
            var patientMedicines = await _patientMedicineService.GetAllPatientMedicinesAsync(filters);
            return Ok(patientMedicines);
        }
        catch (ArgumentException e)
        {
            Log.Error("Invalid argument in GetAllPatientMedicinesAsync: {@Message} {@StackTrace}", e.Message,
                e.StackTrace);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in GetAllPatientMedicinesAsync: {@Message} {@StackTrace}", e.Message,
                e.StackTrace);
            return StatusCode(500, "An unexpected error occurred while retrieving patient medicines.");
        }
    }

    /// <summary>
    ///     Gets a patient medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient medicine.</param>
    /// <returns>An ActionResult containing a PatientMedicineReadDto object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientMedicineReadDto>> GetPatientMedicinesByIdAsync(int id)
    {
        try
        {
            var patientMedicine = await _patientMedicineService.GetPatientMedicineByIdAsync(id);
            // return 404 if patient medicine is not found
            if (patientMedicine == null) return NotFound($"Patient medicine with ID {id} not found.");
            // Else return 200 OK with the result
            return Ok(patientMedicine);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in GetPatientMedicinesByIdAsync: {@Message} {@StackTrace}", e.Message,
                e.StackTrace);
            return StatusCode(500, "An unexpected error occurred while retrieving patient medicine.");
        }
    }

    /// <summary>
    ///     Creates a new patient medicine.
    /// </summary>
    /// <param name="patientMedicineWriteDto">A PatientMedicineWriteDto object containing patient medicine information.</param>
    /// <returns>An ActionResult containing the created PatientMedicineReadDto object.</returns>
    [HttpPost]
    public async Task<ActionResult<PatientMedicineReadDto>> CreatePatientMedicinesAsync(
        [FromBody] PatientMedicineWriteDto patientMedicineWriteDto)
    {
        try
        {
            var patientMedicineReadDto =
                await _patientMedicineService.CreatePatientMedicineAsync(patientMedicineWriteDto);
            // return created 201 status code
            return CreatedAtAction("GetPatientMedicinesById", new { patientMedicineReadDto.Id },
                patientMedicineReadDto);
        }
        catch (InvalidOperationException e)
        {
            Log.Error("Error in CreatePatientMedicinesAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in CreatePatientMedicinesAsync: {@Message} {@StackTrace}", e.Message,
                e.StackTrace);
            return StatusCode(500, "An unexpected error occurred while creating patient medicine.");
        }
    }


    /// <summary>
    ///     Updates a patient medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient medicine.</param>
    /// <param name="patientMedicineWriteDto">A PatientMedicineWriteDto object containing updated patient medicine information.</param>
    /// <returns>An ActionResult containing the updated PatientMedicineReadDto object.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PatientMedicineReadDto>> UpdatePatientMedicinesByIdAsync(int id,
        [FromBody] PatientMedicineWriteDto patientMedicineWriteDto)
    {
        try
        {
            var result = await _patientMedicineService.UpdatePatientMedicineByIdAsync(id, patientMedicineWriteDto);
            return Ok(result);
        }
        catch (InvalidOperationException e)
        {
            Log.Error("Error in UpdatePatientMedicinesByIdAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in UpdatePatientMedicinesByIdAsync: {@Message} {@StackTrace}", e.Message,
                e.StackTrace);
            return StatusCode(500, "An unexpected error occurred while updating patient medicine.");
        }
    }

    /// <summary>
    ///     Deletes a patient medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the patient medicine.</param>
    /// <returns>An ActionResult containing the deleted PatientMedicineReadDto object.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<PatientMedicineReadDto>> DeletePatientMedicinesByIdAsync(int id)
    {
        try
        {
            var result = await _patientMedicineService.DeletePatientMedicineByIdAsync(id);
            if (result == null) return NotFound($"Patient medicine with ID {id} not found.");
            return result;
        }
        catch (InvalidOperationException e)
        {
            Log.Warning("Error in DeletePatientMedicinesByIdAsync: {@ErrorMessage}", e.Message);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Error in DeletePatientMedicinesByIdAsync: {@ErrorMessage} {@StackTrace}", e.Message,
                e.StackTrace);
            return StatusCode(500, "An error occurred while deleting the patient medicine.");
        }
    }
}