
using PFM.Database;
using PFM.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Globalization;
using PFM.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace PFM.Database.Repositories
{
    public class SplitRepository : ISplitRepository
    {
        PfmDbContext _dbContext;
        public SplitRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteSplitsByTransactionId(string transactionId)
        {
            var existingSplits = await _dbContext.Splits
                .Where(s => s.transactionid == transactionId)
                .ToListAsync();

            _dbContext.Splits.RemoveRange(existingSplits);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddSplits(List<SplitEntity> splits)
        {
            _dbContext.Splits.AddRange(splits);
            await _dbContext.SaveChangesAsync();
        }

    }
}
