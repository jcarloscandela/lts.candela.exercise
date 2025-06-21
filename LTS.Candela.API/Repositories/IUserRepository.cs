namespace LTS.Candela.API.Repositories
{
    using LTS.Candela.API.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<(IEnumerable<User> Users, int TotalCount)> GetUsersPaginatedAsync(int page, int pageSize);
        Task<User> UpdateUserAsync(Guid id, User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
    }
}
