using Domain.Models.SharedEntity;
using System;

namespace Domain.Models
{
    public class Report : Entity
    {
        public string ReportText { get; set; }
        public long XRayImageId { get; set; }
        public XRayImage XRayImage { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
}