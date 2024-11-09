using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Repository.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.IRepository
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<IEnumerable<Permission>> GetPermissionsByRole(long roleId);
    }
}