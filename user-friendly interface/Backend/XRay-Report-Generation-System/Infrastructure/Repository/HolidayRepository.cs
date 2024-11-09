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
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(DBContext context) : base(context)
        {
        }

        public async Task<bool> IsHoliday(DateTime date)
        {
            return await _context.Set<Holiday>().AnyAsync(h => h.Date.Date == date.Date);
        }
    }
}
