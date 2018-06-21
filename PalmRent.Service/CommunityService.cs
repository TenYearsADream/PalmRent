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
    public class CommunityService : ICommunityService
    {
        /// <summary>
        /// 获取一个地区下的所有小区
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public CommunityDTO[] GetByRegionId(long regionId)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<CommunityEntity> bs
                    = new BaseService<CommunityEntity>(ctx);
                var cities = bs.GetAll().AsNoTracking()
                    .Where(c => c.RegionId == regionId);
                return cities.Select(c => new CommunityDTO
                {
                    BuiltYear = c.BuiltYear,
                    CreateDateTime = c.CreateDateTime,
                    Id = c.Id,
                    Location = c.Location,
                    Name = c.Name,
                    RegionId = c.RegionId,
                    Traffic = c.Traffic
                }).ToArray();
            }
        }
    }
}
