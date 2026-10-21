using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAsync();
    Task<UserDTO> GetUserByIdAsync(int id);
    Task<bool> CreateUserAsync(UserDTO user);
    Task<bool> UpdateUserAsync(UserDTO user);
    Task<bool> DeleteUserAsync(int id);
}

public class UserService : ServiceBase, IUserService
{
    private const string _path = "User";
    private readonly IRestProvider _restProvider;

    public UserService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var users = await JsonProvider.DeserializeAsync<IEnumerable<UserDTO>>(response);
        return users;
    }

    public async Task<UserDTO> GetUserByIdAsync(int Id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: Id.ToString());
        var user = await JsonProvider.DeserializeAsync<UserDTO>(response);
        return user;
    }

    public async Task<bool> CreateUserAsync(UserDTO user)
    {
        var json = JsonProvider.Serialize(user);
        await _restProvider.PostAsync(SetPathUrl(_path), json);
        return true;
    }

    public async Task<bool> UpdateUserAsync(UserDTO user)
    {
        var json = JsonProvider.Serialize(user);

        await _restProvider.PutAsync(
            SetPathUrl(_path),
            user.UserId.ToString(),
            json);

        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return true;
    }
}