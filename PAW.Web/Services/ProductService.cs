using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<bool> CreateProductAsync(ProductDTO product);
    Task<bool> UpdateProductAsync(ProductDTO product);
    Task<bool> DeleteProductAsync(int id);
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var products = await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);
        return products;
    }

    public async Task<ProductDTO> GetProductByIdAsync(int Id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: Id.ToString());
        var product = await JsonProvider.DeserializeAsync<ProductDTO>(response);
        return product;
    }

    public async Task<bool> CreateProductAsync(ProductDTO product)
    {
        var json = JsonProvider.Serialize(product);
        await _restProvider.PostAsync(SetPathUrl(_path), json);
        return true;
    }

    public async Task<bool> UpdateProductAsync(ProductDTO product)
    {
        var json = JsonProvider.Serialize(product);

        await _restProvider.PutAsync(
            SetPathUrl(_path),
            product.ProductId.ToString(),
            json);

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
        return true;
    }
}
