﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Room : BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public byte Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}