using PalmRent.FrontWeb.Models;
using PalmRent.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PalmRent.FrontWeb.Controllers
{
    public class HouseController : Controller
    {
        public IHouseService houseService { get; set; }
        public IAttachmentService attService { get; set; }
        public ICityService cityService { get; set; }
        public IRegionService regionService { get; set; }
        // GET: House
        public ActionResult Index(long id)
        {
            var house = houseService.GetById(id);
            if (house == null)
            {
                return View("Error", (object)"不存在的房源id");
            }
            var pics = houseService.GetPics(id);
            var attachments = attService.GetAttachments(id);

            HouseIndexViewModel model = new HouseIndexViewModel();
            model.House = house;
            model.Pics = pics;
            model.Attachments = attachments;
            return View(model);
        }
    }
}