using Microsoft.EntityFrameworkCore;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IComponentRepository : IRepositoryBase<Component>
{
    Task<bool> UpsertAsync(Component entity, bool isUpdating);
    Task<bool> CreateAsync(Component entity);
    Task<bool> DeleteAsync(Component entity);
    Task<IEnumerable<Component>> ReadAsync();
    Task<Component> FindAsync(int id);
    Task<Component?> FindByIdAsync(decimal id);
    Task<bool> UpdateAsync(Component entity);
    Task<bool> UpdateManyAsync(IEnumerable<Component> entities);
    Task<bool> ExistsAsync(Component entity);
}

public class ComponentRepository : RepositoryBase<Component>, IComponentRepository
{
    public async Task<Component?> FindByIdAsync(
        decimal id)
    {
        return await DbContext
            .Set<Component>()
            .FirstOrDefaultAsync(
                component => component.Id == id);
    }
}
