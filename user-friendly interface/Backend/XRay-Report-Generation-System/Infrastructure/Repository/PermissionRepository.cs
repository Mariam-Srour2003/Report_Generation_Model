using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    private readonly DBContext _dbContext;
    private readonly ILogger<PermissionRepository> _logger;

    public PermissionRepository(DBContext context, ILogger<PermissionRepository> logger) : base(context)
    {
        _dbContext = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByRole(long roleId)
    {
        try
        {
            _logger.LogInformation("Fetching permissions for Role ID: {RoleId}", roleId);

            var permissions = await _dbContext.RolesPermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();

            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving permissions for Role ID: {RoleId}", roleId);
            throw new Exception($"Failed to retrieve permissions for role ID {roleId}: " + ex.Message);
        }
    }
}
