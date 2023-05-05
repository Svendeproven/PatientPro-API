using exam_api_project.models.Entities;

namespace exam_api_project.Repositories.Interfaces;

public interface IDeviceRepository
{
    // CRUD methods for DeviceModel

    public Task<bool> DeviceTokenExistsAsync(string token);
    public Task<DeviceModel> CreateDeviceAsync(DeviceModel deviceModel);
    public Task<DeviceModel> GetDeviceByIdAsync(int id);
    public Task<IEnumerable<DeviceModel>> GetAllDevicesAsync();
    public Task<DeviceModel> UpdateDeviceAsync(DeviceModel deviceModel);
    public Task DeleteDeviceByTokenIdAsync(string token);
    public Task<IEnumerable<DeviceModel>> GetDevicesByDepartmentIdAsync(int id);
}