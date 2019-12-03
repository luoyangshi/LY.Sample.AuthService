using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Sample.AuthService.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(20)]
        public string NickName { get; set; }

        [StringLength(11)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsDisable { get; set; }
    }
}
