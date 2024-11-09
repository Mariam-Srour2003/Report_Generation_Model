using System;

namespace Infrastructure.DTO
{
    public class UserDetailsDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long RoleId { get; set; }

        public static implicit operator UserDetailsDTO(List<UserDetailsDTO> v)
        {
            throw new NotImplementedException();
        }
    }
}