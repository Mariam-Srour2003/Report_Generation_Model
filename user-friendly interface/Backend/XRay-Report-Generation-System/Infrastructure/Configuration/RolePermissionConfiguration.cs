using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolesPermissions>
    {
        public void Configure(EntityTypeBuilder<RolesPermissions> builder)
        {
            builder.HasKey(rp => rp.Id);

            builder.HasOne(rp => rp.Role)
                   .WithMany()
                   .HasForeignKey(rp => rp.RoleId);

            builder.HasOne(rp => rp.Permission)
                   .WithMany()
                   .HasForeignKey(rp => rp.PermissionId);
        }
    }
}
