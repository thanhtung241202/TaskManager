using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: /Task/ListTasks
        public IActionResult ListTasks()
        {
            var tasks = _taskService.GetAllTasks();
            return View(tasks);
        }

        // GET: /Task/CreateTask
        public IActionResult CreateTask()
        {
            return View();
        }

        // POST: /Task/CreateTask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                _taskService.SaveTask(task);
                return RedirectToAction("ListTasks");
            }
            return View(task);
        }

        // GET: /Task/EditTask/1
        public IActionResult EditTask(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null || task.UserId != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: /Task/EditTask/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTask(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                _taskService.SaveTask(task);
                return RedirectToAction("ListTasks");
            }
            return View(task);
        }

        // GET: /Task/DetailsTask/1
        public IActionResult DetailsTask(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null || task.UserId != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(task);
        }

        // GET: /Task/DeleteTask/1
        public IActionResult DeleteTask(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null || task.UserId != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: /Task/DeleteTask/1
        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTaskConfirmed(int id)
        {
            _taskService.DeleteTask(id);
            return RedirectToAction("ListTasks");
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
