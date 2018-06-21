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
    /// 房屋配套设施
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        private AttachmentDTO ToDTO(AttachmentEntity att)
        {
            AttachmentDTO dto = new AttachmentDTO();
            dto.CreateDateTime = att.CreateDateTime;
            dto.IconName = att.IconName;
            dto.Id = att.Id;
            dto.Name = att.Name;
            return dto;
        }
        /// <summary>
        /// 获取所有配套设施
        /// </summary>
        /// <returns></returns>
        public AttachmentDTO[] GetAll()
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<AttachmentEntity> bs
                    = new BaseService<AttachmentEntity>(ctx);
                var items = bs.GetAll().AsNoTracking();
                return items.ToList().Select(a => ToDTO(a)).ToArray();
            }
        }
        /// <summary>
        /// 获取某一个房屋的配套设施
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public AttachmentDTO[] GetAttachments(long houseId)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<HouseEntity> houseBS
                    = new BaseService<HouseEntity>(ctx);
                var house = houseBS.GetAll().Include(a => a.Attachments)
                    .AsNoTracking().SingleOrDefault(h => h.Id == houseId);
                if (house == null)
                {
                    throw new ArgumentException("houseId" + houseId + "不存在");
                }
                return house.Attachments.ToList().Select(a => ToDTO(a)).ToArray();
            }
        }
    }
}
