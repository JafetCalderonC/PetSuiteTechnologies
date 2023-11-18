using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTOs.User
{
    public class User : BaseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationValue { get; set; }
        public string Email { get; set; }
        public string ProfilePhoto { get; set; }
        public double AddressLatitude { get; set; }
        public double AddressLongitude { get; set; }
        public string ThemePreference { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public byte Status { get; set; }
        public string Role { get; set; }
        public bool IsPasswordRequiredChange { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? CloudinaryPublicId { get; set; }
    }
}
