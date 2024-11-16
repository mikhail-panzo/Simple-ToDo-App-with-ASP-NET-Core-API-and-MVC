using SharedModels.TaskDtos;

namespace SimpleToDoAppWebMVC.Models
{
    public class ToDoTaskGetAllOk
    {
        public string message = null!;

        public List<ToDoTaskDisplayDto> tasks = null!;
    }
}
