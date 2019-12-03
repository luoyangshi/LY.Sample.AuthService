using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace LY.Sample.AuthService.Middleware
{
    public static class AppExceptionHandlerExtensions
    {
        public static IApplicationBuilder UserAppExceptionHandler(this IApplicationBuilder app, Action<AppExceptionHandlerOption> options)
        {
            return app.UseMiddleware<AppExceptionHandlerMiddleware>(options);
        }

    }
}
