using System;
namespace BankingApi.EventReceiver.Contracts;

public interface IMessageHandler
{
    Task HandleAsync(EventMessage message);
}


