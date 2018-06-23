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
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionService : IPermissionService
    {
        /// <summary>
        /// 为一个角色添加权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permIds"></param>
        public void AddPermIds(long roleId, long[] permIds)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<RoleEntity> roleBS
                    = new BaseService<RoleEntity>(ctx);
                var role = roleBS.GetById(roleId);
                if (role == null)
                {
                    throw new ArgumentException("roleId不存在" + roleId);
                }
                BaseService<PermissionEntity> permBS
                    = new BaseService<PermissionEntity>(ctx);
                var perms = permBS.GetAll()
                    .Where(p => permIds.Contains(p.Id)).ToArray();
                foreach (var perm in perms)
                {
                    role.Permissions.Add(perm);
                }
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// 添加一个权限
        /// </summary>
        /// <param name="permName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public long AddPermission(string permName, string description)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<PermissionEntity> permBS = new BaseService<PermissionEntity>(ctx);
                bool exists = permBS.GetAll().Any(p => p.Name == permName);
                if (exists)
                {
                    throw new ArgumentException("权限项已经存在");
                }
                PermissionEntity perm = new PermissionEntity();
                perm.Description = description;
                perm.Name = permName;
                ctx.Permissions.Add(perm);
                ctx.SaveChanges();
                return perm.Id;
            }
        }

        private PermissionDTO ToDTO(PermissionEntity p)
        {
            PermissionDTO dto = new PermissionDTO();
            dto.CreateDateTime = p.CreateDateTime;
            dto.Description = p.Description;
            dto.Id = p.Id;
            dto.Name = p.Name;
            return dto;
        }
        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <returns></returns>
        public PermissionDTO[] GetAll()
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                return bs.GetAll().ToList().Select(p => ToDTO(p)).ToArray();
            }
        }
        /// <summary>
        /// 根据id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PermissionDTO GetById(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                var pe = bs.GetById(id);
                return pe == null ? null : ToDTO(pe);
            }
        }
        /// <summary>
        /// 根据name获取权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PermissionDTO GetByName(string name)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                var pe = bs.GetAll().SingleOrDefault(p => p.Name == name);
                return pe == null ? null : ToDTO(pe);
            }
        }
        /// <summary>
        /// 根据角色获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public PermissionDTO[] GetByRoleId(long roleId)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<RoleEntity> bs = new BaseService<RoleEntity>(ctx);
                return bs.GetById(roleId).Permissions.ToList().Select(p => ToDTO(p)).ToArray();
            }
        }
        /// <summary>
        /// 软删除一个权限
        /// </summary>
        /// <param name="id"></param>
        public void MarkDeleted(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                bs.MarkDeleted(id);
            }
        }
        /// <summary>
        /// 更新一个角色的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permIds"></param>
        public void UpdatePermIds(long roleId, long[] permIds)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<RoleEntity> roleBS
                    = new BaseService<RoleEntity>(ctx);
                var role = roleBS.GetById(roleId);
                if (role == null)
                {
                    throw new ArgumentException("roleId不存在" + roleId);
                }
                role.Permissions.Clear();
                BaseService<PermissionEntity> permBS
                    = new BaseService<PermissionEntity>(ctx);
                var perms = permBS.GetAll()
                    .Where(p => permIds.Contains(p.Id)).ToList();
                foreach (var perm in perms)
                {
                    role.Permissions.Add(perm);
                }
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// 更新一个权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permName"></param>
        /// <param name="description"></param>
        public void UpdatePermission(long id, string permName, string description)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<PermissionEntity> bs = new BaseService<PermissionEntity>(ctx);
                var perm = bs.GetById(id);
                if (perm == null)
                {
                    throw new ArgumentException("id不存在" + id);
                }
                perm.Name = permName;
                perm.Description = description;
                ctx.SaveChanges();
            }
        }
    }
}
