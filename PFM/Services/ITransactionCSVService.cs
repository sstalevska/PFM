using PFM.Models;

namespace PFM.Services
{
    public interface ITransactionCSVService
    {
        public IEnumerable<Transaction> ReadCSV<Transaction>(Stream file);

    }
}
