using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Sample.AuthService.Data.ViewModels
{
    public class AddUserInputDto
    {
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        [Required]
        [StringLength(20,MinimumLength = 8)]
        public string Password { get; set; }
    }
}
