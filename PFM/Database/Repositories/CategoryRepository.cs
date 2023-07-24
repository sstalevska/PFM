using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using PFM.Models;
using PFM.Database;
using PFM.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Globalization;

namespace PFM.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        CategoryDbContext _dbContext;
        public CategoryRepository(CategoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CategoryEntity>> GetCategories(string? parentcode = null)
        {
            var query = _dbContext.Categories.AsQueryable();
            

            if (!String.IsNullOrEmpty(parentcode))
            {
                query = query.Where(o => o.parentcode == parentcode);
            }

            query = query.OrderBy(o => o.code);


            var categories = await query.ToListAsync();


            return new List<CategoryEntity>(categories);
        }
    }
}
