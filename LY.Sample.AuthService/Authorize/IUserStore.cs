using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Sample.AuthService.Authorize
{
    public interface IUserStore
    {
        /// <summary>
        /// 检测用户是否拥有权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permission">权限项</param>
        /// <returns></returns>
        bool CheckPermission(string userId, string permission);

        /// <summary>
        /// 检测用户名密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>用户ID</returns>
        string CheckPassword(string username, string password);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="phoneoremail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckVerificationCode(string phoneoremail, string code);

        /// <summary>
        /// 验证手机或邮箱是否存在，不存在创建用户
        /// </summary>
        /// <param name="phoneoreamil"></param>
        /// <returns></returns>
        string CheckPhoneOrEmailAdd(string phoneoreamil);

    }
}
