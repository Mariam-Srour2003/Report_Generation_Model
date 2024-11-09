using System;

namespace Infrastructure.DTO
{
    public class UserInfoDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; }
        public string Email { get; set; } = string.Empty;
        public long RoleId { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}