
using PFM.Database;
using PFM.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Globalization;

namespace PFM.Database.Repositories
{
    public class SplitRepository : ISplitRepository
    {
        PfmDbContext _dbContext;
        public SplitRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
