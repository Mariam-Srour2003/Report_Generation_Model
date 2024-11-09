using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class HolidayDTO
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
