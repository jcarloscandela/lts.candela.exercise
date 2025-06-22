using LTS.Candela.API.Dtos;
using LTS.Candela.API.Models;
using LTS.Candela.API.Repositories;

namespace LTS.Candela.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto userCreateDto)
    {
        var user = new User
        {
            Name = userCreateDto.Name,
            Email = userCreateDto.Email,
            TranslationCredits = 0, // Default credits
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
        var createdUser = await _userRepository.AddUserAsync(user);

        return MapToDto(createdUser);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return MapToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null) return null;

        var existingUser = await _userRepository.GetUserByEmailAsync(userUpdateDto.Email);
        if (existingUser != null && existingUser.Id != id)
        {
            throw new InvalidOperationException("Email is already in use by another user.");
        }

        user.Name = userUpdateDto.Name;
        user.Email = userUpdateDto.Email;
        user.DateModified = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateUserAsync(id, user);

        return MapToDto(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        return await _userRepository.DeleteUserAsync(id);
    }

    public async Task<UserDto> UpdateCreditsAsync(Guid id, int credits)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return null;

        user.TranslationCredits = credits;
        user.DateModified = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateUserAsync(id, user);
        return MapToDto(updatedUser);
    }

    private UserDto MapToDto(User user)
    {
        if (user == null) return null;
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            TranslationCredits = user.TranslationCredits
        };
    }

    public async Task<PaginatedResponse<UserDto>> GetUsersPaginatedAsync(int page, int pageSize)
    {
        var (users, totalCount) = await _userRepository.GetUsersPaginatedAsync(page, pageSize);
        var userDtos = users.Select(MapToDto);

        return new PaginatedResponse<UserDto>(userDtos, totalCount, page, pageSize);
    }
}
