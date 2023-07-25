using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using PFM.Models;
using PFM.Models.Enums;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;

namespace PFM.Database.Repositories
{
    
    public class TransactionRepository : ITransactionRepository
    {
        PfmDbContext _dbContext;
        public TransactionRepository(PfmDbContext dbContext) { 
            _dbContext= dbContext;
        }

        public async Task<TransactionEntity> CategorizeTransaction(TransactionEntity transactionEntity)
        {
            var trans = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id.Equals(transactionEntity.Id));
            trans.CatCode = transactionEntity.CatCode; 

            await _dbContext.SaveChangesAsync();

            return transactionEntity;
        }

        public async Task<TransactionEntity> CreateTransaction(TransactionEntity transactionEntity)
        {
            _dbContext.Transactions.Add(transactionEntity);

            await _dbContext.SaveChangesAsync();

            return transactionEntity;
        }

        public async Task<bool> DeleteTransaction(string id)
        {
            var transaction = await GetTransactionById(id);

            if (transaction == null)
            {
                return false;
            }

            _dbContext.Remove(transaction);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionEntity> GetTransactionById(string id)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(
            string? transactionKind = null, 
            string? startDate = null, 
            string? endDate = null, 
            int page = 1, 
            int pageSize = 10, 
            SortOrder sortOrder = SortOrder.Asc, 
            string? sortBy = null)
        {
            var query = _dbContext.Transactions.AsQueryable();
            var totalCount = query.Count();
            var totalPages = (int) Math.Ceiling(totalCount*1.0 / pageSize);

            if (!String.IsNullOrEmpty(startDate))
            {
                DateTime parsedStartDate = DateTime.Parse(startDate);
                query = query.Where(o => o.Date >= parsedStartDate);
            }

            if (!String.IsNullOrEmpty(endDate))
            {
                DateTime parsedEndDate = DateTime.Parse(endDate);
                query = query.Where(o => o.Date <= parsedEndDate);
            }

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "id":
                        query = sortOrder == SortOrder.Asc 
                            ? query.OrderBy(x => x.Id) 
                            : query.OrderByDescending(x => x.Id);
                        break;
                    case "beneficiary-name":
                        query = sortOrder == SortOrder.Asc 
                            ? query.OrderBy(x => x.BeneficiaryName) 
                            : query.OrderByDescending(x => x.BeneficiaryName);
                        break;
                    case "amount":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.Amount)
                            : query.OrderByDescending(x => x.Amount);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.Description)
                            : query.OrderByDescending(x => x.Description);
                        break;
                    case "kind":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.Kind)
                            : query.OrderByDescending(x => x.Kind);
                        break;
                    case "currency":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.Currency)
                            : query.OrderByDescending(x => x.Currency);
                        break;
                    case "direction":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.Direction)
                            : query.OrderByDescending(x => x.Direction);
                        break;
                    case "mcc":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.Mcc)
                            : query.OrderByDescending(x => x.Mcc);
                        break;
                    case "catcode":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.CatCode)
                            : query.OrderByDescending(x => x.CatCode);
                        break;
                    default:
                    case "date":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.Date)
                            : query.OrderByDescending(x => x.Date);
                        break;


                }
            }
            else
            {
                query = query.OrderBy(x => x.Date);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            if (!String.IsNullOrEmpty(transactionKind))
            {
                switch (transactionKind)
                {
                    case "dep":
                        query = query.Where(o => o.Kind == TransactionKind.dep);
                        break;
                    case "wdw":
                        query = query.Where(o => o.Kind == TransactionKind.wdw);
                        break;
                    case "pmt":
                        query = query.Where(o => o.Kind == TransactionKind.pmt);
                        break;
                    case "fee":
                        query = query.Where(o => o.Kind == TransactionKind.fee);
                        break;
                    case "inc":
                        query = query.Where(o => o.Kind == TransactionKind.inc);
                        break;
                    case "rev":
                        query = query.Where(o => o.Kind == TransactionKind.rev);
                        break;
                    case "adj":
                        query = query.Where(o => o.Kind == TransactionKind.adj);
                        break;
                    case "lnd":
                        query = query.Where(o => o.Kind == TransactionKind.lnd);
                        break;
                    case "lnr":
                        query = query.Where(o => o.Kind == TransactionKind.lnr);
                        break;
                    case "fcx":
                        query = query.Where(o => o.Kind == TransactionKind.fcx);
                        break;
                    case "aop":
                        query = query.Where(o => o.Kind == TransactionKind.aop);
                        break;
                    case "acl":
                        query = query.Where(o => o.Kind == TransactionKind.acl);
                        break;
                    case "spl":
                        query = query.Where(o => o.Kind == TransactionKind.spl);
                        break;
                    case "sal":
                        query = query.Where(o => o.Kind == TransactionKind.sal);
                        break;
                    default:
                    case null:
                        query = query.Where(o => true);
                        break;

                }

            }
            else
            {
                query = query.Where(o => true);
            }

            var transactions = await query.ToListAsync();

            
                return new PagedSortedList<TransactionEntity>
                {
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    Items = transactions
                };
            

            
        }

        public async Task<List<TransactionEntity>> ImportTransactions(List<TransactionEntity> transactionEntities)
        {
            await _dbContext.Transactions.AddRangeAsync(transactionEntities);
            await _dbContext.SaveChangesAsync();



            return transactionEntities;
        }
    }
}
