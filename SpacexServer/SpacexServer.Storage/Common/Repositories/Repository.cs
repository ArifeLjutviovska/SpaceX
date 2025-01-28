namespace SpacexServer.Storage.Common.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SpacexServer.Storage.Common.Context;
    using SpacexServer.Storage.Common.Entities;
    using System.Linq;

    public class Repository<TAggregate> where TAggregate : Entity
    {
        protected readonly ISpacexDbContext _spacexDbContext;

        protected const int MAX_DATA_COUNT = 2000;

        protected Repository(ISpacexDbContext spacexDbContext)
        {
            _spacexDbContext = spacexDbContext;
        }

        protected void Insert<TEntity>(TEntity entity) where TEntity : Entity
        {
            _spacexDbContext.Set<TEntity>().Add(entity);
        }

        protected void Update<TEntity>(TEntity entity) where TEntity : Entity
        {
            var dbSet = _spacexDbContext.Set<TEntity>();
            var trackedEntity = dbSet.Local.SingleOrDefault(e => e.Id == entity.Id);

            if (trackedEntity != null)
            {
                _spacexDbContext.Entry(trackedEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                dbSet.Attach(entity);
                _spacexDbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        protected void Delete<TEntity>(TEntity entity) where TEntity : Entity
        {
            _spacexDbContext.Set<TEntity>().Remove(entity);
        }
    }
}