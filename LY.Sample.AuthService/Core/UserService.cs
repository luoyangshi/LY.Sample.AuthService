using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Sample.AuthService.Data.Models;
using LY.Sample.AuthService.Data.ViewModels;
using LY.Sample.AuthService.Exceptions;
using SM.UnitOfWork;

namespace LY.Sample.AuthService.Core
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Test()
        {
           throw new Exception("test");
        }

        public async Task AddUser(AddUserInputDto input)
        {
            var userRepos = _unitOfWork.GetRepository<User>();
            var user = userRepos.GetFirstOrDefault(predicate: p => p.UserName == input.UserName);
            if (user != null) throw new Exception("用户名已存在");
            await userRepos.InsertAsync(new User()
            {
                Id = Guid.NewGuid(),
                UserName = input.UserName,
                Password = input.Password,
                IsDisable = false,
            });
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
