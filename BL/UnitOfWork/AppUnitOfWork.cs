using Common.Implementations;
using DA.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace BL.UnitOfWork
{
    public class AppUnitOfWork: UnitOfWork<SalonContext>
    {
        public AppUnitOfWork(SalonContext context)
            : base(context)
        {
        }

        public Repository<T, SalonContext> Repository<T>()
            where T : class, new()
        {
            return new Repository<T, SalonContext>(Context);
        }

        public IQueryable<T> Queryable<T>()
            where T : class, new()
        {
            return new Repository<T, SalonContext>(Context).Query;
        }
        private IQueryable<T> ExecuteQueryProcedure<T>(string name, params SqlParameter[] args) where T: class
        {
            var query = $"EXEC {name} {string.Join(", ", args.Select(s => $"@{s.ParameterName}"))}";

            return Context.Set<T>().FromSqlRaw(query, args);
        }

        private async Task<int> ExecuteProcedure(string name, params SqlParameter[] args)
        {
            return await Context.Database.ExecuteSqlAsync($"EXEC {name} {string.Join(", ", args.Select(s => $"@{s.ParameterName}"))}");
        }
    }
}
