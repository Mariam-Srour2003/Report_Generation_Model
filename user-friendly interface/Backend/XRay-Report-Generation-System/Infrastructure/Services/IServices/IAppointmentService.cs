using Domain.Models;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.IServices
{
    public interface IAppointmentService
    {
        Task<BaseResponseDTO<IEnumerable<AppointmentDto>>> GetAppointments();
        Task<BaseResponseDTO<AppointmentDto>> AddAppointment(AppointmentDto appointmentDTO);
        Task<BaseResponseDTO<AppointmentDto>> UpdateAppointment(AppointmentDto appointmentDTO);
        Task<BaseResponseDTO<object>> DeleteAppointment(long id);
        Task<BaseResponseDTO<AppointmentDto>> GetAppointmentById(long id);
        Task<BaseResponseDTO<IEnumerable<AppointmentDto>>> GetAppointmentsByPatientId(long patientId);
        Task<BaseResponseDTO<IEnumerable<AppointmentDto>>> GetAppointmentsByDoctorId(long doctorId);
    }   
}