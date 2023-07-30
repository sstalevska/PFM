using PFM.Commands;
using PFM.Models;

namespace PFM.Services
{
    public interface ISplitService
    {
        Task<List<ValidationError>> SplitTransaction(string transactionId, List<SplitCommand> splits);
    }
}
