using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Users.Queries
{
    public class GetUserByIdQuery(Guid id) : IRequest<UserDto?>
    {
        public Guid Id { get; } = id;
    }
}
