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
            var StartDate = (DateTime)row["start_date"];
            var EndDate = (DateTime)row["end_date"];
            var reservation = new Reservation
            {
                Id = Convert.ToInt32(row["reservation_id"]),
                StartDate = StartDate.Date,
                EndDate = EndDate.Date,
                UserID = Convert.ToInt32(row["user_id"]),
                PetId = Convert.ToInt32(row["pet_id"]),
                PackageId = Convert.ToInt32(row["package_id"]),
                ReservationCreatedDate = (DateTime)row["created_date"],
                ReservationModifiedDate = (DateTime)row["modified_date"]
            }; return reservation;
         

        }
     }             
     
}

