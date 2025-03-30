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

        public bool IsDone { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;
    }
}