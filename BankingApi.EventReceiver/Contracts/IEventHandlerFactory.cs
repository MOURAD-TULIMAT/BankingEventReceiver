using System;
using BankingApi.EventReceiver.Models;
namespace BankingApi.EventReceiver.Contracts;

public interface IEventHandlerFactory
{
    IEventHandler GetHandler(MessageType messageType);
}

