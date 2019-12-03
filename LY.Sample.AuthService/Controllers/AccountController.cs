using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Sample.AuthService.Core;
using LY.Sample.AuthService.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Annotations;

namespace LY.Sample.AuthService.Controllers
{
    [AllowAnonymous]
    [OpenApiTag("账号管理",Description = "注册修改账号信息")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <returns></returns>
        [HttpPost("Register")]
        public string Register()
        {
            return _userService.Test();
        }

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task Add([FromBody]AddUserInputDto input)
        {
            await _userService.AddUser(input);
        }
    }
}