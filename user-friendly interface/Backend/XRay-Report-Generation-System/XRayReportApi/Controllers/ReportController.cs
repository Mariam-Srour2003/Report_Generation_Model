using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.IServices;
using Infrastructure.DTO;
using Infrastructure.Services;

namespace XRayReportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("GetAllReports")]
        public async Task<BaseResponseDTO<IEnumerable<ReportDTO>>> GetReports()
        {
            return await _reportService.GetReports();
        }

        [HttpPost("AddReport")]
        public async Task<BaseResponseDTO<ReportDTO>> AddReport([FromBody] ReportDTO reportDTO)
        {
            return await _reportService.AddReport(reportDTO);
        }

        [HttpPut("UpdateReport")]
        public async Task<BaseResponseDTO<ReportDTO>> UpdateReport([FromBody] ReportDTO reportDTO)
        {
            return await _reportService.UpdateReport(reportDTO);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<BaseResponseDTO<object>> DeleteReport(long id)
        {
            return await _reportService.DeleteReport(id);
        }

        [HttpGet("GetReportById/{id}")]
        public async Task<BaseResponseDTO<ReportDTO>> GetReportbyId(long id)
        {
            return await _reportService.GetReportById(id);
        }

        [HttpGet("GetReportByXRayImageId/{id}")]
        public async Task<BaseResponseDTO<ReportDTO>> GetReportByXRayImageId(long id)
        {
            return await _reportService.GetReportByXRayImageId(id);
        }

    }
}
