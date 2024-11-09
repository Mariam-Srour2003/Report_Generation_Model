using Domain.Models;
using Infrastructure.Repository.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.IRepository
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<Report> GetByXRayImageIdAsync(long xRayImageId);
    }
}
