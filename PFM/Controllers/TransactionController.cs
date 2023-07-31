using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using PFM.Commands;
using PFM.Database.Entities;
using PFM.Models;
using PFM.Models.Enums;
using PFM.Services;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using CsvHelper.Configuration;
using System;


namespace PFM.Controllers
{
    [ApiController]
    [Route("v1/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ISplitService _splitService;
        private readonly IAutoCategorizationService _autocategorizationService ;

        public TransactionController(
            ITransactionService transactionService, ISplitService splitService, IAutoCategorizationService autocategorizationService)
        {
            _transactionService = transactionService;
            _splitService = splitService;
            _autocategorizationService = autocategorizationService;
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
            

               var errors =  await _autocategorizationService.AutoCategorizeTransactions();
            if (errors.Count > 0)
            {
                var response = new
                {
                    errors = errors
                };
                return BadRequest(response);
            }
            return Ok("Transactions have been auto-categorized successfully.");
            
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
            var e = new List<ValidationError>();
          
            if (!IsFileCsvFormat(file))
            {
                e.Add(new ValidationError("file", "invalid-format", "The file provided is not in csv format."));
                var response = new
                {
                    errors = e
                };
                return BadRequest(response);
            }
            try
            {
                await _transactionService.ReadCSV<TransactionCSVCommand>(file.OpenReadStream());
                return Ok("Transactions imported successfully.");
            }
            catch
            {
                e.Add(new ValidationError("file", "invalid-data", "The file provided doesn't contain required data fields."));
                return BadRequest(e);
;            }



        }



        public bool IsFileCsvFormat(IFormFile file)
           {
            var validExtensions = new[] { ".csv" };
            var fileExtension = Path.GetExtension(file.FileName);
            if (validExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
                return false;
        }




       


    }



}




