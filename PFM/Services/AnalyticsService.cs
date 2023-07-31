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

        public async Task<AnalyticListResponse> GetAnalytics(string? catcode = null, string? startDate = null, string? endDate = null, string? direction = null)
        {
           var response = await _transactionRepository.GetAnalyticsByCategory(catcode, startDate, endDate, direction);
            return response;
        }


    }
}
