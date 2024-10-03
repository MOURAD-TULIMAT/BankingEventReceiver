using System;
namespace BankingApi.EventReceiver.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public MessageType MessageType { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }

    public BankAccount BankAccount { get; set; }
}

