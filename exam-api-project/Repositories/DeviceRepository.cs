using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
///     CRUD for Device repository
/// </summary>
public class DeviceRepository : IDeviceRepository
{
    // Dependency injection
    private readonly ExamContext _dbContext;

    /// <summary>
    ///     Constructor for dependency injection
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public DeviceRepository(ExamContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously checks if a device token exists in the database.
    /// </summary>
    /// <param name="token">The token to check.</param>
    /// <returns>A boolean value indicating whether the token exists.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when there is an error fetching the device with the given token from the database.
    /// </exception>
    public async Task<bool> DeviceTokenExistsAsync(string token)
    {
        try
        {
            // Check if the device exists in the database
            return await _dbContext.Devices.AnyAsync(device => token == device.Token);
        }
        catch (Exception e)
        {
            // Throw an exception if the device could not be fetched from the database
            throw new InvalidOperationException($"Error fetching device. with token: {token}", e);
        }
    }

    /// <summary>
    ///     Asynchronously creates a new device in the database.
    /// </summary>
    /// <param name="deviceModel">The device model to create.</param>
    /// <returns>The created <see cref="DeviceModel" />.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when there is an error creating the device in the database.
    /// </exception>
    public async Task<DeviceModel> CreateDeviceAsync(DeviceModel deviceModel)
    {
        try
        {
            // Add the device to the database
            var newDevice = await _dbContext.Devices.AddAsync(deviceModel);
            await _dbContext.SaveChangesAsync();
            return newDevice.Entity;
        }
        catch (Exception e)
        {
            // Throw an exception if the device could not be created in the database
            throw new InvalidOperationException("Error creating device.", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a device by its ID from the database.
    /// </summary>
    /// <param name="id">The ID of the device to retrieve.</param>
    /// <returns>The <see cref="DeviceModel" /> with the specified ID.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when there is an error fetching the device with the given ID from the database.
    /// </exception>
    public async Task<DeviceModel> GetDeviceByIdAsync(int id)
    {
        try
        {
            // Fetch the device with the specified ID from the database
            var device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == id);
            return device;
        }
        catch (Exception e)
        {
            // Throw an exception if the device could not be fetched from the database
            throw new InvalidOperationException($"Error fetching device. by id: {id}", e);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves all devices from the database.
    /// </summary>
    /// <returns>An IEnumerable of <see cref="DeviceModel" /> containing all devices.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when there is an error fetching devices from the database.
    /// </exception>
    public async Task<IEnumerable<DeviceModel>> GetAllDevicesAsync()
    {
        try
        {
            // Fetch all the departments from the database
            var devices = await _dbContext.Devices.ToListAsync();
            return devices;
        }
        catch (Exception e)
        {
            // Throw an exception if the department could not be fetched from the database
            throw new InvalidOperationException("Error fetching devices.", e);
        }
    }

    /// <summary>
    ///     Asynchronously updates an existing device in the database.
    /// </summary>
    /// <param name="deviceModel">The device model with updated information.</param>
    /// <returns>The updated <see cref="DeviceModel" />, or null if the device does not exist.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when there is an error updating the device in the database.
    /// </exception>
    public async Task<DeviceModel> UpdateDeviceAsync(DeviceModel deviceModel)
    {
        try
        {
            // Fetch the device from the database
            var result =
                await _dbContext.Devices.FirstOrDefaultAsync(device => deviceModel.Token == device.Token);
            // Update the device
            result.Token = deviceModel.Token ?? result.Token;
            result.UserModelId = deviceModel.UserModelId;
            _dbContext.Devices.Update(result);
            await _dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            // Throw an exception if the device could not be updated in the database
            throw new InvalidOperationException("Error updating device.", e);
        }
    }

    /// <summary>
    ///     Asynchronously deletes a device by its UserModelId from the database.
    /// </summary>
    /// <param name="token"></param>
    /// <returns>The deleted <see cref="DeviceModel" />, or null if the device does not exist.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when there is an error deleting the device with the given UserModelId from the database.
    /// </exception>
    public async Task DeleteDeviceByTokenIdAsync(string token)
    {
        try
        {
            // Fetch the device from the database
            var result = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Token == token);
            // Return null if the device does not exist
            if (result == null) throw new ArgumentException("Device does not exist.");
            // Else Delete the device
            _dbContext.Devices.Remove(result);
            await _dbContext.SaveChangesAsync();
 
        }
        catch (Exception e)
        {
            // Throw an exception if the device could not be deleted from the database
            throw new InvalidOperationException($"Error deleting device. with device id {token}", e);
        }
    }

    /// <summary>
    /// Gets the devices by department identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<DeviceModel>> GetDevicesByDepartmentIdAsync(int id)
    {
        try
        {
            var result = await _dbContext.Users.Where(x => x.DepartmentModelId == id).Include(x => x.Devices).ToListAsync();
            var devices = result.SelectMany(x => x.Devices);
            return devices;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Could not fetch devices by department id", e);
        }
    }
}