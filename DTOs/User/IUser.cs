﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.User
{
    public interface IUser
    {
        public int Id { get; set; }
        public string Role { get; set; }
    }
}
