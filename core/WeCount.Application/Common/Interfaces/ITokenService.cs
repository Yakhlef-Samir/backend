using WeCount.Domain.Entities;

namespace WeCount.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
