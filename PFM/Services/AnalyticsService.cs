using AutoMapper;
using PFM.Database.Repositories;
using PFM.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PFM.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ITransactionRepository _transactionRepository;

        public AnalyticsService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Analytic>> GetAnalytics(string? catcode = null, string? startDate = null, string? endDate = null, string? direction = null)
        {
            // Implement the logic to retrieve and aggregate the spending analytics
            // Use the provided filters (catcode, startDate, endDate, direction) to query the transactions and calculate totals
            // Return the results as a collection of Analytics objects

            // Example:
            var analytics = await _transactionRepository.GetAnalyticsByCategory(catcode, startDate, endDate, direction);
            return analytics;
        }


    }
}
