using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class ProductViewModel
{
    public IEnumerable<ProductDTO> Products { get; set; }
        = Enumerable.Empty<ProductDTO>();

    public TipoCambioResponse TipoCambio { get; set; }
        = new();
}

