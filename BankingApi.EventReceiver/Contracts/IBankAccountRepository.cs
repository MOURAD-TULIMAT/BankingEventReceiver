using System;
namespace BankingApi.EventReceiver.Contracts;

public interface IBankAccountRepository
{
    Task UpdateAsync(BankAccount bankAccount);
    Task<BankAccount?> GetByIdAsync(Guid id);
}

