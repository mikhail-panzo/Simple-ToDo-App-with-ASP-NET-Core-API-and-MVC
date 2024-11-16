using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Responses
{
    /// <summary>
    /// A response object with just a message
    /// </summary>
    /// <typeparam name="Tdata"></typeparam>
    public class MessageResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}
