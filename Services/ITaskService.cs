using TaskManager.Models;
using System.Collections.Generic;

namespace TaskManager.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks();
        TaskItem GetTaskById(int id);
        void SaveTask(TaskItem task);
        void DeleteTask(int id);
        List<TaskCategory> GetAllCategories();
        void EnsureDefaultCategories();
        List<TaskPriority> GetAllPriorities();
        void EnsureDefaultPriorities();
        List<TaskState> GetAllStates(); // Đổi từ GetAllStatuses
        void EnsureDefaultStates(); // Đổi từ EnsureDefaultStatuses
    }
}