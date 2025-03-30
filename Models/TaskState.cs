using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskState
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}