using PFM.Database.Entities;
using PFM.Models;
using PFM.Models.Enums;

namespace PFM.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> GetTransactionById(string id);
        Task<TransactionEntity> CreateTransaction(TransactionEntity transactionEntity);

        Task<TransactionEntity> CategorizeTransaction(TransactionEntity transactionEntity);

        Task<bool> DeleteTransaction(string id);

        Task<PagedSortedList<TransactionEntity>> GetTransactions(
          string? transactionKind = null,
          string? startDate = null,
          string? endDate = null,
          int page = 1,
          int pageSize = 10,
          SortOrder sortOrder = SortOrder.Asc,
          string? sortBy = null);


        Task<List<TransactionEntity>> ImportTransactions(List<TransactionEntity> transactionEntities);

    }
}

