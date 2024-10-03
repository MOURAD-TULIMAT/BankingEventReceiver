using System.Text.Json;
using System.Text.Json.Serialization;
using BankingApi.EventReceiver.Contracts;
using BankingApi.EventReceiver.Exeptions;
using BankingApi.EventReceiver.Models;

namespace BankingApi.EventReceiver.Services;

public class MessageHandler : IMessageHandler
{
    private readonly IEventHandlerFactory _eventHandlerFactory;

    public MessageHandler(IEventHandlerFactory eventHandlerFactory)
    {
        _eventHandlerFactory = eventHandlerFactory;
    }

    public async Task HandleAsync(EventMessage message)
    {
        var eventData = DeserializeMessage(message.MessageBody);

        ValidateEventData(eventData);

        var handler = _eventHandlerFactory.GetHandler(eventData.MessageType);
        await handler.HandleAsync(eventData);
    }

    private EventData DeserializeMessage(string messageBody)
    {
        if (string.IsNullOrWhiteSpace(messageBody))
            throw new NonTransientException("Message body is empty");

        try
        {
            return JsonSerializer.Deserialize<EventData>(messageBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            }) ?? throw new NonTransientException("Deserialized message is null");
        }
        catch (JsonException ex)
        {
            throw new NonTransientException("Invalid JSON format", ex);
        }
    }

    private void ValidateEventData(EventData eventData)
    {
        if (eventData == null)
            throw new NonTransientException("Event data is null");

        if (eventData.Id == Guid.Empty)
            throw new NonTransientException("Event ID is missing");

        if (!Enum.IsDefined(typeof(MessageType), eventData.MessageType))
            throw new NonTransientException("Invalid message type");

        if (eventData.BankAccountId == Guid.Empty)
            throw new NonTransientException("Bank account ID is missing");

        if (eventData.Amount <= 0)
            throw new NonTransientException("Amount must be greater than zero");
    }
}