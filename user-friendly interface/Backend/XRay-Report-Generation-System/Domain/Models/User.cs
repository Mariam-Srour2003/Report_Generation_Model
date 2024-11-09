using Domain.Models.SharedEntity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User : Entity
    {

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        [MaxLength(128)]
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string Password { get; set; }
        public string HashSalt { get; set; }


        public long RoleId { get; set; }
        public Role Role { get; set; }
        public bool Active { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? ForceChangePassword { get; set; }

        public ICollection<Appointment> PatientAppointments { get; set; }
        public ICollection<Appointment> DoctorAppointments { get; set; }
    }
}