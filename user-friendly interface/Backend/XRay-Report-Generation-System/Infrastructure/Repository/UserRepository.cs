using Domain.Models;
using Infrastructure.Data;
using Infrastructure.DTO;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DBContext _context;
        public UserRepository(DBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User> GetUserByUsername(string username)
        {

            return await _context.User.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserById(long userId)
        {
            User user = await _context.User
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return new User();
            }

            return user;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            IEnumerable<User> users = _context.User.Include(x => x.Role);

            return users;
        }


        public async Task<IEnumerable<UserDto>> GetAllUser()
        {
            List<UserDto> users = new List<UserDto>();


            users = await _context
                            .User
                            .Include(x => x.Role)
                            .Select(x => new UserDto
                            {
                                Id = x.Id,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.LastName,

                                Email = x.Email,

                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,

                                RoleId = x.RoleId,
                                CreatedDate = x.CreatedDate,
                                ModifiedDate = x.ModifiedDate,


                            })
                            .ToListAsync();



            return users;


        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return new User();
            }
            return user;
        }

    }
}