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
            var trans = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.id.Equals(transactionEntity.id));
            trans.catcode= transactionEntity.catcode; 

            await _dbContext.SaveChangesAsync();

            return transactionEntity;
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
                query = query.Where(o => o.date >= parsedStartDate);
            }

            if (!String.IsNullOrEmpty(endDate))
            {
                DateTime parsedEndDate = DateTime.Parse(endDate);
                query = query.Where(o => o.date <= parsedEndDate);
            }

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "id":
                        query = sortOrder == SortOrder.Asc 
                            ? query.OrderBy(x => x.id) 
                            : query.OrderByDescending(x => x.id);
                        break;
                    case "beneficiary-name":
                        query = sortOrder == SortOrder.Asc 
                            ? query.OrderBy(x => x.beneficiaryname) 
                            : query.OrderByDescending(x => x.beneficiaryname);
                        break;
                    case "amount":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.amount)
                            : query.OrderByDescending(x => x.amount);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.description)
                            : query.OrderByDescending(x => x.description);
                        break;
                    case "kind":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.kind)
                            : query.OrderByDescending(x => x.kind);
                        break;
                    case "currency":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.currency)
                            : query.OrderByDescending(x => x.currency);
                        break;
                    case "direction":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.direction)
                            : query.OrderByDescending(x => x.direction);
                        break;
                    case "mcc":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.mcc)
                            : query.OrderByDescending(x => x.mcc);
                        break;
                    case "catcode":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.catcode)
                            : query.OrderByDescending(x => x.catcode);
                        break;
                    default:
                    case "date":
                        query = sortOrder == SortOrder.Asc
                            ? query.OrderBy(x => x.date)
                            : query.OrderByDescending(x => x.date);
                        break;


                }
            }
            else
            {
                query = query.OrderBy(x => x.date);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            if (!String.IsNullOrEmpty(transactionKind))
            {
                switch (transactionKind)
                {
                    case "dep":
                        query = query.Where(o => o.kind == TransactionKind.dep);
                        break;
                    case "wdw":
                        query = query.Where(o => o.kind == TransactionKind.wdw);
                        break;
                    case "pmt":
                        query = query.Where(o => o.kind == TransactionKind.pmt);
                        break;
                    case "fee":
                        query = query.Where(o => o.kind == TransactionKind.fee);
                        break;
                    case "inc":
                        query = query.Where(o => o.kind == TransactionKind.inc);
                        break;
                    case "rev":
                        query = query.Where(o => o.kind == TransactionKind.rev);
                        break;
                    case "adj":
                        query = query.Where(o => o.kind == TransactionKind.adj);
                        break;
                    case "lnd":
                        query = query.Where(o => o.kind == TransactionKind.lnd);
                        break;
                    case "lnr":
                        query = query.Where(o => o.kind == TransactionKind.lnr);
                        break;
                    case "fcx":
                        query = query.Where(o => o.kind == TransactionKind.fcx);
                        break;
                    case "aop":
                        query = query.Where(o => o.kind == TransactionKind.aop);
                        break;
                    case "acl":
                        query = query.Where(o => o.kind == TransactionKind.acl);
                        break;
                    case "spl":
                        query = query.Where(o => o.kind == TransactionKind.spl);
                        break;
                    case "sal":
                        query = query.Where(o => o.kind == TransactionKind.sal);
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
        public async Task<TransactionEntity> GetTransactionById(string id)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(x => x.id.Equals(id));
        }

        public async Task ImportTransactions(List<TransactionEntity> transactionEntities)
        {
            //await _dbContext.Transactions.AddRangeAsync(transactionEntities);
            transactionEntities.ForEach(n => _dbContext.Transactions.Add(n));

            await _dbContext.SaveChangesAsync();

           // return transactionEntities;
        }

        
    }
}
