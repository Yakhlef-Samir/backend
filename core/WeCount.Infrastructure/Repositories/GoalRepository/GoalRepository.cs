using MongoDB.Driver;
using WeCount.Domain.Entities;
using WeCount.Infrastructure.Common;
using WeCount.Infrastructure.Interfaces;

namespace WeCount.Infrastructure.Repositories.GoalRepository
{
    public class GoalRepository(IMongoDbContext context) : IGoalRepository
    {
        private readonly IMongoCollection<Goal> _goals = context.Goals;

        public Task<Goal?> GetByIdAsync(Guid id) => _goals.GetByIdAsync(id)!;

        public async Task<IEnumerable<Goal>> GetByCoupleIdAsync(
            Guid coupleId,
            int page = 1,
            int pageSize = 20
        ) => await _goals.Find(g => g.CoupleId == coupleId).Paginate(page, pageSize).ToListAsync();

        public async Task<IEnumerable<Goal>> GetAllAsync(int page = 1, int pageSize = 20) =>
            await _goals.Find(_ => true).Paginate(page, pageSize).ToListAsync();

        public Task<Goal> CreateAsync(Goal goal) => _goals.CreateAsync(goal);

        public Task<bool> UpdateAsync(Goal goal) => _goals.UpdateAsync(goal);

        public Task<bool> DeleteAsync(Guid id) => _goals.DeleteAsync(id);
    }
}
