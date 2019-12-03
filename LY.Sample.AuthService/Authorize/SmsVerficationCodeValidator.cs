using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace LY.Sample.AuthService.Authorize
{
    public class SmsVerficationCodeValidator : IExtensionGrantValidator
    {
        private readonly IUserStore _userStore;

        public SmsVerficationCodeValidator( IUserStore userStore)
        {
            _userStore = userStore;
        }

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            if (_userStore.CheckVerificationCode(phone, code))
            {
                var userId = _userStore.CheckPhoneOrEmailAdd(phone);
                if (!string.IsNullOrEmpty(userId))
                {
                    context.Result = new GrantValidationResult(userId, "sms");
                    return Task.CompletedTask;
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "添加用户失败");
                    return Task.CompletedTask;
                }
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "验证码不正确");
            return Task.CompletedTask;

        }

        public string GrantType => "sms";
    }
}
