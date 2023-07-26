using PFM.Commands;

namespace PFM.Services
{
    public interface ISplitService
    {
        Task SplitTransaction(string transactionId, List<SplitCommand> splits);
    }
}
