using MongoDB.Driver;
using WeCount.Application.Common.Interfaces;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Infrastructure.Common;
using CoupleEntity = WeCount.Domain.Entities.Couple.Couple;
using CoupleScoreHistoryItem = WeCount.Domain.Entities.Couple.CoupleScoreHistoryItem;

namespace WeCount.Infrastructure.Repositories.CoupleRepository
{
    public class CoupleRepository(IMongoDbContext context) : ICoupleRepository
    {
        private readonly IMongoCollection<CoupleEntity> _couples = context.Couples;

        public Task<CoupleEntity?> GetByIdAsync(Guid id) => _couples.GetByIdAsync(id)!;

        public async Task<IEnumerable<CoupleEntity>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            return await _couples.Find(_ => true).Paginate(page, pageSize).ToListAsync();
        }

        public Task<CoupleEntity> CreateAsync(CoupleEntity couple) => _couples.CreateAsync(couple);

        public Task<bool> UpdateAsync(CoupleEntity couple) => _couples.UpdateAsync(couple);

        public Task<bool> DeleteAsync(Guid id) => _couples.DeleteAsync(id);

        private readonly IMongoCollection<CoupleScoreHistoryItem> _scoreHistoryCollection =
            context.CoupleScores;

        public async Task<IEnumerable<CoupleScoreHistoryItem>> GetScoreHistoryAsync(Guid coupleId)
        {
            IEnumerable<CoupleScoreHistoryItem> list = await _scoreHistoryCollection
                .Find(h => h.CoupleId == coupleId)
                .ToListAsync();
            return list;
        }

        public async Task<CoupleScoreHistoryItem> AddAsync(CoupleScoreHistoryItem item)
        {
            await _scoreHistoryCollection.InsertOneAsync(item);
            return item;
        }
    }
}
