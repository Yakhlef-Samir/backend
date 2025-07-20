using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCount.Domain.Entities.Budget;

namespace WeCount.Application.Common.Interfaces.Repositories
{
    public interface IBudgetRepository
    {
        Task<Budget?> GetByIdAsync(Guid Id);
        Task<IEnumerable<Budget>> GetByCoupleIdAsync(
            Guid coupleId,
            int page = 1,
            int pageSize = 20
        );
        Task<IEnumerable<Budget>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<Budget> CreateAsync(Budget budget);
        Task<bool> UpdateAsync(Budget budget);
        Task<bool> DeleteAsync(Guid Id);
    }
}

