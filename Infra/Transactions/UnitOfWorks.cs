using InterfaceAdapters.Transactions.Interfaces;
using NHibernate;

namespace Infra.Transactions
{
    public class UnitOfWorks(ISession session) : IUnitOfWorks
    {
        private ITransaction _transaction;

        public void Begintransaction()
        {
            _transaction = session.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction.IsActive)
                _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            if (session.IsOpen)
            {
                session.Close();
            }
        }

        public void RollBack()
        {
            if (_transaction.IsActive)
                _transaction.Rollback();
        }
    }
}
