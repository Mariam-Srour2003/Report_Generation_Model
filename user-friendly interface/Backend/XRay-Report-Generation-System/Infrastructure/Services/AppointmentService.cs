using AutoMapper;
using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Enum;
using Infrastructure.Repository;
using Infrastructure.Repository.IBase;
using Infrastructure.Repository.IRepository;
using Infrastructure.Services.IServices;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<IEnumerable<AppointmentDto>>> GetAppointments()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAll();
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                return new BaseResponseDTO<IEnumerable<AppointmentDto>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = appointmentDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAppointments");
                return new BaseResponseDTO<IEnumerable<AppointmentDto>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<AppointmentDto>> AddAppointment(AppointmentDto appointmentDto)
        {
            try
            {
                appointmentDto.Id = 0;

                var isHoliday = await _unitOfWork.Holidays.IsHoliday(appointmentDto.AppointmentDate);

                if (isHoliday)
                {
                    return new BaseResponseDTO<AppointmentDto>
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = "Cannot book an appointment on a holiday",
                        Data = null
                    };
                }

                var appointment = _mapper.Map<Appointment>(appointmentDto);


                if (await _appointmentRepository.HasConflict(appointment.DoctorId, appointment.PatientId, appointment.AppointmentDate))
                {
                    return new BaseResponseDTO<AppointmentDto>
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = "An appointment already exists within 30 minutes of the specified time.",
                        Data = null
                    };
                }

                await _appointmentRepository.Add(appointment);
                _unitOfWork.Save();

                return new BaseResponseDTO<AppointmentDto>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = _mapper.Map<AppointmentDto>(appointment)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddAppointment");
                return new BaseResponseDTO<AppointmentDto>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<AppointmentDto>> UpdateAppointment(AppointmentDto appointmentDto)
        {
            try
            {
                var isHoliday = await _unitOfWork.Holidays.IsHoliday(appointmentDto.AppointmentDate);

                if (isHoliday)
                {
                    return new BaseResponseDTO<AppointmentDto>
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = "Cannot update the appointment to a holiday",
                        Data = null
                    };
                }

                var existingAppointment = await _appointmentRepository.GetById(appointmentDto.Id);

                if (existingAppointment != null)
                {

                    if (await _appointmentRepository.HasConflict(appointmentDto.DoctorId, appointmentDto.PatientId, appointmentDto.AppointmentDate))
                    {
                        return new BaseResponseDTO<AppointmentDto>
                        {
                            StatusCode = (int)StatusCode.BadRequest,
                            Message = "An appointment already exists within 30 minutes of the specified time.",
                            Data = null
                        };
                    }

                    _mapper.Map(appointmentDto, existingAppointment);

                    _appointmentRepository.Update(existingAppointment);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<AppointmentDto>
                    {
                        StatusCode = (int)StatusCode.Success,
                        Data = _mapper.Map<AppointmentDto>(existingAppointment)
                    };
                }

                return new BaseResponseDTO<AppointmentDto>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Appointment not found",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateAppointment for ID: {appointmentDto.Id}");
                return new BaseResponseDTO<AppointmentDto>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<object>> DeleteAppointment(long id)
        {
            try
            {
                var existingAppointment = await _appointmentRepository.GetById(id);

                if (existingAppointment != null)
                {
                    _appointmentRepository.Delete(existingAppointment);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<object>
                    {
                        StatusCode = (int)StatusCode.Success
                    };
                }

                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Appointment not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteAppointment for ID: {id}");
                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString()
                };
            }
        }

        public async Task<BaseResponseDTO<AppointmentDto>> GetAppointmentById(long id)
        {
            BaseResponseDTO<AppointmentDto> response = new();
            try
            {
                var appointment = await _appointmentRepository.GetById(id);

                if (appointment == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                var appointmentDto = new AppointmentDto
                {
                    Id = appointment.Id,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    AppointmentDate = appointment.AppointmentDate,
                    Status = appointment.Status
                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);

                response.Data = appointmentDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }

        public async Task<BaseResponseDTO<IEnumerable<AppointmentDto>>> GetAppointmentsByPatientId(long patientId)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByPatientId(patientId);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                return new BaseResponseDTO<IEnumerable<AppointmentDto>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = appointmentDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetAppointmentsByPatientId for PatientId: {patientId}");
                return new BaseResponseDTO<IEnumerable<AppointmentDto>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<IEnumerable<AppointmentDto>>> GetAppointmentsByDoctorId(long doctorId)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByDoctorId(doctorId);
                var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

                return new BaseResponseDTO<IEnumerable<AppointmentDto>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = appointmentDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetAppointmentsByDoctorId for DoctorId: {doctorId}");
                return new BaseResponseDTO<IEnumerable<AppointmentDto>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

    }
}
