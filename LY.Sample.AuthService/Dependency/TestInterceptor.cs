using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace LY.Sample.AuthService.Dependency
{
    public class TestInterceptor: IInterceptor
    {
        private readonly ILogger<TestInterceptor> _logger;

        public TestInterceptor(ILogger<TestInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
                invocation.Method.Name,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
            }
            Console.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
        }
    }
}
