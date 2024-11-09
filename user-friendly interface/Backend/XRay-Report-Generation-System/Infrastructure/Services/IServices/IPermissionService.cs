using Domain.Models;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.IServices
{
    public interface IPermissionService
    {
        Task<BaseResponseDTO<IEnumerable<PermissionDTO>>> GetPermissionsByRole(long roleId);
        Task<BaseResponseDTO<IEnumerable<PermissionDTO>>> GetPermissions();
        Task<BaseResponseDTO<PermissionDTO>> AddPermission(PermissionDTO permissionDTO);
        Task<BaseResponseDTO<PermissionDTO>> UpdatePermission(PermissionDTO permissionDTO);
        Task<BaseResponseDTO<object>> DeletePermission(long id);
        Task<BaseResponseDTO<PermissionDTO>> GetPermissionById(long id);
    }
}
