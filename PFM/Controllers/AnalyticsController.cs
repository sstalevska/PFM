using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PFM.Models.Enums;
using PFM.Services;
using System.Globalization;

namespace PFM.Controllers
{
    [ApiController]
    [Route("v1/spending-analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(
            ILogger<TransactionController> logger,
            ITransactionService transactionService,
            ICategoryService categoryService,
            IAnalyticsService analyticsService
            )
        {
            _logger = logger;
            _transactionService = transactionService;
            _categoryService = categoryService;
            _analyticsService = analyticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnalytics(
            [FromQuery] string? catcode = null,
            [FromQuery] string? startDate = null,
            [FromQuery] string? endDate = null, 
            [FromQuery] string? direction = null
            )
        {
            var analytics = await _analyticsService.GetAnalytics(
                                                        catcode, 
                                                        startDate,
                                                        endDate,
                                                        direction);
            return Ok(analytics);
        }
    }
}
