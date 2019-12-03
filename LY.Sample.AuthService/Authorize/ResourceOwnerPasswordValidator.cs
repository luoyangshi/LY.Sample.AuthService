using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace LY.Sample.AuthService.Authorize
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserStore _userStore;

        public ResourceOwnerPasswordValidator(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userId = _userStore.CheckPassword(context.UserName, context.Password);
            if (!string.IsNullOrEmpty(userId))
            {
                context.Result = new GrantValidationResult(userId, "pwd");
                return Task.CompletedTask;
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "用户名或密码错误");
            return Task.CompletedTask;
        }
    }
}
