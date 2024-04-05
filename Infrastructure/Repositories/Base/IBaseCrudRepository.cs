using Core.Entities.Base;

namespace Infrastructure.Repositories.Base
{
    public interface IBaseCrudRepository<T> where T : class, IIdentifiable
    {
        void Add(T entity);
        void DeleteById(int id);
        IQueryable<T> GetAll(bool isTrackable = false);
        T GetById(int id, bool isTrackable = false);
        void Update(T entity);
        void SaveChanges();
    }
}
