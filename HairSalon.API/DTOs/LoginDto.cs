using HairSalon.API.Models;

namespace HairSalon.API.DTOs;

public record LoginDto(string Email, string Password);
public record LoginResponseDto(string Token, string Email, string FirstName, string LastName, UserRole Role);
