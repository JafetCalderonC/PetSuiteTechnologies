using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class RoomMapper : Mapper<Room>
    {
        public override Room BuildObject(Dictionary<string, object> row)
        {
            var room = new Room
            {
                Id = Convert.ToInt32(row["room_id"]),
                Name = (string)row["room_name"],
                Description = (string)row["description"],
                Cost = (decimal)row["cost"],
                Status = (byte)row["status"],
                CreatedDate = (DateTime)row["created_date"],
                ModifiedDate = (DateTime)row["modified_date"]
            };
            return room;
        }
    }
}