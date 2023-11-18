using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Others
{
    public class PasswordOptions
    {
        public string LowerCase { get; set; }
        public string UpperCase { get; set; }
        public string Numbers { get; set; }
        public string SpecialCharacters { get; set; }

        public int MinLowerCase { get; set; }
        public int MinUpperCase { get; set; }
        public int MinNumbers { get; set; }
        public int MinSpecialCharacters { get; set; }
        public int MinPasswordLength { get; set; }
    }
}
