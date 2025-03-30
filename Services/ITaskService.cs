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
    }
}