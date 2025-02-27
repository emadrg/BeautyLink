using Common.Enums;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Common.Implementations
{
    public class CrudRepository<TEntity> : IRepository<TEntity>
            where TEntity : class, new()
    {
        protected DbContext Context { get; }

        public CrudRepository(DbContext context)
        {
            query = context.Set<TEntity>().AsNoTracking();
            Context = context;
        }

        private IQueryable<TEntity> query;
        public IQueryable<TEntity> Query
        {
            get
            {
                return query;
            }
        }


        public virtual TEntity? Get(params object[] key)
        {
            return Context.Set<TEntity>().Find(key);
        }

        public virtual async Task<TEntity?> GetAsync(params object[] key)
        {
            return await Context.Set<TEntity>().FindAsync(key);
        }

        public virtual TEntity? GetWithReload(params object[] key)
        {
            var entity = Get(key);
            if (entity == null) 
                return null;
            
            Context.Entry(entity).Reload();
            return entity;
        }
        public virtual async Task<TEntity?> GetWithReloadAsync(params object[] key)
        {
            var entity = Get(key);
            if (entity == null) 
                return null;
            
            await Context.Entry(entity).ReloadAsync();
            return entity;
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }
        public virtual async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }
        public virtual void AddRange(List<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual void AttachRange(List<TEntity> entities)
        {
            Context.Set<TEntity>().AttachRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().UpdateRange(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Set<TEntity>().Remove(entity);
        }
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void ReplaceRange(IEnumerable<TEntity> oldEntities, IEnumerable<TEntity> newEntities)
        {
            if (oldEntities.Any())
            {
                Context.Set<TEntity>().RemoveRange(oldEntities);
            }
            Context.Set<TEntity>().AddRange(newEntities);
        }

        public virtual IIncludableQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> lambda)
        {
            return Context.Set<TEntity>().Include(lambda);
        }

        public virtual IQueryable<TEntity> GetByQuery(IQueryBuilder queryBuilder)
        {
            var set = Context.Model.FindEntityType(typeof(TEntity)) ?? throw new ArgumentNullException();
            var propName = set.FindPrimaryKey() != null
                ? set.FindPrimaryKey()!.Properties.FirstOrDefault()!.GetColumnName()
                : set.GetProperties().FirstOrDefault()!.GetColumnName();
            var filterCondition = queryBuilder.GetSQLFilterCondition();

            var sqlString = $"SELECT * FROM {set.GetSchema() ?? "dbo"}.{set.GetTableName() ?? set.GetViewName()}" +
                $" {(!String.IsNullOrEmpty(filterCondition) ? $"WHERE {filterCondition}" : String.Empty)}" +
                $" {(queryBuilder.Sorting != null && !String.IsNullOrEmpty(queryBuilder.Sorting.SortBy) ? $"ORDER BY {queryBuilder.Sorting.SortBy} {(queryBuilder.Sorting.SortDirection == SortDirections.Ascending ? " asc" : " desc")}"
                : queryBuilder.Paging != null && queryBuilder.Paging.Take > 0 ? $"ORDER BY {propName}" : "")}" +
                $" {(queryBuilder.Paging != null && queryBuilder.Paging.Take > 0 ? $"OFFSET {queryBuilder.Paging.Skip} ROWS FETCH NEXT {queryBuilder.Paging.Take} ROWS ONLY" : String.Empty)}";

            return Context.Set<TEntity>().FromSqlRaw(sqlString);
        }
    }
}
