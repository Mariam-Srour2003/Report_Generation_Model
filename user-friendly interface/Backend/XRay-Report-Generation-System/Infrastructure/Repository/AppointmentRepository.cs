using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DBContext context) : base(context)
        {
        }
        public async Task<bool> HasConflict(long doctorId, long patientId, DateTime appointmentDate)
        {
            return await _context.Set<Appointment>().AnyAsync(a =>
                (a.DoctorId == doctorId || a.PatientId == patientId) &&
                (a.AppointmentDate >= appointmentDate.AddMinutes(-30) && a.AppointmentDate <= appointmentDate.AddMinutes(30)));
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientId(long patientId)
        {
            return await _context.Set<Appointment>()
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorId(long doctorId)
        {
            return await _context.Set<Appointment>()
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

    }
}
