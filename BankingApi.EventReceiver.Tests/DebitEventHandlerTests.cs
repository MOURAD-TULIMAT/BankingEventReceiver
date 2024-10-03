using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using BankingApi.EventReceiver;
using BankingApi.EventReceiver.Contracts;
using BankingApi.EventReceiver.EventHandlers;
using BankingApi.EventReceiver.Models;
using BankingApi.EventReceiver.Exeptions;
using Microsoft.Extensions.DependencyInjection;
using BankingApi.EventReceiver.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace BankingApi.EventReceiver.Tests;
public class DebitEventHandlerTests
{
    [Fact]
    public async Task HandleAsync_NegativeAmount_ThrowsNonTransientException()
    {
        // Arrange
        var bankAccountId = Guid.NewGuid();
        var amount = -50m;
        var bankAccount = new BankAccount { Id = bankAccountId, Balance = 200m };

        var bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
        bankAccountRepositoryMock.Setup(repo => repo.GetByIdAsync(bankAccountId))
            .ReturnsAsync(bankAccount);

        var transactionRepositoryMock = new Mock<ITransactionRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.BankAccounts).Returns(bankAccountRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.Transactions).Returns(transactionRepositoryMock.Object);

        var eventHandlerFactory = new Mock<IEventHandlerFactory>();

        var messageHandler = new MessageHandler(eventHandlerFactory.Object);

        var eventData = new EventData
        {
            Id = Guid.NewGuid(),
            MessageType = MessageType.Debit,
            BankAccountId = bankAccountId,
            Amount = amount
        };

        var eventmessage = new EventMessage()
        {
            Id = Guid.NewGuid(),
            MessageBody = JsonSerializer.Serialize(eventData),
            ProcessingCount = 0,
        };

        // Act & Assert
        await Assert.ThrowsAsync<NonTransientException>(() => messageHandler.HandleAsync(eventmessage));
    }
}
