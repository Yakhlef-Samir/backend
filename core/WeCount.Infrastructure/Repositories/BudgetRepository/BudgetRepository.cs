using MongoDB.Driver;
using WeCount.Domain.Entities.Budget;
using WeCount.Infrastructure.Common;
using WeCount.Infrastructure.Interfaces;

namespace WeCount.Infrastructure.Repositories.BudgetRepository
{
    public class BudgetRepository(IMongoDbContext context) : IBudgetRepository
    {
        private readonly IMongoCollection<Budget> _budgets = context.Budgets;

        public Task<Budget?> GetByIdAsync(Guid id)
        {
            return _budgets.GetByIdAsync(id)!;
        }

        public async Task<IEnumerable<Budget>> GetByCoupleIdAsync(
            Guid coupleId,
            int page = 1,
            int pageSize = 20
        ) =>
            await _budgets.Find(b => b.CoupleId == coupleId).Paginate(page, pageSize).ToListAsync();

        public async Task<IEnumerable<Budget>> GetAllAsync(int page = 1, int pageSize = 20) =>
            await _budgets.Find(_ => true).Paginate(page, pageSize).ToListAsync();

        public Task<Budget> CreateAsync(Budget budget) => _budgets.CreateAsync(budget);

        public Task<bool> UpdateAsync(Budget budget) => _budgets.UpdateAsync(budget);

        public Task<bool> DeleteAsync(Guid id) => _budgets.DeleteAsync(id);
    }
}
