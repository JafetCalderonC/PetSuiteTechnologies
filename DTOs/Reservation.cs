using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Reservation : BaseDTO
    {
        public int ReservationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UserID { get; set; }
        public int? PetId { get; set; }
        public int PackageId { get; set; }
        public DateTime? ReservationCreatedDate { get; set; }
        public DateTime? ReservationModifiedDate { get; set; }
    }
}
