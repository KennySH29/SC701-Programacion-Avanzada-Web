using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        private readonly ILogger<UserController>
            _logger;

        public UserController(
            IUserService userService,
            IRoleService roleService,
            ILogger<UserController> logger)
        {
            _userService =
                userService;

            _roleService =
                roleService;

            _logger =
                logger;
        }

        private async Task LoadRolesAsync(
            int? selectedRoleId = null)
        {
            var roles =
                await _roleService.GetRolesAsync();

            ViewBag.Roles =
                new SelectList(
                    roles,
                    "RoleId",
                    "RoleName",
                    selectedRoleId);
        }

        public async Task<IActionResult> Index(
            int page = 1)
        {
            const int pageSize = 25;

            var users =
                await _userService.GetUsersAsync();

            var userList =
                users.ToList();

            var totalItems =
                userList.Count;

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

            var pagedUsers =
                userList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            ViewBag.CurrentPage =
                page;

            ViewBag.TotalPages =
                totalPages;

            return View(pagedUsers);
        }

        public async Task<IActionResult> Details(
            int id)
        {
            var user =
                await _userService
                    .GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            /*
             * Para Details vamos a obtener el Role
             * correspondiente y mostrar su nombre.
             */
            var roles =
                await _roleService.GetRolesAsync();

            ViewBag.Role =
                roles.FirstOrDefault(
                    r => r.RoleId == user.RoleId);

            return PartialView(
                "_Details",
                user);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadRolesAsync();

            return PartialView(
                "_Create",
                new UserDTO
                {
                    IsActive = true
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Username,Email,Password,IsActive,RoleId,ModifiedBy,LastModifiedBy")]
            UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                await LoadRolesAsync(
                    user.RoleId);

                return PartialView(
                    "_Create",
                    user);
            }

            await _userService
                .CreateUserAsync(user);

            return RedirectToAction(
                nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id)
        {
            var user =
                await _userService
                    .GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await LoadRolesAsync(
                user.RoleId);

            return PartialView(
                "_Edit",
                user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind(
                "UserId,Username,Email,Password,IsActive,RoleId,ModifiedBy,LastModifiedBy")]
            UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                await LoadRolesAsync(
                    user.RoleId);

                return PartialView(
                    "_Edit",
                    user);
            }

            await _userService
                .UpdateUserAsync(user);

            return RedirectToAction(
                nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id)
        {
            var user =
                await _userService
                    .GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Delete",
                user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(int id)
        {
            await _userService
                .DeleteUserAsync(id);

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