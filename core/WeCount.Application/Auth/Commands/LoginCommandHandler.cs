using MediatR;
using WeCount.Application.Common.Interfaces;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities;

namespace WeCount.Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher
    )
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        // Find user by email
        User user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null || user.PasswordHash is null)
        {
            Console.WriteLine("User not found"); // User not found
        }
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            Console.WriteLine("Invalid password"); // Invalid password
        }

        // Generate token
        string token = _tokenService.GenerateToken(user);

        return new AuthResponseDto(
            user.Id,
            user.Email,
            user.Name.FirstName,
            user.Name.LastName,
            token,
            DateTime.UtcNow.AddDays(7)
        );
    }
}
