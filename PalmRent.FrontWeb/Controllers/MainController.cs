using PalmRent.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PalmRent.FrontWeb.Controllers
{
    public class MainController : Controller
    {
        public ICityService CityService { get; set; }
        // GET: Main
        public ActionResult Index()
        {
            CityService.AddNew("哈尔滨");
            return Content("ok");
        }
    }
}