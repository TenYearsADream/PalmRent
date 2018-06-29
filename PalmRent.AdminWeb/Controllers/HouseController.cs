using PalmRent.AdminWeb.App_Start;
using PalmRent.AdminWeb.Models;
using PalmRent.CommonMVC;
using PalmRent.DTO;
using PalmRent.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PalmRent.AdminWeb.Controllers
{
    public class HouseController : Controller
    {
        public IAdminUserService userSerivce { get; set; }

        public IHouseService houseService { get; set; }
        public ICityService cityService { get; set; }
        public IRegionService regionService { get; set; }
        public ICommunityService communityService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IAttachmentService attService { get; set; }
        // GET: House
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId">房源类型（整租、合租）</param>
        /// <returns></returns>
        [CheckPermission("House.List")]
        public ActionResult List(long typeId, int pageIndex = 1)
        {
            //因为AuthorizFilter做了是否登录的检查，因此这里不会取不到id
            long userId = (long)AdminHelper.GetUserId(HttpContext);
            long? cityId = userSerivce.GetById(userId).CityId;
            if (cityId == null)
            {
                //如果“总部不能***”的操作很多，也可以定义成一个AuthorizeFilter
                //最好用FilterAttribute的方式标注，这样对其他的不涉及这个问题的地方效率高
                //立即实现
                return View("Error", (object)"总部不能进行房源管理");
            }
            var houses = houseService.GetPagedData(cityId.Value, typeId, 10, (pageIndex - 1) * 10);
            long totalCount = houseService.GetTotalCount(cityId.Value, typeId);
            ViewBag.pageIndex = pageIndex;
            ViewBag.totalCount = totalCount;
            ViewBag.typeId = typeId;
            return View(houses);
        }

        public ActionResult LoadCommunities(long regionId)
        {
            var communities = communityService.GetByRegionId(regionId);
            return Json(new AjaxResult { Status = "ok", Data = communities });
        }

        [CheckPermission("House.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            long? cityId = userSerivce.GetById(userId.Value).CityId;
            if (cityId == null)
            {
                return View("Error", (object)"总部不能进行房源管理");
            }
            var regions = regionService.GetAll(cityId.Value);
            var roomTypes = idNameService.GetAll("户型");
            var statuses = idNameService.GetAll("房屋状态");
            var decorateStatuses = idNameService.GetAll("装修状态");
            var attachments = attService.GetAll();
            var types = idNameService.GetAll("房屋类别");

            HouseAddViewModel model = new HouseAddViewModel();
            model.regions = regions;
            model.roomTypes = roomTypes;
            model.statuses = statuses;
            model.decorateStatuses = decorateStatuses;
            model.attachments = attachments;
            model.types = types;
            return View(model);
        }

        [CheckPermission("House.Add")]
        [HttpPost]
        public ActionResult Add(HouseAddModel model)
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            long? cityId = userSerivce.GetById(userId.Value).CityId;
            if (cityId == null)
            {
                return View("Error", (object)"总部不能进行房源管理");
            }

            HouseAddNewDTO dto = new HouseAddNewDTO();
            dto.Address = model.address;
            dto.Area = model.area;
            dto.AttachmentIds = model.attachmentIds;
            dto.CheckInDateTime = model.checkInDateTime;
            dto.CommunityId = model.CommunityId;
            dto.DecorateStatusId = model.DecorateStatusId;
            dto.Description = model.description;
            dto.Direction = model.direction;
            dto.FloorIndex = model.floorIndex;
            dto.LookableDateTime = model.lookableDateTime;
            dto.MonthRent = model.monthRent;
            dto.OwnerName = model.ownerName;
            dto.OwnerPhoneNum = model.ownerPhoneNum;
            dto.RoomTypeId = model.RoomTypeId;
            dto.StatusId = model.StatusId;
            dto.TotalFloorCount = model.totalFloor;
            dto.TypeId = model.TypeId;

            long houseId = houseService.AddNew(dto);
         //   CreateStaticPage(houseId); //生成静态页面           

            //生成房源查看的html文件
            return Json(new AjaxResult { Status = "ok" });
        }

        //private void CreateStaticPage(long houseId)
        //{
        //    var house = houseService.GetById(houseId);
        //    var pics = houseService.GetPics(houseId);
        //    var attachments = attService.GetAttachments(houseId);

        //    HouseIndexViewModel model = new HouseIndexViewModel();
        //    model.House = house;
        //    model.Pics = pics;
        //    model.Attachments = attachments;
        //    string html = MVCHelper.RenderViewToString(this.ControllerContext, @"~/Views/House/StaticIndex.cshtml",
        //        model);
        //    System.IO.File.WriteAllText(@"E:\快盘\NextBig\NET课程\NET掌上租\上课代码\ZSZ\ZSZ.FrontWeb\" + houseId + ".html", html);
        //}

        [CheckPermission("House.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            long? cityId = userSerivce.GetById(userId.Value).CityId;
            if (cityId == null)
            {
                return View("Error", (object)"总部不能进行房源管理");
            }
            var house = houseService.GetById(id);
            HouseEditViewModel model = new HouseEditViewModel();
            model.house = house;

            var regions = regionService.GetAll(cityId.Value);
            var roomTypes = idNameService.GetAll("户型");
            var statuses = idNameService.GetAll("房屋状态");
            var decorateStatuses = idNameService.GetAll("装修状态");
            var attachments = attService.GetAll();
            var types = idNameService.GetAll("房屋类别");

            model.regions = regions;
            model.roomTypes = roomTypes;
            model.statuses = statuses;
            model.decorateStatuses = decorateStatuses;
            model.attachments = attachments;
            model.types = types;
            return View(model);
        }

        [CheckPermission("House.Edit")]
        [HttpPost]
        public ActionResult Edit(HouseEditModel model)
        {
            HouseDTO dto = new HouseDTO();
            dto.Address = model.address;
            dto.Area = model.area;
            dto.AttachmentIds = model.attachmentIds;
            dto.CheckInDateTime = model.checkInDateTime;
            //有没有感觉强硬用一些不适合的DTO，有一些没用的属性时候的迷茫？
            dto.CommunityId = model.CommunityId;
            dto.DecorateStatusId = model.DecorateStatusId;
            dto.Description = model.description;
            dto.Direction = model.direction;
            dto.FloorIndex = model.floorIndex;
            dto.Id = model.Id;
            dto.LookableDateTime = model.lookableDateTime;
            dto.MonthRent = model.monthRent;
            dto.OwnerName = model.ownerName;
            dto.OwnerPhoneNum = model.ownerPhoneNum;
            dto.RoomTypeId = model.RoomTypeId;
            dto.StatusId = model.StatusId;
            dto.TotalFloorCount = model.totalFloor;
            dto.TypeId = model.TypeId;
            houseService.Update(dto);

            //CreateStaticPage(model.Id);//编辑房源的时候重新生成静态页面
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}