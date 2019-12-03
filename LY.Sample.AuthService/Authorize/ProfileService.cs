using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace LY.Sample.AuthService.Authorize
{
    public class ProfileService : IProfileService
    {
        private readonly IUserStore _userStore;

        public ProfileService(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //var sub = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            //var user = _userService.FindById(sub);
            //context.IssuedClaims = new List<Claim>()
            //{
            //    new Claim(JwtClaimTypes.PreferredUserName,user.UserName)
            //};
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //var sub = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            //var user = _userService.FindById(sub);
            //if (user != null && user.IsEnable)
            //{
            //    context.IsActive = true;
            //}
            //else
            //{
            //    context.IsActive = false;
            //}
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
