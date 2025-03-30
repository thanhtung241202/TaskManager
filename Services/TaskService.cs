using TaskManager.Data;
using TaskManager.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
            return _context.TaskItems.Where(t => t.UserId == userId).OrderByDescending(t => t.Date).ToList();
        }

        public void SaveTask(TaskItem task)
        {
            task.UserId = GetCurrentUserId();
            if (task.Id == 0)
                _context.TaskItems.Add(task);
            else
                _context.TaskItems.Update(task);
            _context.SaveChanges();
        }

        public void DeleteTask(int id)
        {
            var task = _context.TaskItems.FirstOrDefault(t => t.Id == id && t.UserId == GetCurrentUserId());
            if (task != null)
            {
                _context.TaskItems.Remove(task);
                _context.SaveChanges();
            }
        }

        public TaskItem GetTaskById(int id)
        {
            string userId = GetCurrentUserId();
            return _context.TaskItems.FirstOrDefault(t => t.Id == id && t.UserId == userId);
        }
    }
}