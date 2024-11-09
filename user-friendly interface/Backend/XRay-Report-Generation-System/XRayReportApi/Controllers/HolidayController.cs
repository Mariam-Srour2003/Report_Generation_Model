using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.IServices;
using Infrastructure.DTO;
using Infrastructure.Services;

namespace XRayReportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpGet]
        [Route("GetAllHolidays")]
        public async Task<BaseResponseDTO<IEnumerable<HolidayDTO>>> GetHolidays()
        {
            return await _holidayService.GetHolidays();
        }

        [HttpPost("AddHoliday")]
        public async Task<BaseResponseDTO<HolidayDTO>> AddHoliday([FromBody] HolidayDTO holidayDTO)
        {
            return await _holidayService.AddHoliday(holidayDTO);
        }

        [HttpPut("UpdateHoliday")]
        public async Task<BaseResponseDTO<HolidayDTO>> UpdateHoliday([FromBody] HolidayDTO holidayDTO)
        {
            return await _holidayService.UpdateHoliday(holidayDTO);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<BaseResponseDTO<object>> DeleteHoliday(long id)
        {
            return await _holidayService.DeleteHoliday(id);
        }

        [HttpGet("GetHolidayById/{id}")]
        public async Task<BaseResponseDTO<HolidayDTO>> GetHolidaybyId(long id)
        {
            return await _holidayService.GetHolidayById(id);
        }

    }
}
