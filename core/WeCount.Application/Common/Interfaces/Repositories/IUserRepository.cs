using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCount.Domain.Entities;

namespace WeCount.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid Id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<User> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid Id);
        Task<bool> ExistsByEmailAsync(string email);
    }
}

