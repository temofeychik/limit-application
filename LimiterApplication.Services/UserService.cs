using LimiterApplication.Data;
using LimiterApplication.Data.Models;
using LimiterApplication.Services.Interfaces;
using LimiterApplication.Services.Models.Input;
using LimiterApplication.Services.Models.Output;

namespace LimiterApplication.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserOutputModel> CreateUser(UserInputModel usermodel)
        {
            User user = new User { FirstName = usermodel.FirstName, LastName = usermodel.LastName};
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new UserOutputModel { FirstName = user.FirstName, LastName = user.LastName, Id = user.Id};
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserOutputModel?> GetUserById(Guid id)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                return new UserOutputModel { FirstName = user.FirstName, LastName = user.LastName, Id = user.Id };
            }
            return null;
        }

        public IEnumerable<UserOutputModel> GetUsers()
        {
            return _context.Users.ToList().Select(x => new UserOutputModel { FirstName = x.FirstName, Id = x.Id, LastName = x.LastName} );
        }

        public async Task<UserOutputModel> UpdateUser(Guid id, UserInputModel usermodel)
        {
            User user = new User { FirstName = usermodel.FirstName, LastName = usermodel.LastName, Id = id };
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new UserOutputModel { FirstName = user.FirstName, LastName = user.LastName, Id = user.Id };
        }
    }
}