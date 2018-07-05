using CaptchaGen;
using PalmRent.Common;
using PalmRent.CommonMVC;
using PalmRent.IService;
using System;
using System.Collections.Generic;
using System.IO;
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

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult CreateVerifyCode()
        {
            string verifyCode = CommonHelper.CreateVerifyCode(4);
            //验证码保存到TempData中最安全
            TempData["verifyCode"] = verifyCode;
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 15, 6);
            return File(ms, "image/jpeg");
        }


        public ActionResult SendSmsVerifyCode(string phoneNum, string verifyCode)
        {
            string serverVerifyCode = (string)TempData["verifyCode"];//取服务器中保存的图形验证码
            if (serverVerifyCode != verifyCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "图形验证码填写错误" });
            }
            /*
            //配置信息从T_Settings 表读取
            string appKey = settingService.GetValue("如鹏短信平台AppKey");
            string userName = settingService.GetValue("如鹏短信平台UserName");
            string tempId = settingService.GetValue("如鹏短信平台注册短信模板Id");

            //短信验证码一般都是数字
            string smsCode = new Random().Next(1000, 9999).ToString();
            TempData["smsCode"] = smsCode;//给ActionResult Register(UserRegModel model)用

            RuPengSMSSender smsSender = new RuPengSMSSender();
            smsSender.AppKey = appKey;
            smsSender.UserName = userName;
            var sendResult = smsSender.SendSMS(tempId, smsCode, phoneNum);
            */
            int a = 1;
            if (a==1)
            {
                //把发送验证码的手机号放到TempData，在注册的时候再次检查一下注册的是不是这个手机号
                //防止网站漏洞
                TempData["RegPhoneNum"] = phoneNum;

                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "请求验证码失败"
                });
            }
        }
    }
}