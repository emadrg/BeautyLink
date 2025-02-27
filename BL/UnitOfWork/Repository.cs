using Common.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BL.UnitOfWork
{
    public class Repository<TEntity, TContext> : CrudRepository<TEntity>
        where TEntity: class, new()
        where TContext: DbContext
    {
        public Repository(TContext context)
            : base(context)
        {

        }

        protected new TContext Context
        {
            get
            {
                return (TContext)base.Context;
            }
        }
    }
}
