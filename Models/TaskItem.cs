using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        public TaskCategory? Category { get; set; }

        public int? PriorityId { get; set; }
        public TaskPriority? Priority { get; set; }

        public int? StatusId { get; set; }
        public TaskState? Status { get; set; } // Đổi từ TaskStatus thành TaskState
    }
}