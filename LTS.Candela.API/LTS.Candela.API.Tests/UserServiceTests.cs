using LTS.Candela.API.Dtos;
using LTS.Candela.API.Models;
using LTS.Candela.API.Repositories;
using LTS.Candela.API.Services;
using NSubstitute;

namespace LTS.Candela.API.Tests;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userService = new UserService(_userRepository);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedUser()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Test", Email = "test@test.com", TranslationCredits = 0 };
        _userRepository.AddUserAsync(Arg.Any<User>()).Returns(user);

        var userCreateDto = new UserCreateDto { Name = "Test", Email = "test@test.com" };
        var result = await _userService.CreateUserAsync(userCreateDto);

        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.TranslationCredits, result.TranslationCredits);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnMappedUser()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Name = "Test", Email = "test@test.com", TranslationCredits = 5 };
        _userRepository.GetUserByIdAsync(userId).Returns(user);

        var result = await _userService.GetUserByIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnNull_WhenUserNotFound()
    {
        _userRepository.GetUserByIdAsync(Arg.Any<Guid>()).Returns((User)null);

        var result = await _userService.GetUserByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnUpdatedUser()
    {
        var userId = Guid.NewGuid();
        var existing = new User { Id = userId, Name = "Old", Email = "old@test.com" };
        var updated = new User { Id = userId, Name = "New", Email = "new@test.com" };

        _userRepository.GetUserByIdAsync(userId).Returns(existing);
        _userRepository.GetUserByEmailAsync("new@test.com").Returns((User)null);
        _userRepository.UpdateUserAsync(userId, Arg.Any<User>()).Returns(updated);

        var dto = new UserUpdateDto { Name = "New", Email = "new@test.com" };
        var result = await _userService.UpdateUserAsync(userId, dto);

        Assert.Equal("New", result.Name);
        Assert.Equal("new@test.com", result.Email);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnNull_WhenUserNotFound()
    {
        _userRepository.GetUserByIdAsync(Arg.Any<Guid>()).Returns((User)null);

        var result = await _userService.UpdateUserAsync(Guid.NewGuid(), new UserUpdateDto { Name = "X", Email = "x@test.com" });

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrow_WhenEmailAlreadyUsedByAnotherUser()
    {
        var userId = Guid.NewGuid();
        var existing = new User { Id = userId, Name = "Old", Email = "old@test.com" };
        var otherUser = new User { Id = Guid.NewGuid(), Name = "Other", Email = "new@test.com" };

        _userRepository.GetUserByIdAsync(userId).Returns(existing);
        _userRepository.GetUserByEmailAsync("new@test.com").Returns(otherUser);

        var dto = new UserUpdateDto { Name = "New", Email = "new@test.com" };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.UpdateUserAsync(userId, dto));
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnTrue()
    {
        _userRepository.DeleteUserAsync(Arg.Any<Guid>()).Returns(true);

        var result = await _userService.DeleteUserAsync(Guid.NewGuid());

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateCreditsAsync_ShouldReturnUpdatedUser()
    {
        var userId = Guid.NewGuid();
        var existing = new User { Id = userId, Name = "User", Email = "u@test.com", TranslationCredits = 0 };
        var updated = new User { Id = userId, Name = "User", Email = "u@test.com", TranslationCredits = 100 };

        _userRepository.GetUserByIdAsync(userId).Returns(existing);
        _userRepository.UpdateUserAsync(userId, Arg.Any<User>()).Returns(updated);

        var result = await _userService.UpdateCreditsAsync(userId, 100);

        Assert.Equal(100, result.TranslationCredits);
    }

    [Fact]
    public async Task UpdateCreditsAsync_ShouldReturnNull_WhenUserNotFound()
    {
        _userRepository.GetUserByIdAsync(Arg.Any<Guid>()).Returns((User)null);

        var result = await _userService.UpdateCreditsAsync(Guid.NewGuid(), 50);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUsersPaginatedAsync_ShouldReturnPaginatedResponse()
    {
        var users = new List<User>
        {
            new() { Id = Guid.NewGuid(), Name = "User1", Email = "user1@test.com", TranslationCredits = 1 },
            new() { Id = Guid.NewGuid(), Name = "User2", Email = "user2@test.com", TranslationCredits = 2 }
        };
        _userRepository.GetUsersPaginatedAsync(1, 10).Returns((users, 25));

        var result = await _userService.GetUsersPaginatedAsync(1, 10);

        Assert.Equal(2, result.Items.Count());
        Assert.Equal(25, result.TotalItems);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(3, result.TotalPages);
    }
}
