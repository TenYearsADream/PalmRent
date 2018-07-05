using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace PalmRent.AngularJsTest
{
    /// <summary>
    /// Handler2 的摘要说明
    /// </summary>
    public class Handler2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
          var data =   context.Request["data"];
          var JsonData =   JsonConvert.DeserializeObject(data);
            
            //context.Response.ContentType = "text/plain";
            context.Response.Write(JsonData);
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