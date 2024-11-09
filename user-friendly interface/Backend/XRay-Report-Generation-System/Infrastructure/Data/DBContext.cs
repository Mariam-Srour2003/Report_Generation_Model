using Domain.Models;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RolePermissionConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HolidayConfiguration).Assembly);


            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Doctor" },
                new Role { Id = 3, Name = "Patient" }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "ManageUsers" },
                new Permission { Id = 2, Name = "ManageRoles" },
                new Permission { Id = 3, Name = "ViewAppointments" },
                new Permission { Id = 4, Name = "ViewPatientHistory" },
                new Permission { Id = 5, Name = "BookAppointments" },
                new Permission { Id = 6, Name = "Signup" },
                new Permission { Id = 7, Name = "Login" },
                new Permission { Id = 8, Name = "UploadImage" },
                new Permission { Id = 9, Name = "ManageReports"},
                new Permission { Id = 10, Name = "ManageXRayImages"}

            );

            modelBuilder.Entity<RolesPermissions>().HasData(
                new RolesPermissions { Id = 1, RoleId = 1, PermissionId = 1 },
                new RolesPermissions { Id = 2, RoleId = 1, PermissionId = 2 },
                new RolesPermissions { Id = 3, RoleId = 1, PermissionId = 7 },
                new RolesPermissions { Id = 4, RoleId = 2, PermissionId = 3 },
                new RolesPermissions { Id = 5, RoleId = 2, PermissionId = 4 },
                new RolesPermissions { Id = 6, RoleId = 2, PermissionId = 7 },
                new RolesPermissions { Id = 7, RoleId = 3, PermissionId = 5 },
                new RolesPermissions { Id = 8, RoleId = 3, PermissionId = 6 },
                new RolesPermissions { Id = 9, RoleId = 3, PermissionId = 7 },
                new RolesPermissions { Id = 10, RoleId = 3, PermissionId = 8 },
                new RolesPermissions { Id = 11, RoleId = 2, PermissionId = 9 },
                new RolesPermissions { Id = 12, RoleId = 2, PermissionId = 10 }
             );

            modelBuilder.Entity<Holiday>().HasData(
                new Holiday { Id = 1, Date = new DateTime(2024, 12, 25), Description = "Christmas Day" },
                new Holiday { Id = 2, Date = new DateTime(2024, 1, 1), Description = "New Year's Day" },
                new Holiday { Id = 3, Date = new DateTime(2024, 5, 1), Description = "Labour Day" },
                new Holiday { Id = 4, Date = new DateTime(2024, 11, 28), Description = "Thanksgiving Day" },
                new Holiday { Id = 5, Date = new DateTime(2024, 11, 22), Description = "Independence Day" }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolesPermissions> RolesPermissions { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<XRayImage> XRayImages { get; set; }

    }
}
