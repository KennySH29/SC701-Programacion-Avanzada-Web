using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController :
        Controller
    {
        private readonly IComponentService
            _componentService;

        private readonly ILogger<ComponentController>
            _logger;


        public ComponentController(
            IComponentService componentService,
            ILogger<ComponentController> logger)
        {
            _componentService =
                componentService;

            _logger =
                logger;
        }


        public async Task<IActionResult> Index(
            int page = 1)
        {
            const int pageSize = 25;

            var components =
                await _componentService
                    .GetComponentsAsync();

            var componentList =
                components.ToList();

            var totalItems =
                componentList.Count;

            var totalPages =
                (int)Math.Ceiling(
                    totalItems /
                    (double)pageSize);


            if (page < 1)
            {
                page = 1;
            }


            if (totalPages > 0 &&
                page > totalPages)
            {
                page = totalPages;
            }


            var pagedComponents =
                componentList
                    .Skip(
                        (page - 1) *
                        pageSize)
                    .Take(pageSize)
                    .ToList();


            ViewBag.CurrentPage =
                page;

            ViewBag.TotalPages =
                totalPages;


            return View(
                pagedComponents);
        }


        public async Task<IActionResult> Details(
            decimal id)
        {
            var component =
                await _componentService
                    .GetComponentByIdAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Details",
                component);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return PartialView(
                "_Create",
                new ComponentDTO());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Content")]
            ComponentDTO component)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "_Create",
                    component);
            }

            await _componentService
                .CreateComponentAsync(
                    component);

            return RedirectToAction(
                nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(
            decimal id)
        {
            var component =
                await _componentService
                    .GetComponentByIdAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Edit",
                component);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id,Name,Content")]
            ComponentDTO component)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "_Edit",
                    component);
            }

            await _componentService
                .UpdateComponentAsync(
                    component);

            return RedirectToAction(
                nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(
            decimal id)
        {
            var component =
                await _componentService
                    .GetComponentByIdAsync(id);

            if (component == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Delete",
                component);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(decimal id)
        {
            await _componentService
                .DeleteComponentAsync(id);

            return RedirectToAction(
                nameof(Index));
        }


        [ResponseCache(
            Duration = 0,
            Location =
                ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id ??
                        HttpContext.TraceIdentifier
                });
        }
    }
}