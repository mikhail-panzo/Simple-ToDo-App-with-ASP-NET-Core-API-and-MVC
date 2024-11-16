using SharedModels.CategoryDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.TaskDtos
{
    public class ToDoTaskDisplayDto
    {
        [Required]
        [DataType(DataType.Text)]
        [Length(1, 100, ErrorMessage = "Title must be between 1 to 100 characters long")]
        public string Title { get; set; } = null!;

        [DataType(DataType.DateTime)]
        public DateTime? Deadline { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [DataType(DataType.Text)]
        public string? CategoryName { get; set; }
    }

    public class ToDoTaskDisplayWithIdDto : ToDoTaskDisplayDto
    {
        public long Id { get; set; }
    }
}
