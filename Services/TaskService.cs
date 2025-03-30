using TaskManager.Data;
using TaskManager.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public List<TaskItem> GetAllTasks()
        {
            string userId = GetCurrentUserId();
            return _context.TaskItems
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.Status) // Đổi từ TaskStatus thành TaskState
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        public TaskItem GetTaskById(int id)
        {
            string userId = GetCurrentUserId();
            return _context.TaskItems
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .FirstOrDefault(t => t.Id == id && t.UserId == userId);
        }

        public void SaveTask(TaskItem task)
        {
            task.UserId = GetCurrentUserId();
            if (task.CategoryId == null)
            {
                var defaultCategory = _context.TaskCategories
                    .FirstOrDefault(c => c.UserId == task.UserId && c.Name == "Công việc");
                if (defaultCategory != null)
                    task.CategoryId = defaultCategory.Id;
            }
            if (task.PriorityId == null)
            {
                var defaultPriority = _context.TaskPriorities
                    .FirstOrDefault(p => p.UserId == task.UserId && p.Name == "Thấp");
                if (defaultPriority != null)
                    task.PriorityId = defaultPriority.Id;
            }
            if (task.StatusId == null) // Gán trạng thái mặc định
            {
                var defaultStatus = _context.TaskStatuses
                    .FirstOrDefault(s => s.UserId == task.UserId && s.Name == "Chưa làm");
                if (defaultStatus != null)
                    task.StatusId = defaultStatus.Id;
            }
            if (task.Id == 0)
                _context.TaskItems.Add(task);
            else
                _context.TaskItems.Update(task);
            _context.SaveChanges();
        }

        public void DeleteTask(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                _context.SaveChanges();
            }
        }

        public List<TaskCategory> GetAllCategories()
        {
            EnsureDefaultCategories();
            string userId = GetCurrentUserId();
            return _context.TaskCategories
                .Where(c => c.UserId == userId)
                .ToList() ?? new List<TaskCategory>();
        }

        public void EnsureDefaultCategories()
        {
            string userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return;

            var existingCategories = _context.TaskCategories
                .Where(c => c.UserId == userId)
                .ToList();

            if (!existingCategories.Any(c => c.Name == "Công việc"))
            {
                _context.TaskCategories.Add(new TaskCategory { Name = "Công việc", UserId = userId });
            }
            if (!existingCategories.Any(c => c.Name == "Cá nhân"))
            {
                _context.TaskCategories.Add(new TaskCategory { Name = "Cá nhân", UserId = userId });
            }
            _context.SaveChanges();
        }

        public List<TaskPriority> GetAllPriorities()
        {
            EnsureDefaultPriorities();
            string userId = GetCurrentUserId();
            return _context.TaskPriorities
                .Where(p => p.UserId == userId)
                .ToList() ?? new List<TaskPriority>();
        }

        public void EnsureDefaultPriorities()
        {
            string userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return;

            var existingPriorities = _context.TaskPriorities
                .Where(p => p.UserId == userId)
                .ToList();

            if (!existingPriorities.Any(p => p.Name == "Thấp"))
            {
                _context.TaskPriorities.Add(new TaskPriority { Name = "Thấp", Color = "green", UserId = userId });
            }
            if (!existingPriorities.Any(p => p.Name == "Cao"))
            {
                _context.TaskPriorities.Add(new TaskPriority { Name = "Cao", Color = "red", UserId = userId });
            }
            _context.SaveChanges();
        }

        public List<TaskState> GetAllStates()
        {
            EnsureDefaultStates();
            string userId = GetCurrentUserId();
            return _context.TaskStatuses
                .Where(s => s.UserId == userId)
                .ToList() ?? new List<TaskState>(); // Đảm bảo không null
        }

        public void EnsureDefaultStates()
        {
            string userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return;

            var existingStates = _context.TaskStatuses
                .Where(s => s.UserId == userId)
                .ToList();

            if (!existingStates.Any(s => s.Name == "Chưa làm"))
            {
                _context.TaskStatuses.Add(new TaskState { Name = "Chưa làm", UserId = userId });
            }
            if (!existingStates.Any(s => s.Name == "Đang làm"))
            {
                _context.TaskStatuses.Add(new TaskState { Name = "Đang làm", UserId = userId });
            }
            if (!existingStates.Any(s => s.Name == "Xong"))
            {
                _context.TaskStatuses.Add(new TaskState { Name = "Xong", UserId = userId });
            }
            _context.SaveChanges();
        }
    }
}