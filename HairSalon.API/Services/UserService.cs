using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HairSalon.API.Data;
using HairSalon.API.DTOs;
using HairSalon.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HairSalon.API.Services;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public UserService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return null;

        string token = GenerateJwtToken(user);

        return new LoginResponseDto(
            token,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.Role, user.Role.ToString()) 
    };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<LoginResponseDto?> Register(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            return null;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Role = 0
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new LoginResponseDto(
            GenerateJwtToken(user),
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role); 
    }

    public Task<bool> VerifyPassword(User user, string password)
    {
        using var hmac = new HMACSHA512();
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Task.FromResult(computedHash.SequenceEqual(Convert.FromBase64String(user.PasswordHash)));
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
    {
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.Role, user.Role.ToString()), 
        new(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
