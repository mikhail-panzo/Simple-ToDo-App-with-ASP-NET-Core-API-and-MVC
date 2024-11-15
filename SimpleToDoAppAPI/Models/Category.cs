using System;
using System.Collections.Generic;
using SharedDtoModels.CategoryDtos;

namespace SimpleToDoAppAPI.Models;

public partial class Category
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Summary { get; set; }

    public virtual ICollection<ToDoTask> Tasks { get; set; } = new List<ToDoTask>();
}

// These are extension methods that maps Category to any CategoryDto and vice versa
public static class CategoryDtoExtensions
{
    // This method is for CategoryDto objects to map to a Category object
    public static Category ToCategory(this CategoryDto category) =>
        new Category
        {
            Name = category.Name,
            Summary = category.Summary
        };

    // This method is for Category objects to map to a CategoryDto object
    public static CategoryDto ToDto(this Category category) =>
        new CategoryDto
        {
            Name = category.Name,
            Summary = category.Summary
        };

    // This method is for Category objects to map to a CategoryDto object but for lists
    public static List<CategoryDto> ToDto(this List<Category> categories) =>
        categories.Select(category => category.ToDto()).ToList();
}
