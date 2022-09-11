using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LeaveScheduler.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LeaveScheduler.Data
{
    public class LeaveSchedulerContext : IdentityDbContext<IdentityUser>
    {
        public LeaveSchedulerContext (DbContextOptions<LeaveSchedulerContext> options)
            : base(options)
        {
        }

        public DbSet<LeaveScheduler.Models.Employee> Employee { get; set; } = default!;

        public DbSet<LeaveScheduler.Models.Request> Request { get; set; }

        public DbSet<LeaveScheduler.Models.Manager> Manager { get; set; }

        public DbSet<LeaveScheduler.Models.EmployeeManager> EmployeeManager { get; set; }

        public DbSet<LeaveScheduler.Models.User> User { get; set; }
    }
}
