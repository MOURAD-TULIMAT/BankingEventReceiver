using BankingApi.EventReceiver.Contracts;
using BankingApi.EventReceiver.Exeptions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace BankingApi.EventReceiver;

public class MessageWorker
{
    private readonly IServiceBusReceiver _serviceBusReceiver;
    private readonly IMessageHandler _messageHandler;
    private readonly ILogger<MessageWorker> _logger;
    private bool _stopRequested = false;

    // Polly retry policy
    private readonly AsyncRetryPolicy _retryPolicy;

    public MessageWorker(
        IServiceBusReceiver serviceBusReceiver,
        IMessageHandler messageHandler,
        ILogger<MessageWorker> logger)
    {
        _serviceBusReceiver = serviceBusReceiver;
        _messageHandler = messageHandler;
        _logger = logger;

        // here I configured the retrying with exponential sleep duration
        _retryPolicy = Policy
            .Handle<TransientException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(5, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Retry {RetryCount} after {TimeSpan} seconds due to transient error.", retryCount, timeSpan.TotalSeconds);
                });
    }

    public async Task Start()
    {
        while (!_stopRequested)
        {
            var message = await _serviceBusReceiver.Peek();
            if (message == null)
            {
                _logger.LogInformation("No messages in the queue. Waiting 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10));
                continue;
            }

            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await _messageHandler.HandleAsync(message);
                    await _serviceBusReceiver.Complete(message);
                });
            }
            catch (NonTransientException ex)
            {
                _logger.LogError(ex, "Non-transient error processing message {MessageId}", message.Id);
                await _serviceBusReceiver.MoveToDeadLetter(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing message {MessageId}", message.Id);
                await _serviceBusReceiver.MoveToDeadLetter(message);
            }
        }
    }

    public async Task StopAsync()
    {
        _stopRequested = true;
    }
}
