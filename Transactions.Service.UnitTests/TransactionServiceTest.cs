using Common.Contracts;
using Moq;
using Transactions.DataAccess.Contracts;
using Transactions.Domain.Models;
using Transactions.Service.Contracts;

namespace Transactions.Service.UnitTests
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private Mock<ITransactionRepository> transactionRepositoryMock;
        private ITransactionService transactionService;

        [SetUp]
        public void Setup()
        {
            transactionRepositoryMock = new Mock<ITransactionRepository>();
            transactionService = new TransactionService(transactionRepositoryMock.Object);
        }

        [Test]
        public async Task GetTransactions_ReturnsTransactions()
        {
            // Arrange
            long customerId = 1001;
            long accountId = 1;
            int start = 0;
            int limit = 10;
            var expectedTransactions = new List<TransactionObj>
            {
                new TransactionObj { AccountId = 1, Amount = 100, CustomerId = 1001, TransactionId = 1 },
                new TransactionObj { AccountId = 1, Amount = 1000, CustomerId = 1001, TransactionId = 2 },
                // Add more TransactionObj as needed
            };
            transactionRepositoryMock
                .Setup(repo => repo.GetTransactions(customerId, accountId, start, limit))
                .ReturnsAsync(expectedTransactions);

            // Act
            var transactions = await transactionService.GetTransactions(customerId, accountId, start, limit);

            // Assert
            Assert.IsNotNull(transactions);
            Assert.AreEqual(expectedTransactions.Count, ((List<TransactionObj>)transactions).Count); // Ensure correct count
            // Add further assertions as needed for the properties of the transactions
        }

        [Test]
        public async Task CreateTransaction_ReturnsTransactionId()
        {
            // Arrange
            var transactionMessage = new EventMessage { AccountId = 1, CustomerId = 1001, InitialCredit = 1000 };
            int expectedTransactionId = 1234;
            transactionRepositoryMock
                .Setup(repo => repo.CreateTransaction(transactionMessage))
                .ReturnsAsync(expectedTransactionId);

            // Act
            var transactionId = await transactionService.CreateTransaction(transactionMessage);

            // Assert
            Assert.AreEqual(expectedTransactionId, transactionId);
        }

        [Test]
        public async Task GetTransactions_ReturnsTransactionsForSpecificCustomerAndAccount()
        {
            // Arrange
            long customerId = 1001;
            long accountId = 456;
            int start = 0;
            int limit = 10;
            var expectedTransactions = new List<TransactionObj>
    {
        new TransactionObj { CustomerId = customerId, AccountId = accountId, Amount = 100, TransactionId = 1 },
        new TransactionObj { CustomerId = customerId, AccountId = accountId, Amount = 1000, TransactionId = 2 },
        new TransactionObj { CustomerId = 1002, AccountId = 480, Amount = 500, TransactionId = 3 },
        new TransactionObj { CustomerId = 1002, AccountId = 480, Amount = 1000, TransactionId = 4 }
    };

            transactionRepositoryMock
                .Setup(repo => repo.GetTransactions(It.IsAny<long>(), It.IsAny<long>(), start, limit))
                .ReturnsAsync((long custId, long accId, int st, int lim) =>
                    expectedTransactions.Where(t => t.CustomerId == customerId && t.AccountId == accountId)
                );

            // Act
            var transactions = await transactionService.GetTransactions(customerId, accountId, start, limit);

            // Assert
            Assert.IsNotNull(transactions);
            Assert.AreNotEqual(expectedTransactions.Count, transactions.Count()); // Ensure correct count

            foreach (var transaction in transactions)
            {
                Assert.AreEqual(customerId, transaction.CustomerId);
                Assert.AreEqual(accountId, transaction.AccountId);


            }
        }
    }
}