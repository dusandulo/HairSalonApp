using Microsoft.AspNetCore.Mvc;
using HairSalon.API.DTOs;
using HairSalon.API.Services;

namespace HairSalon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        var response = await _userService.Login(loginDto);
        if (response == null)
            return Unauthorized("Invalid credentials");

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> Register(RegisterDto registerDto)
    {
        var response = await _userService.Register(registerDto);
        if (response == null)
            return BadRequest("Email already exists");

        return Ok(response);
    }
}
