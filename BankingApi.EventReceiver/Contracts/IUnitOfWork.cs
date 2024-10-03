namespace BankingApi.EventReceiver.Contracts;

public interface IUnitOfWork : IDisposable
{
    IBankAccountRepository BankAccounts { get; }
    ITransactionRepository Transactions { get; }

    Task<int> CommitAsync();
}

