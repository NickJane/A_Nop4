using System;
using System.Data.Entity;

namespace Nop.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _context;
        private DbContextTransaction _transacton;
        private TransactionStatus _transactionStatus;
        private bool _disposed = false;
        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }
        public TransactionStatus TransactionStatus
        {
            get { return _transactionStatus; }
        }
        public IDbContext Context
        {
            get { return _context; }
        }

        public DbContextTransaction Transaction
        {
            get { return _transacton; }
        }
        private bool HasTransactionOpen()
        {
            if (Transaction != null && TransactionStatus != TransactionStatus.WasCommited && TransactionStatus != TransactionStatus.WasRolledBack && TransactionStatus != TransactionStatus.UnActive)
            {
                return true;
            }
            return false;
        }
        public void Flush()
        {
            if (Context != null)
            {
                Context.SaveChanges();
            }
        }
        public void BeginTransaction()
        {
            if (!HasTransactionOpen())
            {
                _transacton = _context.Database.BeginTransaction();
                _transactionStatus = TransactionStatus.Active;
            }
        }
        public void CommitTransaction()
        {

            try
            {
                if (Context != null)
                {
                    if (HasTransactionOpen())
                    {
                        Transaction.Commit();
                        Transaction.Dispose();
                        _transactionStatus = TransactionStatus.WasCommited;
                    }

                }

            }
            catch (Exception e)
            {
                RollBackTransaction();
                throw e;
            }
        }
        public void RollBackTransaction()
        {
            if (HasTransactionOpen())
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Context.Dispose();
                UnitOfWorkFactory.DisposeUnitOfWork();
                _transactionStatus = TransactionStatus.WasRolledBack;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Context != null)
                    {
                        if (Transaction != null)
                        {
                            Transaction.Dispose();
                        }
                        _transacton = null;
                        _context.Dispose();
                        _context = null;
                        UnitOfWorkFactory.DisposeUnitOfWork();
                    }
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
