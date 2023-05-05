using exam_api_project.models.Dtos;
using exam_api_project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers;

/// <summary>
///     Represents a controller for managing medicine-related operations in the API.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MedicineController : ControllerBase
{
    private readonly IMedicineService _medicineService;

    /// <summary>
    ///     Initializes a new instance of the MedicineController class.
    /// </summary>
    /// <param name="medicineService">An implementation of the IMedicineService interface.</param>
    public MedicineController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    /// <summary>
    ///     Gets all medicines.
    /// </summary>
    /// <returns>An ActionResult containing a list of MedicineReadDto objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicineReadDto>>> GetAllMedicineAsync()
    {
        try
        {
            // get all medicine
            var medicine = await _medicineService.GetAllMedicineAsync();
            // Return 204 No Content if the result is empty or null
            if (medicine == null || !medicine.Any()) return NoContent();
            // Return 200 OK with the result
            return Ok(medicine);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            // Return 500 Internal Server Error with the exception message
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Gets a medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the medicine.</param>
    /// <returns>An ActionResult containing a MedicineReadDto object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicineReadDto>> GetMedicineByIdAsync(int id)
    {
        try
        {
            // get medicine by id
            var medicine = await _medicineService.GetMedicineByIdAsync(id);
            // Return 404 Not Found if the result is null
            if (medicine == null)
                return NotFound($"Medicine with ID {id} not found.");
            // Return 200 OK with the result
            return Ok(medicine);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            // Return 500 Internal Server Error with the exception message
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Creates a new medicine.
    /// </summary>
    /// <param name="medicineWriteDto">A MedicineWriteDto object containing medicine information.</param>
    /// <returns>An ActionResult containing the created MedicineReadDto object.</returns>
    [HttpPost]
    public async Task<ActionResult<MedicineReadDto>> CreateNewMedicineAsync(
        [FromBody] MedicineWriteDto medicineWriteDto)
    {
        try
        {
            // create new medicine
            var medicine =
                await _medicineService.CreateNewMedicineAsync(medicineWriteDto);
            // return 201 status code
            return CreatedAtAction("GetMedicineById", new { id = medicine.Id },
                medicine);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            // Return 500 Internal Server Error with the exception message
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    ///     Updates a medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the medicine.</param>
    /// <param name="medicineWriteDto">A MedicineWriteDto object containing updated medicine information.</param>
    /// <returns>An ActionResult containing the updated MedicineReadDto object.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<MedicineReadDto>> UpdateMedicineByIdAsync(int id,
        [FromBody] MedicineWriteDto medicineWriteDto)
    {
        try
        {
            // update medicine
            var medicine = await _medicineService.UpdateMedicineByIdAsync(medicineWriteDto, id);
            // if medicine is null, return 404 status code
            if (medicine == null) return NotFound($"Medicine with ID {id} not found.");
            // return 200 status code
            return Ok(medicine);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            // Return 500 Internal Server Error with the exception message
            return StatusCode(500, e.Message);
        }
    }


    /// <summary>
    ///     Deletes a medicine by its ID.
    /// </summary>
    /// <param name="id">The ID of the medicine.</param>
    /// <returns>An ActionResult indicating the result of the operation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMedicineByIdAsync(int id)
    {
        try
        {
            // delete medicine by id
            var medicine = await _medicineService.DeleteMedicineByIdAsync(id);
            // if medicine is null, return 404 status code
            if (medicine == null) return NotFound($"Medicine with ID {id} not found.");
            // return 204 status code
            return NoContent();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            // Return 500 Internal Server Error with the exception message
            return StatusCode(500, e.Message);
        }
    }
}