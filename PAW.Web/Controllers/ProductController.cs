using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;
using System.Diagnostics;
using Task = System.Threading.Tasks.Task;


namespace PAW.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ICategoryService categoryService, ISupplierService supplierService, IInventoryService inventoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _inventoryService = inventoryService;
            _logger = logger;
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var products =
                await _productService.GetProductsAsync();

            var productList =
                products.ToList();

            var totalItems =
                productList.Count;

            var totalPages =
                (int)Math.Ceiling(
                    totalItems / (double)pageSize);

            // Evitar páginas menores a 1
            if (page < 1)
            {
                page = 1;
            }

            // Evitar páginas mayores a la última
            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var pagedProducts =
                productList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedProducts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            //Obtener las categorías
            var categories =
                await _categoryService.GetCategoriesAsync();


            //Obtener los proveedores
            var suppliers =
                await _supplierService.GetSuppliersAsync();


            //Obtener los inventarios
            var inventories =
                await _inventoryService.GetInventoriesAsync();

            // 5. Buscar la categoría asociada al producto
            ViewBag.Category =
                categories.FirstOrDefault(
                    x => x.CategoryId == product.CategoryId);


            // 6. Buscar el proveedor asociado al producto
            ViewBag.Supplier =
                suppliers.FirstOrDefault(
                    x => x.SupplierId == product.SupplierId);


            // 7. Buscar el inventario asociado al producto
            ViewBag.Inventory =
                inventories.FirstOrDefault(
                    x => x.InventoryId == product.InventoryId);

            return PartialView("_Details", product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadProductListsAsync();

            return PartialView("_Create", new ProductDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            if(!ModelState.IsValid)
            {
                await LoadProductListsAsync();
                return PartialView("_Create", product);
            }

            await _productService.CreateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var product = await _productService.GetProductByIdAsync(id);
            if(product == null)
            {
                return NotFound();

            }

            await LoadProductListsAsync();

            return PartialView("_Edit", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
           [Bind("ProductId,Name,Description,Rating,CreatedBy,ModifiedBy,CategoryId,SupplierId,InventoryId")]
    ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                await LoadProductListsAsync();

                return PartialView("_Edit", product);
            }

            await _productService
                .UpdateProductAsync(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product =
                await _productService
                    .GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            return PartialView("_Delete", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
           int id)
        {
            await _productService
                .DeleteProductAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task LoadProductListsAsync()
        {
            ViewBag.Categories =
                await _categoryService.GetCategoriesAsync();

            ViewBag.Suppliers =
                await _supplierService.GetSuppliersAsync();

            ViewBag.Inventories =
                await _inventoryService.GetInventoriesAsync();
        }
    }
}
