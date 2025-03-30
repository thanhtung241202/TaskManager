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

// Sửa phương thức ListTasks để nhận tham số lọc
        public IActionResult ListTasks(string categoryFilter = null, string priorityFilter = null, string statusFilter = null)
        {
            var tasks = _taskService.GetAllTasks();

            // Áp dụng bộ lọc nếu có
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                int categoryId = _taskService.GetAllCategories().FirstOrDefault(c => c.Name == categoryFilter)?.Id ?? 0;
                tasks = tasks.Where(t => t.CategoryId == categoryId).ToList();
            }
            if (!string.IsNullOrEmpty(priorityFilter))
            {
                int priorityId = _taskService.GetAllPriorities().FirstOrDefault(p => p.Name == priorityFilter)?.Id ?? 0;
                tasks = tasks.Where(t => t.PriorityId == priorityId).ToList();
            }
            if (!string.IsNullOrEmpty(statusFilter))
            {
                int statusId = _taskService.GetAllStates().FirstOrDefault(s => s.Name == statusFilter)?.Id ?? 0;
                tasks = tasks.Where(t => t.StatusId == statusId).ToList();
            }

            // Truyền danh sách để hiển thị trong dropdown
            ViewBag.Categories = _taskService.GetAllCategories();
            ViewBag.Priorities = _taskService.GetAllPriorities();
            ViewBag.States = _taskService.GetAllStates();
            ViewBag.CurrentCategoryFilter = categoryFilter;
            ViewBag.CurrentPriorityFilter = priorityFilter;
            ViewBag.CurrentStatusFilter = statusFilter;

            return View(tasks);
        }

        public IActionResult CreateTask()
        {
            ViewBag.Categories = _taskService.GetAllCategories();
            ViewBag.Priorities = _taskService.GetAllPriorities(); // Thêm dòng này
            ViewBag.States = _taskService.GetAllStates();
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