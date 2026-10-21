using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesAsync();
    Task<RoleDTO> GetRoleByIdAsync(int id);
    Task<bool> CreateRoleAsync(RoleDTO role);
    Task<bool> UpdateRoleAsync(RoleDTO role);
    Task<bool> DeleteRoleAsync(int id);
}

public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;

    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var roles = await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);
        return roles;
    }

    public async Task<RoleDTO> GetRoleByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var role = await JsonProvider.DeserializeAsync<RoleDTO>(response);
        return role;
    }

    public async Task<bool> CreateRoleAsync(RoleDTO role)
    {
        var json = JsonProvider.Serialize(role);
        await _restProvider.PostAsync(SetPathUrl(_path), json);
        return true;
    }

    public async Task<bool> UpdateRoleAsync(RoleDTO role)
    {
        var json = JsonProvider.Serialize(role);

        await _restProvider.PutAsync(
            SetPathUrl(_path),
            role.RoleId.ToString(),
            json);

        return true;
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return true;
    }
}
