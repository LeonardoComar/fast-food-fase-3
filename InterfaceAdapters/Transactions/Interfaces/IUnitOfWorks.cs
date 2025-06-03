namespace InterfaceAdapters.Transactions.Interfaces
{
    public interface IUnitOfWorks : IDisposable
    {
        void Begintransaction();
        void RollBack();
        void Commit();
    }
}
