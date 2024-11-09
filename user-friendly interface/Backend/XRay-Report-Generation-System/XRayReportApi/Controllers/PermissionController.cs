using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.IServices;
using Infrastructure.DTO;
using Infrastructure.Services;
using Domain.Models;

namespace XRayReportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        [Route("GetAllPermissions")]
        public async Task<BaseResponseDTO<IEnumerable<PermissionDTO>>> GetPermissions()
        {
            return await _permissionService.GetPermissions();
        }

        [HttpPost("AddPermission")]
        public async Task<BaseResponseDTO<PermissionDTO>> AddPermission([FromBody] PermissionDTO permissionDTO)
        {
            return await _permissionService.AddPermission(permissionDTO);
        }

        [HttpPut("UpdatePermission")]
        public async Task<BaseResponseDTO<PermissionDTO>> UpdatePermission([FromBody] PermissionDTO permissionDTO)
        {
            return await _permissionService.UpdatePermission(permissionDTO);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<BaseResponseDTO<object>> DeletePermission(long id)
        {
            return await _permissionService.DeletePermission(id);
        }

        [HttpGet("GetPermissionById/{id}")]
        public async Task<BaseResponseDTO<PermissionDTO>> GetPermissionbyId(long id)
        {
            return await _permissionService.GetPermissionById(id);
        }

        [HttpGet("GetPermissionsByRole/{roleId}")]
        public async Task<BaseResponseDTO<IEnumerable<PermissionDTO>>> GetPermissionsByRole(long roleId)
        {
            return await _permissionService.GetPermissionsByRole(roleId);
        }
    }
}
