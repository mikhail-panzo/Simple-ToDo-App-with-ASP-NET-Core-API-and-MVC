using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDtoModels.TaskDtos
{
    public class ToDoTaskDto
    {
        public string Title { get; set; } = null!;

        public string? Details { get; set; }

        public string? Location { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? Deadline { get; set; }

        public long? CategoryId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
