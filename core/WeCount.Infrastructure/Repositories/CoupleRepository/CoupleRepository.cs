using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using WeCount.Domain.Entities.Couple;
using WeCount.Infrastructure.Common;
using WeCount.Infrastructure.Interfaces;

namespace WeCount.Infrastructure.Repositories.CoupleRepository
{
    public class CoupleRepository(IMongoDbContext context) : ICoupleRepository
    {
        private readonly IMongoCollection<Couple> _couples = context.Couples;

        public Task<Couple?> GetByIdAsync(Guid id) => _couples.GetByIdAsync(id)!;

        public async Task<IEnumerable<Couple>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            return await _couples.Find(_ => true).Paginate(page, pageSize).ToListAsync();
        }

        public Task<Couple> CreateAsync(Couple couple) => _couples.CreateAsync(couple);

        public Task<bool> UpdateAsync(Couple couple) => _couples.UpdateAsync(couple);

        public Task<bool> DeleteAsync(Guid id) => _couples.DeleteAsync(id);
    }
}
