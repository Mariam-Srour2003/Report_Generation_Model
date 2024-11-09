using Domain.Models.SharedEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Appointment : Entity
    {
        public long PatientId { get; set; }
        public long DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }

        public User Patient { get; set; }
        public User Doctor { get; set; }
    }

}
