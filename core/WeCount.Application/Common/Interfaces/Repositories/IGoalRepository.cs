using WeCount.Domain.Entities;

namespace WeCount.Application.Common.Interfaces.Repositories
{
    public interface IGoalRepository
    {
        Task<Goal?> GetByIdAsync(Guid Id);
        Task<IEnumerable<Goal>> GetByCoupleIdAsync(Guid coupleId, int page = 1, int pageSize = 20);
        Task<IEnumerable<Goal>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<Goal> CreateAsync(Goal goal);
        Task<bool> UpdateAsync(Goal goal);
        Task<bool> DeleteAsync(Guid Id);
    }
}

