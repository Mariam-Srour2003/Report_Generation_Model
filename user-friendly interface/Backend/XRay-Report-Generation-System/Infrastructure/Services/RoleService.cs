using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Enum;
using Infrastructure.Repository;
using Infrastructure.Repository.IRepository;
using Infrastructure.Services.IServices;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                var roles = await _roleRepository.GetAll();
                var roleDTOs = _mapper.Map<IEnumerable<RoleDTO>>(roles);

                return new BaseResponseDTO<IEnumerable<RoleDTO>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = roleDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRoles");
                return new BaseResponseDTO<IEnumerable<RoleDTO>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<RoleDTO>> AddRole(RoleDTO roleDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(roleDTO.Name))
                {
                    return new BaseResponseDTO<RoleDTO>
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = "Name is required.",
                        Data = null
                    };
                }

                roleDTO.Id = 0;

                var role = _mapper.Map<Role>(roleDTO);

                await _roleRepository.Add(role);
                _unitOfWork.Save();

                return new BaseResponseDTO<RoleDTO>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = _mapper.Map<RoleDTO>(role)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddRole");
                return new BaseResponseDTO<RoleDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<RoleDTO>> UpdateRole(RoleDTO roleDTO)
        {
            try
            {
                var existingRole = await _roleRepository.GetById(roleDTO.Id);

                if (existingRole != null)
                {
                    _mapper.Map(roleDTO, existingRole);

                    _roleRepository.Update(existingRole);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<RoleDTO>
                    {
                        StatusCode = (int)StatusCode.Success,
                        Data = _mapper.Map<RoleDTO>(existingRole)
                    };
                }

                return new BaseResponseDTO<RoleDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Role not found",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateRole for ID: {roleDTO.Id}");
                return new BaseResponseDTO<RoleDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<object>> DeleteRole(long id)
        {
            try
            {
                var existingRole = await _roleRepository.GetById(id);

                if (existingRole != null)
                {
                    _roleRepository.Delete(existingRole);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<object>
                    {
                        StatusCode = (int)StatusCode.Success
                    };
                }

                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Role not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteRole for ID: {id}");
                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString()
                };
            }
        }

        public async Task<BaseResponseDTO<RoleDTO>> GetRoleById(long id)
        {
            BaseResponseDTO<RoleDTO> response = new();
            try
            {
                var role = await _roleRepository.GetById(id);

                if (role == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                var roleDto = new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);

                response.Data = roleDto;
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