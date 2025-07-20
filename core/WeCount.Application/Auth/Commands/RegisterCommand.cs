using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Auth.Commands;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Address,
    string City,
    string ZipCode,
    string Country,
    string Avatar
) : IRequest<AuthResponseDto>;
