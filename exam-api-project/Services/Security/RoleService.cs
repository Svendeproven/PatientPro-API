using exam_api_project.models.Entities;

namespace exam_api_project.Services.Security;

public class RoleService
{
    private readonly UserModel _user;
    private readonly Dictionary<string, int> _roles = new();

    public RoleService(UserModel user)
    {
        _user = user;
        _roles.Add("admin", 1);
        _roles.Add("user", 10);
    }

    public bool IsAdmin()
    {
        return GetRoleLevel(_user.Role) == GetRoleLevel("admin");
    }

    public bool IsUser()
    {
        return GetRoleLevel(_user.Role) <= GetRoleLevel("user");
    }

    public bool isSelf(int id)
    {
        return id == _user.Id;
    }

    private int GetRoleLevel(string role)
    {
        return _roles[role];
    }
}