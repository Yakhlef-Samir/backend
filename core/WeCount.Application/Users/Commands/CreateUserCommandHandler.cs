using MediatR;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Domain.Entities;

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
            Guid newId = Guid.NewGuid();
            User? user = new()
            {
                Id = newId,
                Name = request.Name,
                Email = request.Email,
                Avatar = request.Avatar,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                ZipCode = request.ZipCode,
                Country = request.Country,
                CoupleId = request.CoupleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            return await _userRepository.CreateAsync(user);
        }
    }
}
