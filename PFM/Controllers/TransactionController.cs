using Microsoft.AspNetCore.Mvc;
using PFM.Commands;
using PFM.Models;
using PFM.Models.Enums;
using PFM.Services;

namespace PFM.Controllers
{
    [ApiController]
    [Route("v1")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly ICSVService _csvService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, ICSVService csvService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _csvService =  csvService;
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactionsAsync(
                [FromQuery] string? transactionKind = null,
                [FromQuery] string? startDate = null,
                [FromQuery] string? endDate = null,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10,                
                [FromQuery] SortOrder sortOrder = SortOrder.Asc,
                [FromQuery] string? sortBy = null
            )
        {
            var transactions = await _transactionService.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);
            return Ok(transactions);
        }


     

        [HttpPost("transaction/{id}/split")]
        public IActionResult SplitTransaction()
        {
            return Ok();
        }

        

        [HttpPost("transaction/auto-categorize")]
        public IActionResult AutoCategorizeTransactions()
        {
            return Ok();
        }






        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransaction([FromRoute] string id)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost("transaction/create")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result = await _transactionService.CreateTransaction(command);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("transaction/{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string Id, [FromBody] CategorizeTransactionCommand command)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var result = await _transactionService.CategorizeTransaction(Id, command);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("transaction/{id}/delete")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] string id)
        {
            var result = await _transactionService.DeleteTransaction(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();

        }

        [HttpPost("transactions/import")]
        public async Task<IActionResult> ImportTransactionsFromCSV([FromForm] IFormFileCollection file)
        {
            var transactions = _csvService.ReadCSV<TransactionCSV>(file[0].OpenReadStream());

            return Ok(transactions);
        }
    }
}