using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Responses
{
    /// <summary>
    /// A response object with message and data
    /// </summary>
    /// <typeparam name="Tdata"></typeparam>
    public class DataResponse<Tdata>
    {
        public string Message { get; set; } = string.Empty;

        public Tdata? Data { get; set; }
    }
}
