using PalmRent.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalmRent.AdminWeb.Models
{
    public class AdminUserAddViewModel
    {
        public CityDTO[] Cities { get; set; }
        public RoleDTO[] Roles { get; set; }
    }
}