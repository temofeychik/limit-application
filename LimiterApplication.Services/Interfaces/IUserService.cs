using LimiterApplication.Data.Models;
using LimiterApplication.Services.Models.Input;
using LimiterApplication.Services.Models.Output;

namespace LimiterApplication.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserOutputModel?> GetUserById(Guid id);
        IEnumerable<UserOutputModel> GetUsers();
        Task<UserOutputModel> CreateUser(UserInputModel usermodel);
        Task<UserOutputModel> UpdateUser(Guid id, UserInputModel usermodel);
        Task DeleteUser(Guid id);
    }
}
