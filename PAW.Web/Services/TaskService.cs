using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using Tasks = System.Threading.Tasks;

namespace PAW.Web.Services;

public interface ITaskService
{
    Tasks.Task<IEnumerable<TaskDTO>> GetTasksAsync();
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
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var tasks = await JsonProvider.DeserializeAsync<IEnumerable<TaskDTO>>(response);
        return tasks;
    }
}
