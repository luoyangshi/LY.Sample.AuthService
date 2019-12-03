using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LY.Sample.AuthService.Authorize
{
    public class PermissionAuthorizationRequirement:IAuthorizationRequirement
    {
        public string Name { get; set; }

        public PermissionAuthorizationRequirement(string name)
        {
            Name = name;
        }
    }
}
