using System;
namespace BankingApi.EventReceiver.Data;

public class BankAccountRepository
{
    private readonly BankingApiDbContext _context;

    public BankAccountRepository(BankingApiDbContext context)
    {
        _context = context;
    }

    public async Task<BankAccount?> GetByIdAsync(Guid id)
    {
        return await _context.BankAccounts.FindAsync(id);
    }

    public Task UpdateAsync(BankAccount bankAccount)
    {
        _context.BankAccounts.Update(bankAccount);
        return Task.CompletedTask;
    }
}

