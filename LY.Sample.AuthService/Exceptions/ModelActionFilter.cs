using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Sample.AuthService.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LY.Sample.AuthService.Exceptions
{
    public class ModelActionFilter:ActionFilterAttribute,IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResults=new List<ErrorResult>();
                foreach (var item in context.ModelState)
                {
                    var result=new ErrorResult()
                    {
                        Field = item.Key,
                    };
                    foreach (var error in item.Value.Errors)
                    {
                        if (!string.IsNullOrEmpty(result.Message))
                        {
                            result.Message += "|";
                        }

                        result.Message += error.ErrorMessage;
                    }
                    errorResults.Add(result);
                }
                context.Result=new BadRequestObjectResult(new
                {
                    Code=StatusCodes.Status400BadRequest,
                    Errors=errorResults
                });
            }
        }
    }
}
