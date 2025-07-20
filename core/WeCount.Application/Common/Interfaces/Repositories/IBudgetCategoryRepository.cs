using WeCount.Domain.Entities.Budget;

namespace WeCount.Application.Common.Interfaces.Repositories;

public interface IBudgetCategoryRepository
{
    Task<Budget?> GetByCategoryAsync(string category, Guid? coupleId = null);
    Task<IEnumerable<Budget>> GetAllAsync(Guid? coupleId = null);
    Task<Budget> CreateAsync(Budget budget);
    Task<bool> UpdateAsync(Budget budget);
    Task<bool> DeleteAsync(string category, Guid? coupleId = null);
}

