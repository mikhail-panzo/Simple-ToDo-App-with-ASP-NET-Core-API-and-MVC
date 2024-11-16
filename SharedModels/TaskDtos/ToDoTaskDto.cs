

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.TaskDtos
{
    public class ToDoTaskDto
    {
        [Required]
        [DataType(DataType.Text)]
        [Length(1, 100, ErrorMessage = "Title must be between 1 to 100 characters long")]
        public string Title { get; set; } = null!;

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255, ErrorMessage = "Details cannot be more than 255 characters")]
        public string? Details { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(255, ErrorMessage = "Location cannot be more than 255 characters")]
        public string? Location { get; set; }

        [DataType(DataType.ImageUrl)]
        [MaxLength(255, ErrorMessage = "Image Url cannot be more than 255 characters")]
        public string? ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Deadline { get; set; }

        public long? CategoryId { get; set; }
    }
}
