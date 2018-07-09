using PalmRent.DTO;
using PalmRent.IService;
using PalmRent.Service.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.Service
{
    public class HouseService : IHouseService
    {
        /// <summary>
        /// 新增房屋
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public long AddNew(HouseAddNewDTO house)
        {
            HouseEntity houseEntity = new HouseEntity();
            houseEntity.Address = house.Address;
            houseEntity.Area = house.Area;

            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<AttachmentEntity> attBS
                    = new BaseService<AttachmentEntity>(ctx);
                //拿到house.AttachmentIds为主键的房屋配套设施
                var atts = attBS.GetAll().Where(a => house.AttachmentIds.Contains(a.Id));
                //houseEntity.Attachments = new List<AttachmentEntity>();
                foreach (var att in atts)
                {
                    houseEntity.Attachments.Add(att);
                }
                houseEntity.CheckInDateTime = house.CheckInDateTime;
                houseEntity.CommunityId = house.CommunityId;
                houseEntity.DecorateStatusId = house.DecorateStatusId;
                houseEntity.Description = house.Description;
                houseEntity.Direction = house.Direction;
                houseEntity.FloorIndex = house.FloorIndex;
                //houseEntity.HousePics 新增后再单独添加
                houseEntity.LookableDateTime = house.LookableDateTime;
                houseEntity.MonthRent = house.MonthRent;
                houseEntity.OwnerName = house.OwnerName;
                houseEntity.OwnerPhoneNum = house.OwnerPhoneNum;
                houseEntity.RoomTypeId = house.RoomTypeId;
                houseEntity.StatusId = house.StatusId;
                houseEntity.TotalFloorCount = house.TotalFloorCount;
                houseEntity.TypeId = house.TypeId;
                ctx.Houses.Add(houseEntity);
                ctx.SaveChanges();
                return houseEntity.Id;
            }
        }
        /// <summary>
        /// 为一个房屋添加照片
        /// </summary>
        /// <param name="housePic"></param>
        /// <returns></returns>
        public long AddNewHousePic(HousePicDTO housePic)
        {
            HousePicEntity entity = new HousePicEntity();
            entity.HouseId = housePic.HouseId;
            entity.ThumbUrl = housePic.ThumbUrl;
            entity.Url = housePic.Url;
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                ctx.HousePics.Add(entity);
                ctx.SaveChanges();
                return entity.Id;
            }
        }
        /// <summary>
        /// 删除房屋照片
        /// </summary>
        /// <param name="housePicId"></param>
        public void DeleteHousePic(long housePicId)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                //复习EF状态转换
                /*
                HousePicEntity entity = new HousePicEntity();
                entity.Id = housePicId;
                ctx.Entry(entity).State = EntityState.Deleted;
                ctx.SaveChanges();*/
                var entity = ctx.HousePics
                    .SingleOrDefault(p => p.IsDeleted == false && p.Id == housePicId);
                if (entity != null)
                {
                    ctx.HousePics.Remove(entity);
                    ctx.SaveChanges();
                }
            }
        }

        private HouseDTO ToDTO(HouseEntity entity)
        {
            HouseDTO dto = new HouseDTO();
            dto.Address = entity.Address;
            dto.Area = entity.Area;
            dto.AttachmentIds = entity.Attachments.Select(a => a.Id).ToArray();
            dto.CheckInDateTime = entity.CheckInDateTime;
            dto.CityId = entity.Community.Region.CityId;
            dto.CityName = entity.Community.Region.City.Name;
            dto.CommunityBuiltYear = entity.Community.BuiltYear;
            dto.CommunityId = entity.CommunityId;
            dto.CommunityLocation = entity.Community.Location;
            dto.CommunityName = entity.Community.Name;
            dto.CommunityTraffic = entity.Community.Traffic;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.DecorateStatusId = entity.DecorateStatusId;
            dto.DecorateStatusName = entity.DecorateStatus.Name;
            dto.Description = entity.Description;
            dto.Direction = entity.Direction;
            var firstPic = entity.HousePics.FirstOrDefault();
            if (firstPic != null)
            {
                dto.FirstThumbUrl = firstPic.ThumbUrl;
            }
            dto.FloorIndex = entity.FloorIndex;
            dto.Id = entity.Id;
            dto.LookableDateTime = entity.LookableDateTime;
            dto.MonthRent = entity.MonthRent;
            dto.OwnerName = entity.OwnerName;
            dto.OwnerPhoneNum = entity.OwnerPhoneNum;
            dto.RegionId = entity.Community.RegionId;
            dto.RegionName = entity.Community.Region.Name;
            dto.RoomTypeId = entity.RoomTypeId;
            dto.RoomTypeName = entity.RoomType.Name;
            dto.StatusId = entity.StatusId;
            dto.StatusName = entity.Status.Name;
            dto.TotalFloorCount = entity.TotalFloorCount;
            dto.TypeId = entity.TypeId;
            dto.TypeName = entity.Type.Name;
            return dto;
        }

        /// <summary>
        /// 获取所有房屋信息
        /// </summary>
        /// <returns></returns>
        public HouseDTO[] GetAll()
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> houseBS = new BaseService<HouseEntity>(ctx);
                var houses = houseBS.GetAll()
                    .Include(h => h.Attachments).Include(h => h.Community)
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                        + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type);
                return houses.ToList().Select(h => ToDTO(h)).ToArray();
            }
        }

        /// <summary>
        /// 根据id获取房屋信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HouseDTO GetById(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> houseBS = new BaseService<HouseEntity>(ctx);
                var house = houseBS.GetAll()
                    .Include(h => h.Attachments).Include(h => h.Community)
                    //Include("Community.Region.City");
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                        + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type)
                    .SingleOrDefault(h => h.Id == id);
                //.Where(h => h.Id == id).SingleOrDefault();
                if (house == null)
                {
                    return null;
                }
                return ToDTO(house);
            }
        }
        /// <summary>
        /// 获取在一个城市下创建时间在指定时间范围内的房屋的数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> houseBS = new BaseService<HouseEntity>(ctx);
                return houseBS.GetAll()
                    .LongCount(h => h.Community.Region.CityId == cityId
                    && h.CreateDateTime >= startDateTime && h.CreateDateTime <= endDateTime);
            }
        }
        /// <summary>
        /// 分页获取一个城市下某一个类型的房屋信息列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="typeId"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public HouseDTO[] GetPagedData(long cityId, long typeId, int pageSize, int currentIndex)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> houseBS = new BaseService<HouseEntity>(ctx);
                var houses = houseBS.GetAll()
                    .Include(h => h.Attachments).Include(h => h.Community)
                    /*
                    .Include(h => nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                        + "." + nameof(RegionEntity.City))
                    .Include(h => nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))*/
                    //.Include("Community.Region.City")
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                        + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type)
                    //注意Where的位置，要放到Skip之前
                    .Where(h => h.Community.Region.CityId == cityId && h.TypeId == typeId)
                    .OrderByDescending(h => h.CreateDateTime)
                    .Skip(currentIndex).Take(pageSize);
                //.Where(h => h.Community.Region.CityId == cityId && h.TypeId == typeId);
                return houses.ToList().Select(h => ToDTO(h)).ToArray();
            }
        }
        /// <summary>
        /// 获取一个房屋的所有照片
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public HousePicDTO[] GetPics(long houseId)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                /*
                BaseService<HousePicEntity> bs = new BaseService<HousePicEntity>(ctx);
                return bs.GetAll().AsNoTracking().Where(p => p.HouseId == houseId)
                    .Select(p => new HousePicDTO
                    {
                        CreateDateTime = p.CreateDateTime,
                        HouseId = p.HouseId,
                        Id = p.Id,
                        ThumbUrl = p.ThumbUrl,
                        Url = p.Url
                    })
                    .ToArray();*/
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                return bs.GetById(houseId).HousePics.Select(p => new HousePicDTO
                {
                    CreateDateTime = p.CreateDateTime,
                    HouseId = p.HouseId,
                    Id = p.Id,
                    ThumbUrl = p.ThumbUrl,
                    Url = p.Url
                }).ToArray();
            }
        }

        public int GetTodayNewHouseCount(long cityId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取一个城市下,某一个类型的房屋的数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public long GetTotalCount(long cityId, long typeId)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                return bs.GetAll()
                    .LongCount(h => h.Community.Region.CityId == cityId && h.TypeId == typeId);
            }
        }
        /// <summary>
        /// 软删除一个房屋
        /// </summary>
        /// <param name="id"></param>
        public void MarkDeleted(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                bs.MarkDeleted(id);
            }
        }

        public HouseSearchResult Search(HouseSearchOptions options)
        {
            /*
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                var items = bs.GetAll().Where(h => h.Address.Contains("楼"));
                long c = items.LongCount();
                items.Take(10).ToList();
            }*/

            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                var items = bs.GetAll().Where(h => h.Community.Region.CityId == options.CityId
                            && h.TypeId == options.TypeId);
                if (options.RegionId != null)
                {
                    items = items.Where(t => t.Community.RegionId == options.RegionId);
                }
                if (options.StartMonthRent != null)
                {
                    items = items.Where(t => t.MonthRent >= options.StartMonthRent);
                }
                if (options.EndMonthRent != null)
                {
                    items = items.Where(t => t.MonthRent <= options.EndMonthRent);
                }
                if (options.EndMonthRent != null)
                {
                    items = items.Where(t => t.MonthRent <= options.EndMonthRent);
                }
                if (!string.IsNullOrEmpty(options.Keywords))
                {
                    items = items.Where(t => t.Address.Contains(options.Keywords)
                            || t.Description.Contains(options.Keywords)
                            || t.Community.Name.Contains(options.Keywords)
                            || t.Community.Location.Contains(options.Keywords)
                            || t.Community.Traffic.Contains(options.Keywords));
                }
                long totalCount = items.LongCount();//总搜索结果条数

                items = items.Include(h => h.Attachments).Include(h => h.Community)
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)
                        + "." + nameof(RegionEntity.City))
                    .Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .Include(h => h.DecorateStatus)
                    .Include(h => h.HousePics)
                    .Include(h => h.RoomType)
                    .Include(h => h.Status)
                    .Include(h => h.Type).Include(h => h.Attachments);

                switch (options.OrderByType)
                {
                    case HouseSearchOrderByType.AreaAsc:
                        items = items.OrderBy(t => t.Area);
                        break;
                    case HouseSearchOrderByType.AreaDesc:
                        items = items.OrderByDescending(t => t.Area);
                        break;
                    case HouseSearchOrderByType.CreateDateDesc:
                        items = items.OrderByDescending(t => t.CreateDateTime);
                        break;
                    case HouseSearchOrderByType.MonthRentAsc:
                        items = items.OrderBy(t => t.MonthRent);
                        break;
                    case HouseSearchOrderByType.MonthRentDesc:
                        items = items.OrderByDescending(t => t.MonthRent);
                        break;
                }
                //一定不要items.Where
                //而要items=items.Where();
                //OrderBy要在Skip和Take之前
                //给用户看的页码从1开始，程序中是从0开始
                items = items.Skip((options.CurrentIndex - 1) * options.PageSize)
                    .Take(options.PageSize);
                HouseSearchResult searchResult = new HouseSearchResult();
                searchResult.totalCount = totalCount;
                List<HouseDTO> houses = new List<HouseDTO>();
                foreach (var item in items)
                {
                    houses.Add(ToDTO(item));
                }
                searchResult.result = houses.ToArray();
                return searchResult;
            }
        }
        /// <summary>
        /// 更新一个房屋
        /// </summary>
        /// <param name="house"></param>
        public void Update(HouseDTO house)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                HouseEntity entity = bs.GetById(house.Id);
                entity.Address = house.Address;
                entity.Area = house.Area;
                //2,3,4
                entity.Attachments.Clear();//先删再加
                var atts = ctx.Attachments.Where(a => a.IsDeleted == false &&
                    house.AttachmentIds.Contains(a.Id));
                foreach (AttachmentEntity att in atts)
                {
                    entity.Attachments.Add(att);
                }
                //3,4,5
                entity.CheckInDateTime = house.CheckInDateTime;
                entity.CommunityId = house.CommunityId;
                entity.DecorateStatusId = house.DecorateStatusId;
                entity.Description = house.Description;
                entity.Direction = house.Direction;
                entity.FloorIndex = house.FloorIndex;
                entity.LookableDateTime = house.LookableDateTime;
                entity.MonthRent = house.MonthRent;
                entity.OwnerName = house.OwnerName;
                entity.OwnerPhoneNum = house.OwnerPhoneNum;
                entity.RoomTypeId = house.RoomTypeId;
                entity.StatusId = house.StatusId;
                entity.TotalFloorCount = house.TotalFloorCount;
                entity.TypeId = house.TypeId;
                ctx.SaveChanges();
            }
        }
    }
}
