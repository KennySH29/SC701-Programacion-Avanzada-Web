using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var inventories = await _inventoryService.GetInventoriesAsync();
            var inventoryList = inventories.ToList();
            var totalItems = inventoryList.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1)
            {
                page = 1;
            }

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var pagedInventories = inventoryList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedInventories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
                return NotFound();

            return PartialView("_Details", inventory);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new InventoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitPrice,UnitsInStock,ProductId,ModifiedBy")] InventoryDTO inventory)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", inventory);
            }

            await _inventoryService.CreateInventoryAsync(inventory);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
                return NotFound();

            return PartialView("_Edit", inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("InventoryId,UnitPrice,UnitsInStock,ProductId,ModifiedBy")]
            InventoryDTO inventory)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", inventory);
            }

            await _inventoryService.UpdateInventoryAsync(inventory);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
                return NotFound();

            return PartialView("_Delete", inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _inventoryService.DeleteInventoryAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
