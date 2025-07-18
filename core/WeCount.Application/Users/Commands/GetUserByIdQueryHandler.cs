using MediatR;
using WeCount.Application.Users.Queries;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Repositories.UserRepository;

namespace WeCount.Application.Users.Commands
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _userRepository.GetByIdAsync(request.UserId);
        }
    }
}
