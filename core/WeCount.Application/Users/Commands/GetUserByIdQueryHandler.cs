using MediatR;
using MongoDB.Driver;
using WeCount.Application.Common.Mapping;
using WeCount.Application.DTOs;
using WeCount.Application.Users.Queries;
using WeCount.Domain.Entities;

namespace WeCount.Application.Users.Commands;

public class GetUserByIdQueryHandler(IMongoCollection<User> collection, IMapperService mapper)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        User? entity = await collection
            .Find(u => u.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        return entity is null ? null : mapper.Map<UserDto>(entity);
    }
}
