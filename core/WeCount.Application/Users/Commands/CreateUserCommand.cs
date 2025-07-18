using MediatR;
using WeCount.Domain.Entities;

namespace WeCount.Application.Users.Commands
{
    public record CreateUserCommand(string Name, string Email) : IRequest<User>;
}