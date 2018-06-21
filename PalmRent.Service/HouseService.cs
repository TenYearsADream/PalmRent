using PalmRent.DTO;
using PalmRent.IService;
using PalmRent.Service.Entities;
using System;
using System.Collections.Generic;
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

        public long AddNewHousePic(HousePicDTO housePic)
        {
            throw new NotImplementedException();
        }

        public void DeleteHousePic(long housePicId)
        {
            throw new NotImplementedException();
        }

        public HouseDTO[] GetAll()
        {
            throw new NotImplementedException();
        }

        public HouseDTO GetById(long id)
        {
            throw new NotImplementedException();
        }

        public long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime)
        {
            throw new NotImplementedException();
        }

        public HouseDTO[] GetPagedData(long cityId, long typeId, int pageSize, int currentIndex)
        {
            throw new NotImplementedException();
        }

        public HousePicDTO[] GetPics(long houseId)
        {
            throw new NotImplementedException();
        }

        public int GetTodayNewHouseCount(long cityId)
        {
            throw new NotImplementedException();
        }

        public long GetTotalCount(long cityId, long typeId)
        {
            throw new NotImplementedException();
        }

        public void MarkDeleted(long id)
        {
            throw new NotImplementedException();
        }

        public HouseSearchResult Search(HouseSearchOptions options)
        {
            throw new NotImplementedException();
        }

        public void Update(HouseDTO house)
        {
            throw new NotImplementedException();
        }
    }
}
