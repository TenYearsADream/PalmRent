﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PalmRent.AdminWeb.App_Start
{
    public class PalmRentExceptionFilter : IExceptionFilter
    {
        private static ILog log = LogManager.GetLogger(typeof(PalmRentExceptionFilter));
        public void OnException(ExceptionContext filterContext)
        {
            log.Error("出现未处理异常", filterContext.Exception);
        }
    }
}