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
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IReportRepository reportRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<IEnumerable<ReportDTO>>> GetReports()
        {
            try
            {
                var reports = await _reportRepository.GetAll();
                var reportDTOs = _mapper.Map<IEnumerable<ReportDTO>>(reports);

                return new BaseResponseDTO<IEnumerable<ReportDTO>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = reportDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReports");
                return new BaseResponseDTO<IEnumerable<ReportDTO>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<ReportDTO>> AddReport(ReportDTO reportDTO)
        {
            try
            {
                reportDTO.Id = 0;

                var report = _mapper.Map<Report>(reportDTO);

                await _reportRepository.Add(report);
                _unitOfWork.Save();

                return new BaseResponseDTO<ReportDTO>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = _mapper.Map<ReportDTO>(report)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add Report");
                return new BaseResponseDTO<ReportDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<ReportDTO>> UpdateReport(ReportDTO reportDTO)
        {
            try
            {
                var existingReport = await _reportRepository.GetById(reportDTO.Id);

                if (existingReport != null)
                {
                    _mapper.Map(reportDTO, existingReport);

                    _reportRepository.Update(existingReport);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<ReportDTO>
                    {
                        StatusCode = (int)StatusCode.Success,
                        Data = _mapper.Map<ReportDTO>(existingReport)
                    };
                }

                return new BaseResponseDTO<ReportDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Report not found",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Update Report for ID: {reportDTO.Id}");
                return new BaseResponseDTO<ReportDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<object>> DeleteReport(long id)
        {
            try
            {
                var existingReport = await _reportRepository.GetById(id);

                if (existingReport != null)
                {
                    _reportRepository.Delete(existingReport);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<object>
                    {
                        StatusCode = (int)StatusCode.Success
                    };
                }

                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Report not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Delete Report for ID: {id}");
                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString()
                };
            }
        }

        public async Task<BaseResponseDTO<ReportDTO>> GetReportById(long id)
        {
            BaseResponseDTO<ReportDTO> response = new();
            try
            {
                var report = await _reportRepository.GetById(id);

                if (report == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                var reportDto = new ReportDTO
                {
                    Id = report.Id,
                    ReportText = report.ReportText,
                    XRayImageId = report.XRayImageId,
                    GeneratedDate = report.GeneratedDate
                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);

                response.Data = reportDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }

        public async Task<BaseResponseDTO<ReportDTO>> GetReportByXRayImageId(long xRayImageId)
        {
            BaseResponseDTO<ReportDTO> response = new();
            try
            {
                var report = await _reportRepository.GetByXRayImageIdAsync(xRayImageId);

                if (report == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                    response.Message = "Report not found";
                    return response;
                }

                var reportDto = new ReportDTO
                {
                    Id = report.Id,
                    ReportText = report.ReportText,
                    XRayImageId = report.XRayImageId,
                    GeneratedDate = report.GeneratedDate
                };

                response.StatusCode = Convert.ToInt32(StatusCode.Success);
                response.Data = reportDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }

            return response;
        }

    }
}
