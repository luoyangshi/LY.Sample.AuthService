using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LY.Sample.AuthService.Middleware
{
    public class AppExceptionHandlerOption
    {
        public AppExceptionHandlerOption(
            AppExceptionHandleType handleType = AppExceptionHandleType.JsonHandle,
            IList<PathString> jsonHandleUrlKeys = null,
            string errorHandingPath = "")
        {
            HandleType = handleType;
            JsonHandleUrlKeys = jsonHandleUrlKeys;
            ErrorHandingPath = errorHandingPath;
        }

        /// <summary>
        /// 异常处理方式
        /// </summary>
        public AppExceptionHandleType HandleType { get; set; }

        /// <summary>
        /// Json处理方式的Url关键字
        /// <para>仅HandleType=Both时生效</para>
        /// </summary>
        public IList<PathString> JsonHandleUrlKeys { get; set; }

        /// <summary>
        /// 错误跳转页面
        /// </summary>
        public PathString ErrorHandingPath { get; set; }
    }

    /// <summary>
    /// 错误处理方式
    /// </summary>
    public enum AppExceptionHandleType
    {
        JsonHandle = 0,   //Json形式处理
        PageHandle = 1,   //跳转网页处理
        Both = 2          //根据Url关键字自动处理
    }

    public class ApiResponse
    {
        public int State => IsSuccess ? 1 : 0;
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}


