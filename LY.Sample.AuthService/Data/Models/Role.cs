using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Sample.AuthService.Data.Models
{
    public class Role
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

    }
}
