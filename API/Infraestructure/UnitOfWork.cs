//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------

using Infraestructure.Interfaces;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infraestructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposedValue;
        private readonly ContextDB ContextDB;
        private IDbContextTransaction transaction;

        public UnitOfWork(ContextDB _ContextDB)
        {
            this.ContextDB = _ContextDB;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(ContextDB);
        }

        public void BeginTransaction()
        {
            transaction = this.ContextDB.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void RollbackTransaction()
        {
            transaction.RollbackAsync();
        }




        public bool SaveChanges()
        {
            return ContextDB.SaveChanges() > 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ContextDB.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
