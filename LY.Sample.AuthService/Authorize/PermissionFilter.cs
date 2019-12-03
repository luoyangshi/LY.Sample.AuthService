using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LY.Sample.AuthService.Authorize
{
    public class PermissionFilter:Attribute,IAsyncAuthorizationFilter
    {
        public string Name { get; set; }

        public PermissionFilter(string name)
        {
            Name = name;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var result = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new PermissionAuthorizationRequirement(Name));
            if (!result.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
