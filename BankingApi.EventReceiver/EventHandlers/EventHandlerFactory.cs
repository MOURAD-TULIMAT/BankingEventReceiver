using BankingApi.EventReceiver.Contracts;
using BankingApi.EventReceiver.Exeptions;
using BankingApi.EventReceiver.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BankingApi.EventReceiver.EventHandlers;

public class EventHandlerFactory : IEventHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EventHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEventHandler GetHandler(MessageType messageType)
    {

        return messageType switch
        {
            MessageType.Credit => _serviceProvider.GetRequiredService<CreditEventHandler>(),
            MessageType.Debit => _serviceProvider.GetRequiredService<DebitEventHandler>(),
            _ => throw new NonTransientException($"Handler not found for message type: {messageType}")
        };
    }
}

