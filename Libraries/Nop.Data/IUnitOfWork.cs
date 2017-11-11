using System;
using System.Data.Entity;

namespace Nop.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContext Context { get; }
        DbContextTransaction Transaction { get; }
        void Flush();
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
    }
}
