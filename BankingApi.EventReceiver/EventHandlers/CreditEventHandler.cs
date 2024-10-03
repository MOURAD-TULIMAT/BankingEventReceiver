using BankingApi.EventReceiver.Contracts;
using BankingApi.EventReceiver.Exeptions;
using BankingApi.EventReceiver.Models;

namespace BankingApi.EventReceiver.EventHandlers;

public class DebitEventHandler : IEventHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DebitEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(EventData eventData)
    {
        var bankAccount = await _unitOfWork.BankAccounts.GetByIdAsync(eventData.BankAccountId);
        if (bankAccount == null)
            throw new NonTransientException($"Bank account {eventData.BankAccountId} not found");

        bankAccount.Balance -= eventData.Amount;

        // Log the transaction
        await _unitOfWork.Transactions.AddAsync(new Transaction
        {
            Id = Guid.NewGuid(),
            BankAccountId = eventData.BankAccountId,
            Amount = eventData.Amount,
            MessageType = MessageType.Debit,
            Timestamp = DateTime.UtcNow
        });

        await _unitOfWork.CommitAsync();
    }
}

