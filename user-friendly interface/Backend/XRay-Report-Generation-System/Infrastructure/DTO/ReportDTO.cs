using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class ReportDTO
    {
        public long Id { get; set; }
        public string ReportText { get; set; }
        public long XRayImageId { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}
