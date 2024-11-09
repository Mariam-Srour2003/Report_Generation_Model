using Infrastructure.Services.IServices;
using Infrastructure.UnitOfWork;
using System;
using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<BaseResponseDTO<UserInfoDto>> Login(LoginDto request)
        {
            BaseResponseDTO<UserInfoDto> response = new();
            try
            {
                var user = await _unitOfWork.Users.GetUserByUsername(request.Username);
                string token = "";
                if (user == null)
                {
                    response.Message = "User is not valid";
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                    return response;
                }
                else
                {

                    if (!VerifyPasswordHash(request.Password, Convert.FromBase64String(user.Password), Convert.FromBase64String(user.HashSalt)))
                    {
                        response.Message = "Invalid Password";
                        response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                        return response;
                    }

                    token = await CreateToken(user);
                }

                UserInfoDto userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    RoleId = user.RoleId,
                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);
                response.Data = userInfo;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());

            }

            return response;

        }
        public async Task<BaseResponseDTO<UserDto>> Register(UserDto request)
        {
            BaseResponseDTO<UserDto> response = new();
            try
            {
                User existingUser = await _unitOfWork.Users.GetUserByUsername(request.Username);
                if (!(existingUser == null))
                {
                    response.Message = "Username already exists . Please Choose another username";
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                    return response;
                }

                if (!(ValidatePassword(request.Password)))
                {

                    response.Message = "The Password must be between 8 ana 16 characters and contain at least one uppercase and one lowercase.";
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                    return response;
                }


                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User user = new User()
                {
                    Id = 0,
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Username = request.Username,
                    Email = request.Email,
                    Password = Convert.ToBase64String(passwordHash),
                    HashSalt = Convert.ToBase64String(passwordSalt),
                    RoleId = request.RoleId,
                    CreatedDate = DateTime.Now,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,

                };

                _unitOfWork.Users.Add(user);
                _unitOfWork.Save();

                response.StatusCode = Convert.ToInt32(StatusCode.Success);
            }

            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }
        public static bool ValidatePassword(string password)
        {
            if (password.Length < 8 || password.Length > 16)
            {
                return false;
            }

            bool hasLowercase = false;
            bool hasUppercase = false;
            bool hasSpecialCharacter = false;

            foreach (char c in password)
            {
                if (char.IsLower(c))
                {
                    hasLowercase = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUppercase = true;
                }
                else if (IsSpecialCharacter(c))
                {
                    hasSpecialCharacter = true;
                }
            }

            return hasLowercase && hasUppercase && hasSpecialCharacter;
        }
        private static bool IsSpecialCharacter(char c)
        {
            string specialCharacters = "@#$-_";
            return specialCharacters.Contains(c);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hmac = new HMACSHA512(passwordSalt);
            var newPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return newPassword.SequenceEqual(passwordHash);
        }
        private async Task<string> CreateToken(User user)
        {
            var thisrole = user.RoleId;
            var role = await _unitOfWork.Roles.GetById(thisrole);
            var permissions = await _unitOfWork.Permissions.GetPermissionsByRole(thisrole);

            List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("role", role.Name),
                        new Claim("userId", user.Id.ToString())
                    };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim(permission.Name, permission.Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            // Write and return JWT token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        public async Task<BaseResponseDTO<string>> UpdateUser(long userId, UserDto updatedUserDto)
        {
            BaseResponseDTO<string> response = new();
            try
            {
                var user = await _unitOfWork.Users.GetById(userId);
                if (user == null)
                {
                    response.Message = "User is not found";
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                    return response;
                }

                user.FirstName = updatedUserDto.FirstName;
                user.MiddleName = updatedUserDto.MiddleName;
                user.LastName = updatedUserDto.LastName;
                user.Username = updatedUserDto.Username;
                user.Email = updatedUserDto.Email;
                user.RoleId = updatedUserDto.RoleId;
                user.CreatedDate = updatedUserDto.CreatedDate;
                user.DateOfBirth = updatedUserDto.DateOfBirth;
                user.Gender = updatedUserDto.Gender;

                _unitOfWork.Users.Update(user);
                _unitOfWork.Save();

                response.StatusCode = Convert.ToInt32(StatusCode.Success);
                response.Message = "User Updated Successfully";
                response.Data = "" + user.Id;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }
        public async Task<BaseResponseDTO<string>> DeleteUser(long userId)
        {
            BaseResponseDTO<String> response = new();
            try
            {
                var user = await _unitOfWork.Users.GetById(userId);
                if (user == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                _unitOfWork.Users.Delete(user);
                _unitOfWork.Save();
                response.StatusCode = Convert.ToInt32(StatusCode.Success);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }
        public async Task<BaseResponseDTO<IEnumerable<UserDetailsDTO>>> GetUsers()
        {
            BaseResponseDTO<IEnumerable<UserDetailsDTO>> response = new();
            try
            {
                var users = await _unitOfWork.Users.GetAllUsers();
                var userDtos = users.Select(user => new UserDetailsDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleId = user.RoleId 
                }).ToList();

                response.StatusCode = Convert.ToInt32(StatusCode.Success);
                response.Data = userDtos;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }
        public async Task<BaseResponseDTO<UserDetailsDTO>> GetUserById(long userId)
        {
            BaseResponseDTO<UserDetailsDTO> response = new();
            try
            {
                var user = await _unitOfWork.Users.GetById(userId);
                if (user == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                var userDetailsDto = new UserDetailsDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleId = user.RoleId 
                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);

                response.Data = userDetailsDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }
    }
}