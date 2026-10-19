using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;


public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task<CategoryDTO> GetCategoryByIdAsync(int id);
    Task<bool> CreateCategoryAsync(CategoryDTO category);
    Task<bool> UpdateCategoryAsync(CategoryDTO category);
    Task<bool> DeleteCategoryAsync(int id);
}
public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var categories = await JsonProvider.DeserializeAsync<IEnumerable<CategoryDTO>>(response);
        return categories;
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
        var category = await JsonProvider.DeserializeAsync<CategoryDTO>(response);
        return category;
    }

    public async Task<bool> CreateCategoryAsync(CategoryDTO category)
    {
        var json = JsonProvider.Serialize(category);
        await _restProvider.PostAsync(SetPathUrl(_path), json);
        return true;
    }

    public async Task<bool> UpdateCategoryAsync(CategoryDTO category)
    {
        var json = JsonProvider.Serialize(category);

        await _restProvider.PutAsync(
            SetPathUrl(_path),
            category.CategoryId.ToString(),
            json);

        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return true;
    }
}
