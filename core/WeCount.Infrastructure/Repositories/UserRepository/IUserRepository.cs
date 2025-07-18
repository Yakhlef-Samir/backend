using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCount.Domain.Entities;

namespace WeCount.Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
