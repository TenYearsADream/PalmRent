using PalmRent.CommonMVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PalmRent.AdminWeb.App_Start
{
    public class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new PalmRentExceptionFilter());
            filters.Add(new JsonNetActionFilter());
            filters.Add(new PalmRentAuthorizeFilter());
        }
    }
}