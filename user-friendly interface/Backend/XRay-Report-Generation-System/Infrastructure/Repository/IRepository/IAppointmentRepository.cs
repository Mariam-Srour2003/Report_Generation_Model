using Domain.Models;
using Infrastructure.Repository.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.IRepository
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<bool> HasConflict(long doctorId, long patientId, DateTime appointmentDate);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(long patientId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(long doctorId);

    }
}
