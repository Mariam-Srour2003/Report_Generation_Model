using System;

namespace Infrastructure.DTO
{
    internal class AddUserDTO
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public string? Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public long RoleId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? ForceChangePassword { get; set; }
    }
}
