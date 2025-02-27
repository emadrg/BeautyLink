using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Implementations
{
    public class UnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        protected TContext Context;


        public UnitOfWork(TContext context)
        {
            Context = context;
        }


        public async Task<int> SaveChanges()
        {
            //try
            //{
            return await Context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException e)
            //{
            //    throw new Exception(e.Message, e.InnerException);
            //}
            //catch (DbUpdateException e)
            //{
            //    throw new Exception(e.Message, e.InnerException);
            //}
            //catch (RetryLimitExceededException e)
            //{
            //    throw new Exception(e.Message, e.InnerException);
            //}
            //catch (SqlException e)
            //{
            //    throw new Exception(e.Message, e.InnerException);
            //}
            //catch (Exception e)
            //{
            //    throw new Exception(e.Message, e.InnerException);
            //}
        }

        public int SaveChangesSync()
        {
            return Context.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null)
                {
                    Context.Dispose();
                    Context = null!;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
