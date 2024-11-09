using Domain.Models.SharedEntity;
using System;

namespace Domain.Models
{
    public class RolesPermissions : Entity
    {
        public Role Role { get; set; }
        public long RoleId { get; set; }
        public Permission Permission { get; set; }
        public long PermissionId { get; set; }
    }
}
