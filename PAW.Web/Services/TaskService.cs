using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using Tasks = System.Threading.Tasks;

namespace PAW.Web.Services;

public interface ITaskService
{
    Tasks.Task<IEnumerable<TaskDTO>> GetTasksAsync();

    Tasks.Task<TaskDTO> GetTaskByIdAsync(int id);

    Tasks.Task<bool> CreateTaskAsync(TaskDTO task);

    Tasks.Task<bool> UpdateTaskAsync(TaskDTO task);

    Tasks.Task<bool> DeleteTaskAsync(int id);
}

public class TaskService : ServiceBase, ITaskService
{
    private const string _path = "Task";

    private readonly IRestProvider _restProvider;

    public TaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Tasks.Task<IEnumerable<TaskDTO>> GetTasksAsync()
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var tasks =
            await JsonProvider.DeserializeAsync<IEnumerable<TaskDTO>>(
                response);

        return tasks;
    }

    public async Tasks.Task<TaskDTO> GetTaskByIdAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id.ToString());

        var task =
            await JsonProvider.DeserializeAsync<TaskDTO>(
                response);

        return task;
    }

    public async Tasks.Task<bool> CreateTaskAsync(TaskDTO task)
    {
        var json =
            JsonProvider.Serialize(task);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            json);

        return true;
    }

    public async Tasks.Task<bool> UpdateTaskAsync(TaskDTO task)
    {
        var json =
            JsonProvider.Serialize(task);

        await _restProvider.PutAsync(
            SetPathUrl(_path),
            task.Id.ToString(),
            json);

        return true;
    }

    public async Tasks.Task<bool> DeleteTaskAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path),
            id.ToString());

        return true;
    }
}