using SharedModels.TaskDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleToDoAppAPI.Models;

public partial class ToDoTask
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Details { get; set; }

    public string? Location { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? Deadline { get; set; }

    public long? CategoryId { get; set; }

    public bool IsCompleted { get; set; }

    public virtual Category? Category { get; set; }
}

public static class ToDoTaskExtensions
{
    public static ToDoTaskDisplayDto ToDisplayDto(this ToDoTask task)
    {

        ToDoTaskDisplayDto toDoTaskDisplayDto = new ToDoTaskDisplayDto()
        {
            Title = task.Title,
            Deadline = task.Deadline,
            IsCompleted = task.IsCompleted,
            CategoryName = task.Category == null ? "No Category" : task.Category.Name
        };

        return toDoTaskDisplayDto;
    }

    public static ToDoTaskDisplayWithIdDto ToDisplayWithIdDto(this ToDoTask task) =>
        new ToDoTaskDisplayWithIdDto()
        {
            Id = task.Id,
            Title = task.Title,
            Deadline = task.Deadline,
            IsCompleted = task.IsCompleted,
            CategoryName = task.Category == null ? "No Category" : task.Category.Name
        };

    public static ToDoTaskDto ToDto(this ToDoTask task)
    {
        return new ToDoTaskDto()
        {
            Title = task.Title,
            Details = task.Details,
            Location = task.Location,
            ImageUrl = task.ImageUrl,
            Deadline = task.Deadline,
            CategoryId = task.CategoryId
        };
    }

    public static ToDoTask ToTask(this ToDoTaskDto taskDto)
    {
        return new ToDoTask()
        {
            Title = taskDto.Title,
            Details = taskDto.Details,
            Location = taskDto.Location,
            ImageUrl = taskDto.ImageUrl,
            Deadline = taskDto.Deadline,
            CategoryId = taskDto.CategoryId
        };
    }
}