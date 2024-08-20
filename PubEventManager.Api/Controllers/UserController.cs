namespace PubEventManager.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.RegisterUserAsync(registerUserDto);
        if (!result)
        {
            return BadRequest("Registration failed.");
        }

        return Ok(new { message = "User registered successfully." });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        var (token ,userId) = await _userService.AuthenticationAsync(loginDto.Email, loginDto.Password);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token, UserId = userId });
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<RegisterUserDto>> GetUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(user);
    }
}
