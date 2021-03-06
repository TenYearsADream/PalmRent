﻿using PalmRent.DTO;
using PalmRent.IService;
using PalmRent.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmRent.Service
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class IdNameService : IIdNameService
    {
        public long AddNew(string typeName, string name)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                IdNameEntity idName =
                    new IdNameEntity { Name = name, TypeName = typeName };

                //todo:检查重复性
                ctx.IdNames.Add(idName);
                ctx.SaveChanges();
                return idName.Id;
            }
        }

        private IdNameDTO ToDTO(IdNameEntity entity)
        {
            IdNameDTO dto = new IdNameDTO();
            dto.CreateDateTime = entity.CreateDateTime;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.TypeName = entity.TypeName;
            return dto;
        }

        public IdNameDTO[] GetAll(string typeName)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<IdNameEntity> bs
                    = new BaseService<IdNameEntity>(ctx);
                return bs.GetAll().Where(e => e.TypeName == typeName)
                    .ToList().Select(e => ToDTO(e)).ToArray();
            }
        }

        public IdNameDTO GetById(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<IdNameEntity> bs
                    = new BaseService<IdNameEntity>(ctx);
                return ToDTO(bs.GetById(id));
            }
        }
    }
}
