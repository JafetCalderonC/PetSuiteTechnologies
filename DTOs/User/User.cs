using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOs.User
{
    public class User : BaseDTO, IUser
    {
        public bool IsOtpVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentificationType { get; set; }
        public string IdentifierValue { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public IFormFile ProfilePic { get; set; }
        public double AddressLatitude { get; set; }
        public double AddressLongitude { get; set; }
        public byte Status { get; set; }
        public string ThemePreference { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<string> PhoneNumbers { get; set; }

        [JsonIgnore]
        public string ProfilePicUrl { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
