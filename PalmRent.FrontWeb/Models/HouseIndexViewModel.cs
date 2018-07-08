﻿using PalmRent.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalmRent.FrontWeb.Models
{
    public class HouseIndexViewModel
    {
        public HouseDTO House { get; set; }
        public HousePicDTO[] Pics { get; set; }
        public AttachmentDTO[] Attachments { get; set; }
    }
}