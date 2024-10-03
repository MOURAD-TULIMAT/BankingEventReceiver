using Xunit;
using Moq;
using BankingApi.EventReceiver;
using BankingApi.EventReceiver.Contracts;
using BankingApi.EventReceiver.EventHandlers;
using BankingApi.EventReceiver.Models;
using BankingApi.EventReceiver.Exeptions;

namespace BankingApi.EventReceiver.Tests;
public class CreditEventHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldIncreaseBalance()
    {
        // Arrange
        var bankAccountId = Guid.NewGuid();
        var amount = 100m;
        var bankAccount = new BankAccount { Id = bankAccountId, Balance = 200m };

        var bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
        bankAccountRepositoryMock.Setup(repo => repo.GetByIdAsync(bankAccountId))
            .ReturnsAsync(bankAccount);

        var transactionRepositoryMock = new Mock<ITransactionRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.BankAccounts).Returns(bankAccountRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.Transactions).Returns(transactionRepositoryMock.Object);

        var handler = new CreditEventHandler(unitOfWorkMock.Object);
        var eventData = new EventData
        {
            Id = Guid.NewGuid(),
            MessageType = MessageType.Credit,
            BankAccountId = bankAccountId,
            Amount = amount
        };

        // Act
        await handler.HandleAsync(eventData);

        // Assert
        Assert.Equal(300m, bankAccount.Balance);
        transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }
    [Fact]
    public async Task HandleAsync_BankAccountNotFound_ThrowsNonTransientException()
    {
        // Arrange
        var bankAccountId = Guid.NewGuid();
        var amount = 100m;

        var bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
        bankAccountRepositoryMock.Setup(repo => repo.GetByIdAsync(bankAccountId))
            .ReturnsAsync((BankAccount?)null);

        var transactionRepositoryMock = new Mock<ITransactionRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.BankAccounts).Returns(bankAccountRepositoryMock.Object);
        unitOfWorkMock.Setup(uow => uow.Transactions).Returns(transactionRepositoryMock.Object);

        var handler = new CreditEventHandler(unitOfWorkMock.Object);
        var eventData = new EventData
        {
            Id = Guid.NewGuid(),
            MessageType = MessageType.Credit,
            BankAccountId = bankAccountId,
            Amount = amount
        };

        // Act & Assert
        await Assert.ThrowsAsync<NonTransientException>(() => handler.HandleAsync(eventData));
    }
}
