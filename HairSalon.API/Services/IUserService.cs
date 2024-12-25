using HairSalon.API.DTOs;
using HairSalon.API.Models;

namespace HairSalon.API.Services;

public interface IUserService
{
    Task<LoginResponseDto?> Login(LoginDto loginDto);
    Task<bool> VerifyPassword(User user, string password);
    string CreateToken(User user);
    Task<LoginResponseDto?> Register(RegisterDto registerDto);
}
