using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            IRoleService roleService,
            ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var roles = await _roleService.GetRolesAsync();
            var roleList = roles.ToList();

            var totalItems = roleList.Count;

            var totalPages =
                (int)Math.Ceiling(
                    totalItems / (double)pageSize);

            if (page < 1)
            {
                page = 1;
            }

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var pagedRoles = roleList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedRoles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var role =
                await _roleService.GetRoleByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return PartialView("_Details", role);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView(
                "_Create",
                new RoleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("RoleName")] RoleDTO role)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", role);
            }

            await _roleService.CreateRoleAsync(role);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role =
                await _roleService.GetRoleByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return PartialView("_Edit", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("RoleId,RoleName")]
            RoleDTO role)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", role);
            }

            await _roleService.UpdateRoleAsync(role);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var role =
                await _roleService.GetRoleByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return PartialView("_Delete", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roleService.DeleteRoleAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
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
