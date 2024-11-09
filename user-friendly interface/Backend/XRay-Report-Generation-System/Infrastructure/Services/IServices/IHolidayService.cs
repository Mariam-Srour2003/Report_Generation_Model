using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.IServices
{
    public interface IHolidayService
    {
        Task<BaseResponseDTO<IEnumerable<HolidayDTO>>> GetHolidays();
        Task<BaseResponseDTO<HolidayDTO>> AddHoliday(HolidayDTO holidayDto);
        Task<BaseResponseDTO<HolidayDTO>> UpdateHoliday(HolidayDTO holidayDto);
        Task<BaseResponseDTO<object>> DeleteHoliday(long id);
        Task<BaseResponseDTO<HolidayDTO>> GetHolidayById(long id);
        Task<BaseResponseDTO<bool>> IsHoliday(DateTime date);
    }
}
