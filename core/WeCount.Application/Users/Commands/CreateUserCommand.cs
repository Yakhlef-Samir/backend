using MediatR;
using WeCount.Domain.Entities;
using WeCount.Domain.ValueObjects;

namespace WeCount.Application.Users.Commands
{
    public record CreateUserCommand(
        FullName Name,
        string Email,
        string Avatar,
        string? PhoneNumber,
        string? Address,
        string? City,
        string? ZipCode,
        string? Country,
        Guid CoupleId
    ) : IRequest<User>;
}
