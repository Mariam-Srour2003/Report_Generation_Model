using Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.IServices
{
    public interface IRoleService
    {
        Task<BaseResponseDTO<IEnumerable<RoleDTO>>> GetRoles();
        Task<BaseResponseDTO<RoleDTO>> AddRole(RoleDTO roleDTO);
        Task<BaseResponseDTO<RoleDTO>> UpdateRole(RoleDTO roleDTO);
        Task<BaseResponseDTO<object>> DeleteRole(long id);
        Task<BaseResponseDTO<RoleDTO>> GetRoleById(long id);

    }
}