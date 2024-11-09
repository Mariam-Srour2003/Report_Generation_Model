using AutoMapper;
using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Enum;
using Infrastructure.Repository;
using Infrastructure.Repository.IRepository;
using Infrastructure.Services.IServices;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HolidayService> _logger;

        public HolidayService(
            IHolidayRepository holidayRepository,
            IUnitOfWork unitOfWork,
        IMapper mapper,
            ILogger<HolidayService> logger)
        {
            _holidayRepository = holidayRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<IEnumerable<HolidayDTO>>> GetHolidays()
        {
            try
            {
                var holidays = await _holidayRepository.GetAll();
                var holidayDtos = _mapper.Map<IEnumerable<HolidayDTO>>(holidays);

                return new BaseResponseDTO<IEnumerable<HolidayDTO>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = holidayDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHolidays");
                return new BaseResponseDTO<IEnumerable<HolidayDTO>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<HolidayDTO>> AddHoliday(HolidayDTO holidayDto)
        {
            try
            {
                var holiday = _mapper.Map<Holiday>(holidayDto);

                await _holidayRepository.Add(holiday);
                _unitOfWork.Save();

                return new BaseResponseDTO<HolidayDTO>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = _mapper.Map<HolidayDTO>(holiday)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddHoliday");
                return new BaseResponseDTO<HolidayDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<object>> DeleteHoliday(long id)
        {
            try
            {
                var existingHoliday = await _holidayRepository.GetById(id);

                if (existingHoliday != null)
                {
                    _holidayRepository.Delete(existingHoliday);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<object>
                    {
                        StatusCode = (int)StatusCode.Success
                    };
                }

                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Holiday not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteHoliday for ID: {id}");
                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString()
                };
            }
        }

        public async Task<BaseResponseDTO<bool>> IsHoliday(DateTime date)
        {
            try
            {
                var isHoliday = await _holidayRepository.IsHoliday(date);

                return new BaseResponseDTO<bool>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = isHoliday
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in IsHoliday for Date: {date}");
                return new BaseResponseDTO<bool>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = false
                };
            }
        }

        public async Task<BaseResponseDTO<HolidayDTO>> GetHolidayById(long id)
        {
            BaseResponseDTO<HolidayDTO> response = new();
            try
            {
                var holiday = await _holidayRepository.GetById(id);

                if (holiday == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                var holidayDto = new HolidayDTO
                {
                    Id = holiday.Id,
                    Date = holiday.Date,
                    Description = holiday.Description

                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);

                response.Data = holidayDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }

        public async Task<BaseResponseDTO<HolidayDTO>> UpdateHoliday(HolidayDTO holidayDTO)
        {
            try
            {
                var existingHoliday = await _holidayRepository.GetById(holidayDTO.Id);

                if (existingHoliday != null)
                {
                    _mapper.Map(holidayDTO, existingHoliday);

                    _holidayRepository.Update(existingHoliday);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<HolidayDTO>
                    {
                        StatusCode = (int)StatusCode.Success,
                        Data = _mapper.Map<HolidayDTO>(existingHoliday)
                    };
                }

                return new BaseResponseDTO<HolidayDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Holiday not found",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateHoliday for ID: {holidayDTO.Id}");
                return new BaseResponseDTO<HolidayDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

    }
}