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
    public class AdminLogService : IAdminLogService
    {
        public long AddNew(long adminUserId, string message)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                AdminLogEntity log = new AdminLogEntity() { AdminUserId = adminUserId, Message = message };
                ctx.AdminUserLogs
                    .Add(log);
                ctx.SaveChanges();
                return log.Id;
            }
        }

        public PalmRent.DTO.AdminLogDTO GetById(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<AdminLogEntity> bs = new BaseService<AdminLogEntity>(ctx);
                var log = bs.GetById(id);
                if (log == null)
                {
                    return null;
                }
                AdminLogDTO dto = new AdminLogDTO();
                dto.AdminUserId = log.AdminUserId;
                dto.AdminUserName = log.AdminUser.Name;
                dto.AdminUserPhoneNum = log.AdminUser.PhoneNum;
                dto.CreateDateTime = log.CreateDateTime;
                dto.Id = log.Id;
                dto.Message = log.Message;
                return dto;
            }
        }
    }
}
