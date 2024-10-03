namespace BankingApi.EventReceiver.Contracts;

public interface IEventHandler
{
    Task HandleAsync(EventData eventData);
}