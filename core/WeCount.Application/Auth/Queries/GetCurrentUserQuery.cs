using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Auth.Queries;

public record GetCurrentUserQuery(Guid UserId) : IRequest<UserProfileDto>;
