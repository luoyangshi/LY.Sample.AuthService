using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Sample.AuthService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LY.Sample.AuthService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
    }
}
