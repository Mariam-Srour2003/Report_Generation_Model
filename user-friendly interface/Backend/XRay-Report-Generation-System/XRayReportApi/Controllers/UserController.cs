using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.DTO;
using Infrastructure.Services.IServices;

namespace XRayReportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;


        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<BaseResponseDTO<UserInfoDto>> Login(LoginDto request)
        {
            BaseResponseDTO<UserInfoDto> userInfo = await _userService.Login(request);
            return userInfo;
        }

        [HttpPost("register")]
        public async Task<BaseResponseDTO<UserDto>> Register(UserDto request)
        {

            BaseResponseDTO<UserDto> user = await _userService.Register(request);

            return user;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<BaseResponseDTO<IEnumerable<UserDetailsDTO>>> GetAllUsers()
        {
            var users = await _userService.GetUsers();
            return users;
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<BaseResponseDTO<UserDetailsDTO>> GetUserById(long userId)
        {

            var userDetailsDto = await _userService.GetUserById(userId);
            return userDetailsDto;

        }


        [HttpPut("Update/{userId}")]
        public async Task<BaseResponseDTO<string>> UpdateUser(int userId, UserDto updatedUserDto)
        {

            var result = await _userService.UpdateUser(userId, updatedUserDto);
            return result;

        }

        [HttpDelete("Delete/{userId}")]
        public async Task<BaseResponseDTO<string>> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUser(userId);
            return result;
        }


        


    }
}
