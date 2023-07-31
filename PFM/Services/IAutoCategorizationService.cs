using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Services
{
    public interface IAutoCategorizationService
    {
        Task<List<ValidationError>> AutoCategorizeTransactions();
        RuleErrorListResponse<AutoCategorizationRule> FindMatchingRule(TransactionEntity transaction, List<AutoCategorizationRule> rules);


    }
}
