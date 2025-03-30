using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.Models;
using System.Security.Claims;

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

        public IActionResult ListTasks()
        {
            var tasks = _taskService.GetAllTasks();
            return View(tasks);
        }

        public IActionResult CreateTask()
        {
            ViewBag.Categories = _taskService.GetAllCategories();
            ViewBag.Priorities = _taskService.GetAllPriorities(); // Thêm dòng này
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                _taskService.SaveTask(task);
                return RedirectToAction("ListTasks");
            }
            ViewBag.Categories = _taskService.GetAllCategories();
            ViewBag.Priorities = _taskService.GetAllPriorities();
            return View(task);
        }

        public IActionResult EditTask(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _taskService.GetAllCategories();
            ViewBag.Priorities = _taskService.GetAllPriorities();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTask(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                _taskService.SaveTask(task);
                return RedirectToAction("ListTasks");
            }
            ViewBag.Categories = _taskService.GetAllCategories();
            ViewBag.Priorities = _taskService.GetAllPriorities();
            return View(task);
        }

        public IActionResult DetailsTask(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        public IActionResult DeleteTask(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTaskConfirmed(int id)
        {
            _taskService.DeleteTask(id);
            return RedirectToAction("ListTasks");
        }
    }
}