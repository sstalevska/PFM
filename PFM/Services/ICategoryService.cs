using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Services
{
    public interface ICategoryService 
    {
         Task<ListResponse<Category>> GetCategories(string? parentcode);
        Task<IEnumerable<PFM.Database.Entities.CategoryEntity>> ReadCSV(Stream file);


    }
}
