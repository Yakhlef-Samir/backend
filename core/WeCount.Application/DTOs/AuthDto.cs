using WeCount.Domain.Entities;

namespace WeCount.Application.DTOs;

public record AuthResponseDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Token,
    DateTime TokenExpiration
);

public record UserProfileDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Avatar,
    string PhoneNumber,
    string Address,
    string City,
    string ZipCode,
    string Country,
    Guid? CoupleId
);
