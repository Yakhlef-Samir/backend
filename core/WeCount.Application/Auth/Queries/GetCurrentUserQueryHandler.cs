using MediatR;
using WeCount.Application.Common.Mapping;
using WeCount.Application.DTOs;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Auth.Queries;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserProfileDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapperService _mapper;

    public GetCurrentUserQueryHandler(IUserRepository userRepository, IMapperService mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserProfileDto> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            return null!;
        }

        return new UserProfileDto(
            user.Id,
            user.Email,
            user.Name.FirstName,
            user.Name.LastName,
            user.Avatar,
            user.PhoneNumber ?? string.Empty,
            user.Address ?? string.Empty,
            user.City ?? string.Empty,
            user.ZipCode ?? string.Empty,
            user.Country ?? string.Empty,
            user.CoupleId
        );
    }
}
