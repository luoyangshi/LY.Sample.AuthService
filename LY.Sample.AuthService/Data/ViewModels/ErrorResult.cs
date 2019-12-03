using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Sample.AuthService.Data.ViewModels
{
    public class ErrorResult
    {
        /// <summary>
        /// 参数领域
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
    }
}
