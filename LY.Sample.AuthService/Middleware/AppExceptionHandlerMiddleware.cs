using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Sample.AuthService.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LY.Sample.AuthService.Middleware
{
    public class AppExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private AppExceptionHandlerOption _option = new AppExceptionHandlerOption();
        private readonly IDictionary<int, string> _exceptionStatusCodeDic;
        private readonly ILogger<AppExceptionHandlerMiddleware> _logger;
        public AppExceptionHandlerMiddleware(RequestDelegate next, Action<AppExceptionHandlerOption> actionOptions, ILogger<AppExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            actionOptions(_option);
            _exceptionStatusCodeDic = new Dictionary<int, string>
            {
                { 401, "未授权的请求" },
                { 404, "找不到该页面" },
                { 403, "访问被拒绝" },
                { 500, "服务器发生意外的错误" }
            };
        }

        public async Task Invoke(HttpContext context)
        {
            Exception exception = null;
            try
            {
                await _next(context); //调用管道执行下一个中间件
            }
            catch (AppException ex)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                var apiResponse = new ApiResponse(){IsSuccess = false,Message = ex.ErrorMsg};
                var serializerResult = JsonConvert.SerializeObject(apiResponse);
                context.Response.ContentType = "application/json;charset=utf-8";
                await context.Response.WriteAsync(serializerResult);
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError; //发生未捕获的异常，手动设置状态码
                exception = ex;
            }
            finally
            {
                if (_exceptionStatusCodeDic.ContainsKey(context.Response.StatusCode) && !context.Items.ContainsKey("ExceptionHandled")) //预处理标记
                {
                    string errorMsg;
                    if (context.Response.StatusCode == 500 && exception != null)
                    {
                        errorMsg = $"{(exception.InnerException != null ? exception.InnerException.Message : exception.Message)}";
                        _logger.LogError(errorMsg);
                    }
                    else
                    {
                        errorMsg = _exceptionStatusCodeDic[context.Response.StatusCode];
                    }
                    exception = new Exception(errorMsg);
                }
                if (exception != null)
                {
                    var handleType = _option.HandleType;
                    if (handleType == AppExceptionHandleType.Both) //根据Url关键字决定异常处理方式
                    {
                        var requestPath = context.Request.Path;
                        handleType = _option.JsonHandleUrlKeys != null && _option.JsonHandleUrlKeys.Count(
                                         k => requestPath.StartsWithSegments(k, StringComparison.CurrentCultureIgnoreCase)) > 0
                            ? AppExceptionHandleType.JsonHandle
                            : AppExceptionHandleType.PageHandle;
                    }
                    if (handleType == AppExceptionHandleType.JsonHandle)
                        await JsonHandle(context, exception);
                    else
                        await PageHandle(context, exception, _option.ErrorHandingPath);
                }
            }
        }

        /// <summary>
        /// 统一格式响应类
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private ApiResponse GetApiResponse(Exception ex)
        {
            return new ApiResponse() { IsSuccess = false, Message = ex.Message };
        }

        /// <summary>
        /// 处理方式：返回Json格式
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task JsonHandle(HttpContext context, System.Exception ex)
        {
            var apiResponse = GetApiResponse(ex);
            var serializerResult = JsonConvert.SerializeObject(apiResponse);
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(serializerResult);
        }

        /// <summary>
        /// 处理方式：跳转网页
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task PageHandle(HttpContext context, System.Exception ex, PathString path)
        {
            context.Items.Add("Exception", ex);
            var originPath = context.Request.Path;
            context.Request.Path = path;   //设置请求页面为错误跳转页面
            try
            {
                await _next(context);
            }
            catch
            {

            }
            finally
            {
                context.Request.Path = originPath;   //恢复原始请求页面
            }
        }
    }
}
