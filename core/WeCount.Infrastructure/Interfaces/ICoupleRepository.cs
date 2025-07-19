using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCount.Domain.Entities.Couple;

namespace WeCount.Infrastructure.Interfaces
{
    public interface ICoupleRepository
    {
        Task<Couple?> GetByIdAsync(Guid Id);
        Task<IEnumerable<Couple>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<Couple> CreateAsync(Couple couple);
        Task<bool> UpdateAsync(Couple couple);
        Task<bool> DeleteAsync(Guid Id);
    }
}
