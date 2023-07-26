using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using PFM.Models;
using PFM.Database;
using PFM.Models.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PFM.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        PfmDbContext _dbContext;
        public CategoryRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CategoryEntity>> GetCategories(string? parentcode = null)
        {
            var query = _dbContext.Categories.AsQueryable();

            if (parentcode == null)
            {
                // Filtriranje samo na main categories
                query = query.Where(o => o.parentcode == null || !Regex.IsMatch(o.parentcode, "^[A-Z]+$"));
            }
            else
            {
                //Filtriranje na subkategorii spored parentcode
                query = query.Where(o => o.parentcode == parentcode);
            }

            query = query.OrderBy(o => o.code);

            var categories = await query.ToListAsync();

            return new List<CategoryEntity>(categories);
        }

        public async Task ImportCategories(List<CategoryEntity> categoryEntities)
        {
             
            categoryEntities.ForEach(n => _dbContext.Categories.Add(n));

            await _dbContext.SaveChangesAsync();
        }
        public async Task<CategoryEntity> GetCategoryByCode(string code)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.code == code);
        }
    }
}
