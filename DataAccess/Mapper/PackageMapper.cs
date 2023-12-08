using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class PackageMapper : Mapper<Package>
    {
        public override Package BuildObject(Dictionary<string, object> row)
        {
            var package = new Package
            {
                Id = (int)row["package_id"],
                PackageName = (string)row["package_name"],
                Description = (string)row["description"],
                RoomId = (int)row["room_id"],
                PetBreedType = (string)row["pet_breed_type"],
                PetSize = (string)row["pet_size"],
                PetAggressiveness = (byte)row["pet_aggressiveness"],
                Status = (byte)row["status"],
                CreatedDate = (DateTime)row["created_date"],
                ModifiedDate = (DateTime)row["modified_date"],
    };
            return package;
        }
    }
}
