using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalmRent.AngularJsTest
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            //context.Response.ContentType = "text/plain";
          var title =   context.Request["title"];
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}