using MongoDB.Driver;
using WeCount.Application.Common.Interfaces;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Domain.Entities.Transaction;

namespace WeCount.Infrastructure.Repositories.TransactionCategoryRepository
{
    public class TransactionCategoryRepository : ITransactionCategoryRepository
    {
        private readonly IMongoCollection<TransactionCategory> _categories;

        public TransactionCategoryRepository(IMongoDbContext context)
        {
            _categories = context.TransactionCategories;
        }

        public async Task<TransactionCategory?> GetByNameAsync(string name)
        {
            return await _categories.Find(c => c.Code == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllAsync() =>
            await _categories.Find(_ => true).ToListAsync();

        public async Task<TransactionCategory> CreateAsync(TransactionCategory category)
        {
            await _categories.InsertOneAsync(category);
            return category;
        }

        public async Task<bool> UpdateAsync(TransactionCategory category)
        {
            ReplaceOneResult result = await _categories.ReplaceOneAsync(
                c => c.Code == category.Code,
                category
            );
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string name)
        {
            DeleteResult result = await _categories.DeleteOneAsync(c => c.Code == name);
            return result.DeletedCount > 0;
        }
    }
}
