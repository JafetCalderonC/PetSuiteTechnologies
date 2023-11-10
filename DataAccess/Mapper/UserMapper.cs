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
            var obj = new User()
            {
                Id = (int)row["user_id"],
                IsOtpVerified = (byte)row["is_otp_verified"] == 1,
                PasswordHash = (string)row["password_hash"],
                Role = (string)row["role"],
                Status = (byte)row["status"],
                FirstName = (string)row["first_name"],
                LastName = (string)row["last_name"],
                IdentificationType = (string)row["identification_type"],
                IdentifierValue = (string)row["identifier_value"],
                Email = (string)row["email"],
                ProfilePicUrl = (string)row["profile_pic_url"],
                ThemePreference = (string)row["theme_preference"],
                CreatedDate = (DateTime)row["created_date"],
                ModifiedDate = (DateTime)row["modified_date"],
                AddressLatitude = (float)row["address_latitude"],
                AddressLongitude = (float)row["address_longitude"]
            };

            return obj;
        }
    }
}