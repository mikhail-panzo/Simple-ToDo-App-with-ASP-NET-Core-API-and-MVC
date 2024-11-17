using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleToDoAppWebMVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public string? ErrorTitle { get; set; } = "An error occurred while processing your request.";

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
