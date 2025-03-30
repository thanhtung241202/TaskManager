using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskPriority
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty; // Màu sắc để hiển thị (ví dụ: "green", "red")

        [Required]
        public string UserId { get; set; } = string.Empty; // Liên kết với người dùng
    }
}