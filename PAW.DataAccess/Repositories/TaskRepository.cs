using PawTask = PAW.Models.Task;
using Tasks = System.Threading.Tasks;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface ITaskRepository : IRepositoryBase<PawTask>
{
    Tasks.Task<bool> UpsertAsync(PawTask entity, bool isUpdating);
    Tasks.Task<bool> CreateAsync(PawTask entity);
    Tasks.Task<bool> DeleteAsync(PawTask entity);
    Tasks.Task<IEnumerable<PawTask>> ReadAsync();
    Tasks.Task<PawTask> FindAsync(int id);
    Tasks.Task<bool> UpdateAsync(PawTask entity);
    Tasks.Task<bool> UpdateManyAsync(IEnumerable<PawTask> entities);
    Tasks.Task<bool> ExistsAsync(PawTask entity);
}

public class TaskRepository : RepositoryBase<PawTask>, ITaskRepository
{
}
