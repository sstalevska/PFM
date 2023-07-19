using Microsoft.AspNetCore.Mvc;
using PFM.Commands;
using PFM.Models;
using PFM.Models.Enums;

namespace PFM.Services
{
    public interface ITransactionService
    {
      //  Task CategorizeTransaction(CategorizeTransactionCommand categorizeTransactionCommand);

        Task<Transaction> GetTransactionById(string id);
        Task<Transaction> CreateTransaction(CreateTransactionCommand command);

        Task<Transaction> CategorizeTransaction(string Id, CategorizeTransactionCommand command);

        Task<PagedSortedList<Transaction>> GetTransactions(
            string? transactionKind,
            string? startDate,
            string? endDate,
            int page,
            int pageSize,
            SortOrder sortOrder,
            string? sortBy);
        Task<bool> DeleteTransaction(string id);
    }
}
