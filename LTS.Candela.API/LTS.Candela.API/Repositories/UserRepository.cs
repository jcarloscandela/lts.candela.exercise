using LTS.Candela.API.Models;
using LTS.Candela.API.Data;

using Microsoft.EntityFrameworkCore;

namespace LTS.Candela.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .OrderByDescending(x => x.DateModified)
            .ToListAsync();
    }

    public async Task<User> UpdateUserAsync(Guid id, User user)
    {
        _context.Users.Attach(user);
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<(IEnumerable<User> Users, int TotalCount)> GetUsersPaginatedAsync(int page, int pageSize)
    {
        var totalCount = await _context.Users.CountAsync();
        var users = await _context.Users
            .OrderByDescending(u => u.DateModified)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }
}
