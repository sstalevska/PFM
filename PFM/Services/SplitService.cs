using PFM.Database.Repositories;

namespace PFM.Services
{
    public class SplitService : ISplitService
    {
        ISplitRepository _splitRepository;
        public SplitService(ISplitRepository splitRepository)
        {
            _splitRepository = splitRepository;
        }
    }
}
