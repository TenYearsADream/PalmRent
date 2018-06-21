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
    public class CityService : ICityService
    {
        /// <summary>
        /// 添加一个城市
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public long AddNew(string cityName)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<CityEntity> bs
                    = new BaseService<CityEntity>(ctx);
                //判断是否存在任何一条数据满足 c.Name == cityName
                //即存在这样一个名字的城市
                //如果只是判断“是否存在”，那么用Any效率比Where().count()效率高
                //Where(c => c.Name == cityName).Count()>0
                bool exists = bs.GetAll().Any(c => c.Name == cityName);
                if (exists)
                {
                    throw new ArgumentException("城市已经存在");
                }
                CityEntity city = new CityEntity();
                city.Name = cityName;
                ctx.Cities.Add(city);
                ctx.SaveChanges();
                return city.Id;
            }
        }
        private CityDTO ToDTO(CityEntity city)
        {
            CityDTO dto = new CityDTO();
            dto.CreateDateTime = city.CreateDateTime;
            dto.Id = city.Id;
            dto.Name = city.Name;
            return dto;
        }
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        public CityDTO[] GetAll()
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                return bs.GetAll().AsNoTracking()
                    .ToList().Select(c => ToDTO(c)).ToArray();
            }
        }
        /// <summary>
        /// 根据Id获取城市信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CityDTO GetById(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                var city = bs.GetById(id);
                if (city == null)
                {
                    return null;
                }
                return ToDTO(city);
            }
        }
    }
}
