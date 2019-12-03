using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Sample.AuthService.Exceptions
{
    public class AppException:Exception
    {
        public string ErrorMsg { get; set; }
        public AppException(string errorMsg)
        {
            ErrorMsg = errorMsg;
        }
    }
}
