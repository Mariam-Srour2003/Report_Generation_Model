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
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly DBContext _context;

        public ReportRepository(DBContext context) : base(context)
        {
            _context = context;

        }
        public async Task<Report> GetByXRayImageIdAsync(long xRayImageId)
        {
            return await _context.Reports
                .Include(r => r.XRayImage)
                .FirstOrDefaultAsync(r => r.XRayImageId == xRayImageId);
        }

    }
}
