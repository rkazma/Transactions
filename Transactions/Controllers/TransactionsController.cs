using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transactions.Domain.Models;
using Transactions.DTOModels;
using Transactions.Service.Contracts;

namespace Transactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;
        public TransactionsController(ITransactionService transactionService, IMapper mapper, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("customer/{customerId}/account/{accountId}")]
        public async Task<IActionResult> GetTransaction([FromRoute] long customerId, [FromRoute] long accountId, [FromQuery] PaginationReqDTO pagination)
        {
            _logger.LogInformation("Accessing GetTransaction service");
            var result = await _transactionService.GetTransactions(customerId, accountId, pagination.Start, pagination.Limit);

            IEnumerable<TransactionDTO> resultDto = _mapper.Map<IEnumerable<TransactionObj>, IEnumerable<TransactionDTO>>(result);

            _logger.LogInformation("GetTransaction service ended");
            return Ok(resultDto);

        }
    }
}
