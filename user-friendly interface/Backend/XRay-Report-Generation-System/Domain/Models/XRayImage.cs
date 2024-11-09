using Domain.Models.SharedEntity;
using System;

namespace Domain.Models
{
    public class XRayImage : Entity
    {
        public string FileName { get; set; }
        public string ImagePath { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
