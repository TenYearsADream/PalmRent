using PalmRent.AdminWeb.Models;
using PalmRent.CommonMVC;
using PalmRent.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PalmRent.AdminWeb.Controllers
{
    public class PermissionController : Controller
    {
        public IPermissionService PermSvc { get; set; }
        // GET: Permission
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var perms = PermSvc.GetAll();
            return View(perms);
        }

        public ActionResult GetDelete(long id)
        {
            PermSvc.MarkDeleted(id);
            //return RedirectToAction("List");//删除之后刷新
            return RedirectToAction(nameof(List));
        }
        public ActionResult Delete2(long id)
        {
            PermSvc.MarkDeleted(id);
            return Json(new AjaxResult { Status = "ok" });
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult Add(string name,string description)
        public ActionResult Add(PermissionAddNewModel model)
        {
            PermSvc.AddPermission(model.Name, model.Description);
            //return RedirectToAction(nameof(List));
            //todo:权限项名字不能重复
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var perm = PermSvc.GetById(id);
            return View(perm);
        }

        [HttpPost]
        public ActionResult Edit(PermissionEditModel model)
        {
            PermSvc.UpdatePermission(model.Id, model.Name, model.Description);
            //todo:检查name不能重复
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}