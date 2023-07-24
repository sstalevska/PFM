using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Database.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetCategories(string? parentcode = null);

    }
}
