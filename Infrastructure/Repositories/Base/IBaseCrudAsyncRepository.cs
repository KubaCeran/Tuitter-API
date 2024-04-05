using Core.Entities.Base;

namespace Infrastructure.Repositories.Base
{
    public interface IBaseCrudAsyncRepository<T> where T : class, IIdentifiable
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, bool isTrackable = false, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
