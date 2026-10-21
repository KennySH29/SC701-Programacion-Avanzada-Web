using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService
            _notificationService;

        private readonly ILogger<NotificationController>
            _logger;

        public NotificationController(
            INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _notificationService =
                notificationService;

            _logger =
                logger;
        }

        public async Task<IActionResult> Index(
            int page = 1)
        {
            const int pageSize = 25;

            var notifications =
                await _notificationService
                    .GetNotificationsAsync();

            var notificationList =
                notifications.ToList();

            var totalItems =
                notificationList.Count;

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

            var pagedNotifications =
                notificationList
                    .Skip(
                        (page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            ViewBag.CurrentPage =
                page;

            ViewBag.TotalPages =
                totalPages;

            return View(
                pagedNotifications);
        }

        public async Task<IActionResult> Details(
            int id)
        {
            var notification =
                await _notificationService
                    .GetNotificationByIdAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Details",
                notification);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView(
                "_Create",
                new NotificationDTO
                {
                    IsRead = false
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("UserId,Message,IsRead")]
            NotificationDTO notification)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "_Create",
                    notification);
            }

            await _notificationService
                .CreateNotificationAsync(
                    notification);

            return RedirectToAction(
                nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id)
        {
            var notification =
                await _notificationService
                    .GetNotificationByIdAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Edit",
                notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id,UserId,Message,IsRead")]
            NotificationDTO notification)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "_Edit",
                    notification);
            }

            await _notificationService
                .UpdateNotificationAsync(
                    notification);

            return RedirectToAction(
                nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id)
        {
            var notification =
                await _notificationService
                    .GetNotificationByIdAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Delete",
                notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(int id)
        {
            await _notificationService
                .DeleteNotificationAsync(id);

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
