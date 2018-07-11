using PalmRent.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalmRent.FrontWeb.Models
{
    public class HouseSearchViewModel
    {
        public RegionDTO[] regions { get; set; }
        public HouseDTO[] houses { get; set; }
    }
}