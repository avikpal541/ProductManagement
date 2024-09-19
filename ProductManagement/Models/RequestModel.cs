using ProductManagement.Models.Validation;
using System.Runtime.InteropServices;

namespace ProductManagement.Models
{
    /// <summary>
    /// Request Model for filter
    /// </summary>
    public class RequestModel
    {
        public  int? minprice { get; set; }

        public int? maxprice { get; set; }

        [RequestModel_EnsureSizeValidation]
        public string? size { get; set; }

        public string? highlight { get; set; }
    }
}
