using Infrastructure.Repository.IRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;

        public IUserRepository Users { get; }
        public IPermissionRepository Permissions { get; }
        public IRoleRepository Roles { get; }
        public IAppointmentRepository Appointments { get; }
        public IHolidayRepository Holidays { get; }
        public IReportRepository Reports { get; }
        public IXRayImageRepository XRayImages { get; }

        public UnitOfWork(DBContext dbContext,
            IUserRepository usersRepository,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IAppointmentRepository appointmentRepository,
            IHolidayRepository holidayRepository,
            IReportRepository reports,
            IXRayImageRepository xRayImages)
        
        {
            _context = dbContext;
            Users = usersRepository;
            Permissions = permissionRepository;
            Roles = roleRepository;
            Appointments = appointmentRepository;
            Holidays = holidayRepository;
            Reports = reports;
            XRayImages = xRayImages;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}