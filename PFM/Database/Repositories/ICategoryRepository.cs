using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Database.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetCategories(string? parentcode = null);
        Task ImportCategories(List<CategoryEntity> categoryEntities);

        Task<CategoryEntity> GetCategoryByCode(string code);
        Task<bool> IsDuplicateCategory(string code);


    }
}
