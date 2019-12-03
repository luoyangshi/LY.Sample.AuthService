using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Sample.AuthService.Data.Models
{
    public class Permission
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
    }
}
