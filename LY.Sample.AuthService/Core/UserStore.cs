using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Sample.AuthService.Authorize;
using LY.Sample.AuthService.Data.Models;
using Newtonsoft.Json;
using SM.UnitOfWork;

namespace LY.Sample.AuthService.Core
{
    public class UserStore : IUserStore
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckPermission(string userId, string permission)
        {
            var permissionStr = RedisHelper.HGet("Permissions", userId);
            if (permissionStr == null) return false;
            var permissions = JsonConvert.DeserializeObject<List<string>>(permissionStr);
            return permissions.Any(p => p == permission);
        }

        public string CheckPassword(string username, string password)
        {
            var userRepos = _unitOfWork.GetRepository<User>();
            return userRepos.GetFirstOrDefault(predicate: p => p.UserName == username && p.Password == password)?.Id.ToString();
        }

        public bool CheckVerificationCode(string phoneoremail, string verificationCode)
        {
            var code = RedisHelper.Get(phoneoremail);
            return code != null && code == verificationCode;
        }

        public string CheckPhoneOrEmailAdd(string phoneoreamil)
        {
            var userRepos = _unitOfWork.GetRepository<User>();
            var user = userRepos.GetFirstOrDefault(predicate: p => p.Email == phoneoreamil || p.Phone == phoneoreamil);
            if (user == null)
            {
                var id = Guid.NewGuid();
                userRepos.Insert(new User()
                {
                    Id = id,
                    Email = phoneoreamil.Contains("@") ? phoneoreamil : null,
                    Phone = phoneoreamil.Contains("@") ? null : phoneoreamil,
                    IsDisable = false,
                    NickName = phoneoreamil,
                    UserName = phoneoreamil,
                    Password = phoneoreamil + "123123",
                });
                _unitOfWork.SaveChanges();
                return id.ToString();
            }
            return user.Id.ToString();
        }
    }
}
