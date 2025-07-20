using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Common;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Application.Common.Interfaces;

namespace WeCount.Infrastructure.Repositories.UserRepository
{
    public class UserRepository(IMongoDbContext context) : IUserRepository
    {
        private readonly IMongoCollection<User> _users = context.Users;

        public Task<User?> GetByIdAsync(Guid id) => _users.GetByIdAsync(id)!;

        public async Task<User?> GetByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            var list = await _users.Find(_ => true).Paginate(page, pageSize).ToListAsync();
            return list;
        }

        public Task<User> CreateAsync(User user) => _users.CreateAsync(user);

        public Task<bool> UpdateAsync(User user) => _users.UpdateAsync(user);

        public Task<bool> DeleteAsync(Guid id) => _users.DeleteAsync(id);

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).AnyAsync();
    }
}
