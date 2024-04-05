using Core.Entities.Base;
using Infrastructure.DataContext;
using Infrastructure.Middlewares.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base
{
    public class BaseRepository<T>(TuitterContext context) : IBaseCrudAsyncRepository<T>, IBaseCrudRepository<T> where T : class, IIdentifiable
    {
        private readonly DbSet<T> _entities = context.Set<T>();
        public void Add(T entity)
        {
            _entities.Add(entity);
            context.SaveChanges();
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public void DeleteById(int id)
        {
            var entityToDelete = _entities.FirstOrDefault(x => x.Id == id);
            if (entityToDelete is not null)
            {
                _entities.Remove(entityToDelete);
                context.SaveChanges();
            }
        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entityToDelete = await _entities.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entityToDelete is not null)
            {
                _entities.Remove(entityToDelete);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public IQueryable<T> GetAll(bool isTrackable = false)
        {
            return isTrackable ? _entities : _entities.AsNoTracking();
        }

        public T GetById(int id, bool isTrackable = false)
        {
            var entities = isTrackable ? _entities : _entities.AsNoTracking();
            return entities.FirstOrDefault(x => x.Id == id) ??
                throw new NotFoundException($"Entity with ID: {id} not found");
        }

        public async Task<T> GetByIdAsync(int id,bool isTrackable = false, CancellationToken cancellationToken = default)
        {
            var entities = isTrackable ? _entities : _entities.AsNoTracking();
            return await entities.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ??
                throw new NotFoundException($"Entity with ID: {id} not found");
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public void Update(T entity)
        {
            var existingEntity = _entities.FirstOrDefault(x => x.Id == entity.Id) ??
                throw new NotFoundException($"Entity with ID: {entity.Id} not found");

            context.Entry(existingEntity).State = EntityState.Detached;
            context.Entry(existingEntity).CurrentValues.SetValues(entity);
            context.Entry(existingEntity).State = EntityState.Modified;

            context.SaveChanges();
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _entities.FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken) ??
                throw new NotFoundException($"Entity with ID: {entity.Id} not found");

            context.Entry(existingEntity).State = EntityState.Detached;
            context.Entry(existingEntity).CurrentValues.SetValues(entity);
            context.Entry(existingEntity).State = EntityState.Modified;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
