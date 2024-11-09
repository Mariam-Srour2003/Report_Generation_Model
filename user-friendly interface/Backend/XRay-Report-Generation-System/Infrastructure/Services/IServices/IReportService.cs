using Domain.Models;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.IServices
{
    public interface IReportService
    {
        Task<BaseResponseDTO<IEnumerable<ReportDTO>>> GetReports();
        Task<BaseResponseDTO<ReportDTO>> UpdateReport(ReportDTO reportDTO);
        Task<BaseResponseDTO<ReportDTO>> AddReport(ReportDTO reportDTO);
        Task<BaseResponseDTO<object>> DeleteReport(long id);
        Task<BaseResponseDTO<ReportDTO>> GetReportById(long id);
        Task<BaseResponseDTO<ReportDTO>> GetReportByXRayImageId(long xRayImageId);
    }
}
