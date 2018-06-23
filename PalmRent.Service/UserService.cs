using PalmRent.Common;
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
    public class UserService : IUserService
    {
        /// <summary>
        /// 添加一个前台用户
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public long AddNew(string phoneNum, string password)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                //检查手机号不能重复

                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                bool exists = bs.GetAll().Any(u => u.PhoneNum == phoneNum);
                if (exists)
                {
                    throw new ArgumentException("手机号已经存在");
                }
                UserEntity user = new UserEntity();
                user.PhoneNum = phoneNum;
                string salt = CommonHelper.CreateVerifyCode(5);
                string pwdHash = CommonHelper.CalcMD5(salt + password);
                user.PasswordHash = pwdHash;
                user.PasswordSalt = salt;
                ctx.Users.Add(user);
                ctx.SaveChanges();
                return user.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetAll().SingleOrDefault(u => u.PhoneNum == phoneNum);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    string dbPwdHash = user.PasswordHash;
                    string salt = user.PasswordSalt;
                    string userPwdHash = CommonHelper.CalcMD5(salt + password);
                    return dbPwdHash == userPwdHash;
                }
            }
        }
        private UserDTO ToDTO(UserEntity user)
        {
            UserDTO dto = new UserDTO();
            dto.CityId = user.CityId;
            dto.CreateDateTime = user.CreateDateTime;
            dto.Id = user.Id;
            dto.LastLoginErrorDateTime = user.LastLoginErrorDateTime;
            dto.LoginErrorTimes = user.LoginErrorTimes;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }

        public UserDTO GetById(long id)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(id);
                return user == null ? null : ToDTO(user);
            }
        }

        public UserDTO GetByPhoneNum(string phoneNum)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetAll().SingleOrDefault(u => u.PhoneNum == phoneNum);
                return user == null ? null : ToDTO(user);
            }
        }

        public void IncrLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public bool IsLocked(long id)
        {
            throw new NotImplementedException();
        }

        public void ResetLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void SetUserCityId(long userId, long cityId)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户id不存在" + userId);
                }
                user.CityId = cityId;
                ctx.SaveChanges();
            }
        }

        public void UpdatePwd(long userId, string newPassword)
        {
            using (PalmRentDbContext ctx = new PalmRentDbContext())
            {
                //检查手机号不能重复
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在 " + userId);
                }
                string salt = user.PasswordSalt;// CommonHelper.CreateVerifyCode(5);
                string pwdHash = CommonHelper.CalcMD5(salt + newPassword);
                user.PasswordHash = pwdHash;
                user.PasswordSalt = salt;
                ctx.SaveChanges();
            }
        }
    }
}
