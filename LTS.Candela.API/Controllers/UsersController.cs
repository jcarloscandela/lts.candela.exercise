using Microsoft.AspNetCore.Mvc;
using LTS.Candela.API.Services;
using LTS.Candela.API.Dtos;
using LTS.Candela.API.Models;

namespace LTS.Candela.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<UserDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var paginatedUsers = await _userService.GetUsersPaginatedAsync(page, pageSize);
        return Ok(paginatedUsers);
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var userDto = await _userService.GetUserByIdAsync(id);

        if (userDto == null)
        {
            return NotFound();
        }

        return Ok(userDto);
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto userCreateDto)
    {
        var userDto = await _userService.CreateUserAsync(userCreateDto);

        return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
    }

    // PATCH: api/User/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUser(Guid id, [FromBody] UserUpdateDto userUpdateDto)
    {
        var updatedUserDto = await _userService.UpdateUserAsync(id, userUpdateDto);
        if (updatedUserDto == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var deleted = await _userService.DeleteUserAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    // PATCH: api/User/5/credits
    [HttpPatch("{id}/credits")]
    public async Task<IActionResult> UpdateCredits(Guid id, [FromBody] int credits)
    {
        var updatedUserDto = await _userService.UpdateCreditsAsync(id, credits);
        if (updatedUserDto == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}
