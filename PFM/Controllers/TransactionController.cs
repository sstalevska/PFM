using Microsoft.AspNetCore.Mvc;
using PFM.Commands;
using PFM.Database.Entities;
using PFM.Models;
using PFM.Models.Enums;
using PFM.Services;
using System.Runtime.Intrinsics.X86;

namespace PFM.Controllers
{
    [ApiController]
    [Route("v1/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ISplitService _splitService;

        public TransactionController(ITransactionService transactionService, ISplitService splitService)
        {
            _transactionService = transactionService;
            _splitService = splitService;
        }

        [HttpGet]
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
            var response = await _transactionService.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);

            if(response.Errors.Count() >0)
            {
                return BadRequest(new { errors = response.Errors });
            }
            return Ok(response.Data);
        }


        [HttpPost("{id}/split")]
        public async Task<IActionResult> SplitTransaction(string id, [FromBody] List<SplitCommand> splits)
        {
            var errors =  await _splitService.SplitTransaction(id, splits);
           

            if (errors.Count > 0)
            {
                var response = new
                {
                    errors = errors
                };
                return BadRequest(response);
            }
            return Ok("Splits added successfully.");
        }



        [HttpPost("auto-categorize")]
        public async Task<IActionResult> AutoCategorizeTransactions()
        {
            try
            {
                await _transactionService.AutoCategorizeTransactions();

                return Ok("Transactions have been auto-categorized successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during auto-categorization: {ex.Message}");
            }
        }




        [HttpPost("{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string Id, [FromBody] CategorizeTransactionCommand command)
        {
           
            var result = await _transactionService.CategorizeTransaction(Id, command);
           if(result.Errors.Count > 0)
            {
                var r = new
                {
                    errors = result.Errors
                };
                return BadRequest(r);
            }
            return Ok(result.Result);
        }



        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsFromCSV([FromForm] IFormFile file)
        {
            var transactions = await _transactionService.ReadCSV<TransactionCSVCommand>(file.OpenReadStream());

            return Ok("Transactions imported successfully.");
        }




    }




}