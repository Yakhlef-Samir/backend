using MediatR;
using WeCount.Application.Users.Commands;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Repositories.UserRepository;

namespace WeCount.Application.Users.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var newId = Guid.NewGuid();
            var user = new User
            {
                Id = newId,
                UserId = newId,
                Email = request.Email,
                FirstName = request.Name,
                LastName = string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            return await _userRepository.CreateAsync(user);
        }
    }
}
