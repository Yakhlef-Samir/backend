using WeCount.Domain.Entities.Couple;
using CoupleEntity = WeCount.Domain.Entities.Couple.Couple;
using CoupleScoreHistoryItem = WeCount.Domain.Entities.Couple.CoupleScoreHistoryItem;

namespace WeCount.Application.Common.Interfaces.Repositories
{
    public interface ICoupleRepository
    {
        Task<CoupleEntity?> GetByIdAsync(Guid Id);
        Task<IEnumerable<CoupleEntity>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<CoupleEntity> CreateAsync(CoupleEntity couple);
        Task<bool> UpdateAsync(CoupleEntity couple);
        Task<bool> DeleteAsync(Guid Id);
        Task<IEnumerable<CoupleScoreHistoryItem>> GetScoreHistoryAsync(Guid coupleId);
        Task<CoupleScoreHistoryItem> AddAsync(CoupleScoreHistoryItem coupleScoreHistory);
    }
}

