using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using LY.Sample.AuthService.Data.ViewModels;
using LY.Sample.AuthService.Dependency;

namespace LY.Sample.AuthService.Core
{
    //[Intercept(typeof(TestInterceptor))]
    public interface IUserService
    {
        string Test();
        Task AddUser(AddUserInputDto input);
    }
}
