using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class PetMapper : Mapper<Pet>
    {
        public override Pet BuildObject(Dictionary<string, object> row)
        {
            var pet = new Pet
            {
                Id = Convert.ToInt32(row["pet_id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                Status = (byte)row["status"],
                PetName = (string)row["pet_name"],
                Description = (string)row["description"],
                PetAge = Convert.ToInt32(row["age"]),
                PetBreedType = (string)row["breed"],
                PetAggressiveness = (byte)row["aggressiveness"],
                CreatedDate = (DateTime)row["created_date"],
                ModifiedDate = (DateTime)row["modified_date"]
            };
            return pet;
        } 
    }
}