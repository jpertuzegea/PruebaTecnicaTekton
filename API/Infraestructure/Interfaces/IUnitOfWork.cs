//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

namespace Infraestructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        bool SaveChanges();
         
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
