using System;

namespace Infrastructure.DTO
{
    public class UserDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public long RoleId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public char Gender { get; set; }

    }
}