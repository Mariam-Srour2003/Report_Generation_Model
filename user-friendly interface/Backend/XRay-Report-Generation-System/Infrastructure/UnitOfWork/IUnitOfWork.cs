using Infrastructure.Repository.IRepository;
using System;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IAppointmentRepository Appointments { get; }
        IHolidayRepository Holidays { get; }
        IReportRepository Reports { get; }
        IXRayImageRepository XRayImages { get; }

        int Save();
    }
}