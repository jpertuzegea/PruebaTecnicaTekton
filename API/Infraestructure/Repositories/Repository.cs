//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ContextDB ContextDB;
         
        public Repository(ContextDB _contextdb)
        {
            this.ContextDB = _contextdb;
        }
         
        public async Task<IEnumerable<T>> ExecuteProcedure<T>(string nameProcedure, params object[] parameters) where T : class
        {
            return await ContextDB.Set<T>().FromSqlRaw(nameProcedure, parameters).ToListAsync<T>();
        }
         
        public void Add(TEntity entity)
        {
            ContextDB.Set<TEntity>().Add(entity);
        }

        public void AddRange(List<TEntity> list)
        {
            ContextDB.Set<TEntity>().AddRange(list);
        }
         
        public async Task<TEntity> Find(int Id)
        {
            return await ContextDB.Set<TEntity>().FindAsync(Id);
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> query)
        {
            return await ContextDB.Set<TEntity>().FirstOrDefaultAsync(query);
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> query, params Expression<Func<TEntity, object>>[] includes)
        {
            if (!includes.Any())
            {
                throw new ArgumentException("Parameter hasn't object", "includes");
            }

            var queryable = ContextDB.Set<TEntity>().AsQueryable();

            return await includes.Aggregate(queryable, (query, include) => query.Include(include)).FirstOrDefaultAsync(query);
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await ContextDB.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TType>> Get<TType>(Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await ContextDB.Set<TEntity>().Select(select).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query)
        {
            return await ContextDB.Set<TEntity>().Where(query).ToListAsync();
        }

        public async Task<IEnumerable<TType>> Get<TType>(Expression<Func<TEntity, TType>> select, Expression<Func<TEntity, bool>> query) where TType : class
        {
            return await ContextDB.Set<TEntity>().Where(query).Select(select).ToListAsync();
        }

        public IEnumerable<TType> Get<TType>(Expression<Func<TEntity, TType>> select, Expression<Func<TEntity, bool>> query, Expression<Func<TEntity, object>> orders, bool ascending = true) where TType : class
        {
            return ascending ? ContextDB.Set<TEntity>().Where(query).OrderBy(orders).Select(select)
                 : ContextDB.Set<TEntity>().Where(query).OrderByDescending(orders).Select(select);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query, params Expression<Func<TEntity, object>>[] includes)
        {
            if (!includes.Any())
            {
                throw new ArgumentException("Parameter hasn't object", "includes");
            }

            var queryable = ContextDB.Set<TEntity>().AsQueryable();

            return await includes.Aggregate(queryable, (query, include) => query.Include(include)).Where(query).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query, Expression<Func<TEntity, object>> orders, bool ascending = true)
        {
            var queryable = ContextDB.Set<TEntity>().Where(query);

            return ascending ? await queryable.OrderBy(orders).ToListAsync()
                             : await queryable.OrderByDescending(orders).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query, Expression<Func<TEntity, object>> orders, bool ascending = true, params Expression<Func<TEntity, object>>[] includes)
        {
            if (!includes.Any())
            {
                throw new ArgumentException("Parameter hasn't object", "includes");
            }

            var queryable = ContextDB.Set<TEntity>().AsQueryable();

            return ascending ? await includes.Aggregate(queryable, (query, include) => query.Include(include)).Where(query).OrderBy(orders).ToListAsync()
                             : await includes.Aggregate(queryable, (query, include) => query.Include(include)).Where(query).OrderByDescending(orders).ToListAsync();
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> query)
        {
            return ContextDB.Set<TEntity>().Where(query);
        }

        public void Update(TEntity entity)
        {
            ContextDB.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            ContextDB.Entry(entity).State = EntityState.Deleted;
        }

        public void RemoveRange(List<TEntity> entity)
        {
            ContextDB.Set<TEntity>().RemoveRange(entity);
        }
         
    }
}
