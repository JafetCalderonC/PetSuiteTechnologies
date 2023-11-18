using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Service :BaseDTO
    {
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; } 
        public int ServiceStatus { get; set; }
        public decimal ServiceCost { get; set; }
        public DateTime ServiceCreatedDate { get; set; }
        public DateTime? ServiceModifiedDate { get; set;}
    }
}
