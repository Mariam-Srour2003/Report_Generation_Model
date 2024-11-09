using Domain.Models;
using Infrastructure.Repository.IRepository;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DBContext context) : base(context)
        {
        }
    }
}