using Microsoft.AspNetCore.Mvc;
using PFM.Commands;
using PFM.Models;
using PFM.Models.Enums;

namespace PFM.Services
{
    public interface ITransactionService
    {


        Task<Transaction> CategorizeTransaction(string Id, CategorizeTransactionCommand command);

        Task<PagedSortedList<Transaction>> GetTransactions(
              string? transactionKind,
              string? startDate,
              string? endDate,
              int page,
              int pageSize,
              SortOrder sortOrder,
              string? sortBy);
        Task<Transaction> GetTransactionById(string id);

        Task<IEnumerable<PFM.Database.Entities.TransactionEntity>> ReadCSV<TransactionEntity>(Stream file);
    }
}
