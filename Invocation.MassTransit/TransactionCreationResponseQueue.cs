using Common.Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Transactions.Common;
using Transactions.Service.Contracts;

namespace Invocation.MassTransit
{
    public class TransactionCreationResponseQueue : IConsumer<EventMessage>
    {
        private readonly ITransactionService _transactionService;
        private readonly string rabbitMQAddress;
        private readonly ILogger<TransactionCreationResponseQueue> _logger;
        public TransactionCreationResponseQueue(ITransactionService transactionService, IConfiguration configuration, ILogger<TransactionCreationResponseQueue> logger)
        {
            _transactionService = transactionService;
            rabbitMQAddress = configuration[Constants.QUEUE_SETTINGS_ADDRESS];
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<EventMessage> context)
        {
            _logger.LogInformation("Accessing TransactionCreationResponseQueue consumer");
            EventMessage? request;
            var responseEndPoint = await context.GetSendEndpoint(new Uri(Path.Combine(rabbitMQAddress, context.Message.EndPoint)));
            try
            {
                var result = await _transactionService.CreateTransaction(context.Message);
                request = new EventMessage { AccountId = context.Message.AccountId, CustomerId = context.Message.CustomerId, ResultCode = DBErrorCode.SUCCESS };

                _logger.LogInformation("Transaction Creation went successful");

                await responseEndPoint.Send<EventMessage>(request);
            }
            catch
            {
                _logger.LogError("Transaction Creation failed");
                request = new EventMessage { AccountId = context.Message.AccountId, CustomerId = context.Message.CustomerId, ResultCode = DBErrorCode.TRANSACTION_CREATION_FAILED };

                await responseEndPoint.Send<EventMessage>(request);

                throw;
            }
        }
    }
}