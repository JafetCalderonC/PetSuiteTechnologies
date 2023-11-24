using DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class UserMapper : Mapper<User>
    {
        public override User BuildObject(Dictionary<string, object> row)
        {
            var user = new User()
            {
                Id = (int)row["user_id"],
                IsPasswordRequiredChange = (bool)row["is_password_required_change"],
                PasswordHash = (string)row["password_hash"],
                PasswordSalt = (string)row["password_salt"],
                Role = (string)row["role"],
                Status = (byte)row["status"],
                FirstName = (string)row["first_name"],
                LastName = (string)row["last_name"],
                IdentificationType = (string)row["identification_type"],
                IdentificationValue = (string)row["identifier_value"],
                Email = (string)row["email"],
                CloudinaryPublicId = (string)row["cloudinary_public_id"],
                ThemePreference = (string)row["theme_preference"],
                CreatedDate = (DateTime)row["created_date"],
                ModifiedDate = (DateTime)row["modified_date"],
                AddressLatitude = (float)(double)row["address_latitude"],
                AddressLongitude = (float)(double)row["address_longitude"]
            };

            return user;
        }
    }
}