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
    /// <summary>
    /// 预约看房
    /// </summary>
    public class HouseAppointmentService : IHouseAppointmentService
    {
        /// <summary>
        /// 添加预约看房
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="phoneNum"></param>
        /// <param name="houseId"></param>
        /// <param name="visitDate"></param>
        /// <returns></returns>
        public long AddNew(long? userId, string name, string phoneNum, long houseId, DateTime visitDate)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                HouseAppointmentEntity houseApp = new HouseAppointmentEntity();
                houseApp.HouseId = houseId;
                houseApp.Name = name;
                houseApp.PhoneNum = phoneNum;
                houseApp.Status = "未处理";
                houseApp.UserId = userId;
                houseApp.VisitDate = visitDate;
                ctx.HouseAppointments.Add(houseApp);
                ctx.SaveChanges();
                return houseApp.Id;
            }
        }

        public bool Follow(long adminUserId, long houseAppointmentId)
        {
            throw new NotImplementedException();
        }

        private HouseAppointmentDTO ToDTO(HouseAppointmentEntity houseApp)
        {
            HouseAppointmentDTO dto = new HouseAppointmentDTO();
            dto.CommunityName = houseApp.House.Community.Name;
            dto.CreateDateTime = houseApp.CreateDateTime;
            dto.FollowAdminUserId = houseApp.FollowAdminUserId;
            if (houseApp.FollowAdminUser != null)
            {
                dto.FollowAdminUserName = houseApp.FollowAdminUser.Name;
            }
            dto.FollowDateTime = houseApp.FollowDateTime;
            dto.HouseId = houseApp.HouseId;
            dto.Id = houseApp.Id;
            dto.Name = houseApp.Name;
            dto.PhoneNum = houseApp.PhoneNum;
            dto.RegionName = houseApp.House.Community.Region.Name;
            dto.Status = houseApp.Status;
            dto.UserId = houseApp.UserId;
            dto.VisitDate = houseApp.VisitDate;
            dto.HouseAddress = houseApp.House.Address;
            return dto;
        }
        /// <summary>
        /// 通过id获取预约看房信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HouseAppointmentDTO GetById(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseAppointmentEntity> bs
                    = new BaseService<HouseAppointmentEntity>(ctx);
                var houseApp = bs.GetAll().Include(a => a.House)
                    //Include("House.Community")
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community))
                    .Include(a => a.FollowAdminUser)
                    //Include("House.Community.Region")
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .AsNoTracking().SingleOrDefault(a => a.Id == id);
                if (houseApp == null)
                {
                    return null;
                }
                return ToDTO(houseApp);
            }
        }
        /// <summary>
        /// 预约看房分页列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="status"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public HouseAppointmentDTO[] GetPagedData(long cityId, string status, int pageSize, int currentIndex)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseAppointmentEntity> bs
                    = new BaseService<HouseAppointmentEntity>(ctx);
                var apps = bs.GetAll().Include(a => a.House)
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community))
                    .Include(a => a.FollowAdminUser)
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .AsNoTracking()
                    .Where(a => a.House.Community.Region.CityId == cityId && a.Status == status)
                    .OrderByDescending(a => a.CreateDateTime)//Skip之前一定要调用OrderBy
                    .Skip(currentIndex).Take(pageSize);
                return apps.ToList().Select(a => ToDTO(a)).ToArray();
            }
        }
        /// <summary>
        /// 预约看房总条数
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public long GetTotalCount(long cityId, string status)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseAppointmentEntity> bs
                    = new BaseService<HouseAppointmentEntity>(ctx);
                var count = bs.GetAll()
                    //Where(a => a.House.Community.Region.CityId == cityId && a.Status == status).LongCount()
                    .LongCount(a => a.House.Community.Region.CityId == cityId && a.Status == status);
                return count;
            }
        }
    }
}
