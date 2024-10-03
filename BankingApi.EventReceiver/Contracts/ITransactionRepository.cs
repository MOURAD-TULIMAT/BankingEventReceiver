using BankingApi.EventReceiver.Models;

namespace BankingApi.EventReceiver.Contracts
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
    }
}

