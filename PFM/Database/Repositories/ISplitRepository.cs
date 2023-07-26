using PFM.Database.Entities;

namespace PFM.Database.Repositories
{
    public interface ISplitRepository
    {
        Task AddSplits(List<SplitEntity> splits);
        Task DeleteSplitsByTransactionId(string transactionId);
    }
}
