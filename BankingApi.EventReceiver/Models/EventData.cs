using BankingApi.EventReceiver.Models;

namespace BankingApi.EventReceiver;
// this class represents the deserialized body of the event message
public class EventData
{
    public Guid Id { get; set; }
    public MessageType MessageType { get; set; }
    public Guid BankAccountId { get; set; }
    public decimal Amount { get; set; }
}

