using System.Linq.Expressions;

namespace SMSClient.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {

        Task<TEntity?> GetById(object id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate);



        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);    

    }
}
