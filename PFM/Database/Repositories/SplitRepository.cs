
using PFM.Database;
using PFM.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Globalization;

namespace PFM.Database.Repositories
{
    public class SplitRepository : ISplitRepository
    {
        SplitDbContext _dbContext;
        public SplitRepository(SplitDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
