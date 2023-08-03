using BankManagmentDB.Entity;



namespace BankManagmentDB
{
    public interface IUnitOfWork
    {
        IRepository<AccountType> AccountTypesRepository { get; }
        IRepository<PaymentMethod> PaymentMethodsRepository { get; }
        IRepository<BankAccount> BankAccountsRepository { get; }
        IRepository<BankTransaction> BankTransactionsRepository { get; }
        IRepository<BankAccountPosting> BankAccountPostingsRepository { get; }

        Task SaveAsync();
    }
}
