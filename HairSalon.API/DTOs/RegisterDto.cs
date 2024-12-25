using HairSalon.API.Models;

namespace HairSalon.API.DTOs;

public record RegisterDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role = UserRole.Client
);