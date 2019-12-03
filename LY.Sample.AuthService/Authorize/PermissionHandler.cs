using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace LY.Sample.AuthService.Authorize
{
    public class PermissionHandler:AuthorizationHandler<PermissionAuthorizationRequirement>,IAuthorizationHandler
    {
        private readonly IUserStore _userStore;

        public PermissionHandler(IUserStore userStore)
        {
            _userStore = userStore;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = context.User.Claims.FirstOrDefault(p => p.Type == JwtClaimTypes.Subject)?.Value;
            if (userId != null && _userStore.CheckPermission(userId, requirement.Name))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
