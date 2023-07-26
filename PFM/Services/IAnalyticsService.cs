using PFM.Models;
using PFM.Models.Enums;

namespace PFM.Services
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<Analytic>> GetAnalytics(string? catcode = null, string? startDate = null, string? endDate = null, string? direction = null);

    }
}
