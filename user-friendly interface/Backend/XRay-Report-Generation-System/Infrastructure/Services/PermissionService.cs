using AutoMapper;
using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Enum;
using Infrastructure.Repository;
using Infrastructure.Repository.IRepository;
using Infrastructure.Services.IServices;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionService> _logger;
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<PermissionService> logger)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponseDTO<IEnumerable<PermissionDTO>>> GetPermissions()
        {
            try
            {
                var permissions = await _permissionRepository.GetAll();
                var permissionDTOs = _mapper.Map<IEnumerable<PermissionDTO>>(permissions);

                return new BaseResponseDTO<IEnumerable<PermissionDTO>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = permissionDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPermissions");
                return new BaseResponseDTO<IEnumerable<PermissionDTO>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<IEnumerable<PermissionDTO>>> GetPermissionsByRole(long roleId)
        {
            _logger.LogInformation("Fetching permissions for Role ID: {RoleId}", roleId);

            BaseResponseDTO<IEnumerable<PermissionDTO>> response = new();

            try
            {
                var permissions = await _permissionRepository.GetPermissionsByRole(roleId);
                var permissionDTOs = _mapper.Map<IEnumerable<PermissionDTO>>(permissions);

                response.StatusCode = (int)StatusCode.Success;
                response.Data = permissionDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPermissionsByRole for RoleId: {RoleId}", roleId);
                response.StatusCode = (int)StatusCode.BadRequest;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponseDTO<PermissionDTO>> AddPermission(PermissionDTO permissionDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(permissionDTO.Name))
                {
                    return new BaseResponseDTO<PermissionDTO>
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = "Name is required.",
                        Data = null
                    };
                }

                permissionDTO.Id = 0;

                var permission = _mapper.Map<Permission>(permissionDTO);

                await _permissionRepository.Add(permission);
                _unitOfWork.Save();

                return new BaseResponseDTO<PermissionDTO>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = _mapper.Map<PermissionDTO>(permission)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddPermission");
                return new BaseResponseDTO<PermissionDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<PermissionDTO>> UpdatePermission(PermissionDTO permissionDTO)
        {
            try
            {
                var existingPermission = await _permissionRepository.GetById(permissionDTO.Id);

                if (existingPermission != null)
                {
                    _mapper.Map(permissionDTO, existingPermission);

                    _permissionRepository.Update(existingPermission);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<PermissionDTO>
                    {
                        StatusCode = (int)StatusCode.Success,
                        Data = _mapper.Map<PermissionDTO>(existingPermission)
                    };
                }

                return new BaseResponseDTO<PermissionDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Permission not found",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdatePermission for ID: {permissionDTO.Id}");
                return new BaseResponseDTO<PermissionDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<object>> DeletePermission(long id)
        {
            try
            {
                var existingPermission = await _permissionRepository.GetById(id);

                if (existingPermission != null)
                {
                    _permissionRepository.Delete(existingPermission);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<object>
                    {
                        StatusCode = (int)StatusCode.Success
                    };
                }

                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "Permission not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeletePermission for ID: {id}");
                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString()
                };
            }
        }

        public async Task<BaseResponseDTO<PermissionDTO>> GetPermissionById(long id)
        {
            BaseResponseDTO<PermissionDTO> response = new();
            try
            {
                var permission = await _permissionRepository.GetById(id);

                if (permission == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                var permissionDto = new PermissionDTO
                {
                    Id = permission.Id,
                    Name = permission.Name
                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);

                response.Data = permissionDto;
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