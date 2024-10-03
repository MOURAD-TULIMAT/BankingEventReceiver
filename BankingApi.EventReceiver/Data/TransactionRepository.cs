using System;
using BankingApi.EventReceiver.Contracts;
using BankingApi.EventReceiver.Models;
namespace BankingApi.EventReceiver.Data;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankingApiDbContext _context;

    public TransactionRepository(BankingApiDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }
}

