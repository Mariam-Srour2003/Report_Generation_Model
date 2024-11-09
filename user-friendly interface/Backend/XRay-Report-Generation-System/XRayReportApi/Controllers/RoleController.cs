using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.IServices;
using Infrastructure.DTO;

namespace XRayReportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<BaseResponseDTO<IEnumerable<RoleDTO>>> GetRoles()
        {
            return await _roleService.GetRoles();
        }

        [HttpPost("AddRole")]
        public async Task<BaseResponseDTO<RoleDTO>> AddRole([FromBody] RoleDTO roleDTO)
        {
            return await _roleService.AddRole(roleDTO);
        }

        [HttpPut("UpdateRole")]
        public async Task<BaseResponseDTO<RoleDTO>> UpdateRole([FromBody] RoleDTO roleDTO)
        {
            return await _roleService.UpdateRole(roleDTO);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<BaseResponseDTO<object>> DeleteRole(long id)
        {
            return await _roleService.DeleteRole(id);
        }

        [HttpGet("GetRoleById/{id}")]
        public async Task<BaseResponseDTO<RoleDTO>> GetRolebyId(long id)
        {
            return await _roleService.GetRoleById(id);
        }
    }
}
