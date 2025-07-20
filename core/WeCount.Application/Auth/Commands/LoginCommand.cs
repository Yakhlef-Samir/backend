using MediatR;
using WeCount.Application.DTOs;

namespace WeCount.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
