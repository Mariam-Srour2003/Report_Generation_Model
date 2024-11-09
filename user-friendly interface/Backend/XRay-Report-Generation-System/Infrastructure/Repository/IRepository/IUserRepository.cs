using Infrastructure.DTO;
using Infrastructure.Repository.IBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Infrastructure.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
        public Task<IEnumerable<User>> GetAllUsers();

        public Task<IEnumerable<UserDto>> GetAllUser();

        public Task<User> GetUserById(long userId);

    }
}