//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using System.Linq.Expressions;

namespace Infraestructure.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Get();
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query, Expression<Func<TEntity, object>> orders, bool ascending = true);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> query, Expression<Func<TEntity, object>> predicate, bool ascending = true, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TType>> Get<TType>(Expression<Func<TEntity, TType>> select) where TType : class;
        Task<IEnumerable<TType>> Get<TType>(Expression<Func<TEntity, TType>> select, Expression<Func<TEntity, bool>> query) where TType : class;


        void Add(TEntity entity);
        void AddRange(List<TEntity> list);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entity);


        Task<TEntity> Find(int Id);
        Task<TEntity> Find(Expression<Func<TEntity, bool>> query);
        Task<TEntity> Find(Expression<Func<TEntity, bool>> query, params Expression<Func<TEntity, object>>[] includes);


        Task<IEnumerable<T>> ExecuteProcedure<T>(string nameProcedure, params object[] parameters) where T : class;

    }
}
