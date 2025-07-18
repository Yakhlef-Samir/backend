using MediatR;
using WeCount.Domain.Entities;

namespace WeCount.Application.Users.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<User?>;
}
