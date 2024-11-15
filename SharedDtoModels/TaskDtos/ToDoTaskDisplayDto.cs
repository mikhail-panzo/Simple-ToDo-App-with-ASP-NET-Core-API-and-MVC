using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDtoModels.TaskDtos
{
    public class ToDoTaskDisplayDto
    {
        [Required]
        [Length(1, 100, ErrorMessage = "Title must be between 1 to 100 characters long")]
        public string Title { get; set; } = null!;

        public DateTime? Deadline { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        public string? CategoryName { get; set; }
    }
}
