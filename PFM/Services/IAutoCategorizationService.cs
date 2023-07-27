using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Services
{
    public interface IAutoCategorizationService
    {
        Task AutoCategorizeTransactions();
    
    }
}
