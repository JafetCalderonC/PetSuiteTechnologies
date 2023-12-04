using System;
using DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class ReservationMapper : Mapper<Reservation>
    {
        public override Reservation BuildObject(Dictionary<string, object> row)
        {
            var reservation = new Reservation
            {
                Id = Convert.ToInt32(row["reservation_id"]),
                StartDate = (DateOnly)row["start_date"],
                EndDate = (DateOnly)row["end_date"],
                UserID = Convert.ToInt32(row["user_id"]),
                PetId = Convert.ToInt32(row["pet_id"]),
                PackageId = Convert.ToInt32(row["package_id"]),
                ReservationCreatedDate = (DateTime)row["created_date"],
                ReservationModifiedDate = (DateTime)row["modified_date"]
            }; return reservation;
         

        }
     }             
     
}

