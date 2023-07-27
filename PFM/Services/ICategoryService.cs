using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Services
{
    public interface ICategoryService 
    {
         Task<List<Category>> GetCategories(string? parentcode);
        Task<IEnumerable<PFM.Database.Entities.CategoryEntity>> ReadCSV(Stream file);


    }
}
