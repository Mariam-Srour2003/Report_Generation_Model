using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class XRayImageDTO
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string ImagePath { get; set; }
        public long UserId { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
