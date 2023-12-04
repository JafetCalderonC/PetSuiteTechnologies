using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOs
{
    public class Pet : BaseDTO
    {
        public string PetName { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public int PetAge { get; set; }
        public string PetBreedType { get; set; }
        public byte PetAggressiveness { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UserId { get; set; }
    }
}
