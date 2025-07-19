using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Interfaces;

namespace WeCount.Infrastructure.Repositories.UserRepository
{
    public class UserRepository(IMongoDbContext context) : IUserRepository
    {
        private readonly IMongoCollection<User> _users = context.Users;

        public async Task<User?> GetByIdAsync(Guid Id) =>
            await _users.Find(u => u.Id == Id).FirstOrDefaultAsync();

        public async Task<User?> GetByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            return await _users
                .Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return user;
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var result = await _users.DeleteOneAsync(u => u.Id == Id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _users.Find(u => u.Email == email).AnyAsync();
    }
}
