using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.CategoryDtos
{
    public class CategoryDto
    {
        [Required]
        [DataType(DataType.Text)]
        [Length(1,50, ErrorMessage = "The category name must be at least 1 character long or less than or equal to 50 characters.")]
        public string Name { get; set; } = null!;

        [DataType(DataType.Text)]
        [MaxLength(255, ErrorMessage = "Summary cannot be more than 255 characters")]
        public string? Summary { get; set; }
    }

    public class CategoryWithIdDto : CategoryDto
    {
        public long Id { get; set; }
    }
}