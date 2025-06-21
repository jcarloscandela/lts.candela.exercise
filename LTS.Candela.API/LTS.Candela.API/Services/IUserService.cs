using LTS.Candela.API.Dtos;
using LTS.Candela.API.Models;

namespace LTS.Candela.API.Services;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<PaginatedResponse<UserDto>> GetUsersPaginatedAsync(int page, int pageSize);
    Task<UserDto> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<UserDto> UpdateCreditsAsync(Guid id, int credits);
}
