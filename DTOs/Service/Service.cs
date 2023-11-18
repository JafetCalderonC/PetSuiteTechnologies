using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOs.Service
{
    public class Service : BaseDTO
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public float Cost { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
