using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Services
{
    public interface ICategoryService 
    {
         Task<List<Category>> GetCategories(string? parentcode);
        IEnumerable<Database.Entities.CategoryEntity> ReadCSV<CategoryEntity>(Stream file);
    


    }
}
