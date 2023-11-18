using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOs
{
    public class Package : BaseDTO
    {
        public string PackageName { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public string PetBreedType { get; set; }
        public string PetSize { get; set; }
        public byte PetAggressiveness { get; set; }
        public byte Status { get; set; }
        public List<int> Services { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
