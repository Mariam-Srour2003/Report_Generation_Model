using Infrastructure.DTO;
using System;

namespace Infrastructure.Services.IServices
{
    public interface IUserService
    {
        Task<BaseResponseDTO<UserInfoDto>> Login(LoginDto request);
        Task<BaseResponseDTO<UserDto>> Register(UserDto request);
        public Task<BaseResponseDTO<string>> UpdateUser(long userId, UserDto updatedUserDto);
        public Task<BaseResponseDTO<string>> DeleteUser(long userId);
        public Task<BaseResponseDTO<IEnumerable<UserDetailsDTO>>> GetUsers();
        public Task<BaseResponseDTO<UserDetailsDTO>> GetUserById(long userId);
    }
}
