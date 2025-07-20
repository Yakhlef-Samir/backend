using MediatR;
using WeCount.Application.Common.Interfaces;
using WeCount.Application.Common.Mapping;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities;
using WeCount.Domain.ValueObjects;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapperService _mapper;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IMapperService mapper
    )
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Name = new FullName(request.FirstName, request.LastName),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            City = request.City,
            ZipCode = request.ZipCode,
            Country = request.Country,
            Avatar = request.Avatar,
        };

        await _userRepository.CreateAsync(user);

        // Generate token
        var token = _tokenService.GenerateToken(user);

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
