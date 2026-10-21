using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;
using Tasks = System.Threading.Tasks;

namespace PAW.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskService taskService,
            ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public async Tasks.Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 25;

            var tasks =
                await _taskService.GetTasksAsync();

            var taskList =
                tasks.ToList();

            var totalItems =
                taskList.Count;

            var totalPages =
                (int)Math.Ceiling(
                    totalItems / (double)pageSize);

            if (page < 1)
            {
                page = 1;
            }

            if (totalPages > 0 &&
                page > totalPages)
            {
                page = totalPages;
            }

            var pagedTasks =
                taskList
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedTasks);
        }

        public async Tasks.Task<IActionResult> Details(int id)
        {
            var task =
                await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Details",
                task);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView(
                "_Create",
                new TaskDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Tasks.Task<IActionResult> Create(
            [Bind(
                "Name,Description,Status,DueDate,ModifiedBy")]
            TaskDTO task)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "_Create",
                    task);
            }

            await _taskService.CreateTaskAsync(task);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Tasks.Task<IActionResult> Edit(int id)
        {
            var task =
                await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Edit",
                task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Tasks.Task<IActionResult> Edit(
            [Bind(
                "Id,Name,Description,Status,DueDate,ModifiedBy")]
            TaskDTO task)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(
                    "_Edit",
                    task);
            }

            await _taskService.UpdateTaskAsync(task);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Tasks.Task<IActionResult> Delete(int id)
        {
            var task =
                await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return PartialView(
                "_Delete",
                task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Tasks.Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteTaskAsync(id);

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
