﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.DTO
{
    public class HousePicDTO:BaseDTO
    {
        public long HouseId { get; set; }
        public String Url { get; set; }
        public String ThumbUrl { get; set; }
    }
}
