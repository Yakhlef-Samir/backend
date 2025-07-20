using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCount.Domain.Entities;

namespace WeCount.Application.Common.Interfaces.Repositories
{
    public interface IDebtRepository
    {
        Task<Debt?> GetByIdAsync(Guid Id);
        Task<IEnumerable<Debt>> GetByCoupleIdAsync(Guid coupleId, int page = 1, int pageSize = 20);
        Task<IEnumerable<Debt>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<Debt> CreateAsync(Debt debt);
        Task<bool> UpdateAsync(Debt debt);
        Task<bool> DeleteAsync(Guid Id);
    }
}

