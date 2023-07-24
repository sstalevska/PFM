using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PFM.Database.Repositories;
using PFM.Models;
using PFM.Models.Enums;
using System.Globalization;

namespace PFM.Services
{
    public class CategoryService : ICategoryService
    {
        ICategoryRepository _categoryRepository;
        IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper) {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<Category>> GetCategories(string? parentcode)
        {
            var categories = await _categoryRepository.GetCategories(parentcode);
            return _mapper.Map<List<Category>>(categories);
        }
        /*
        public async Task<Results<List<CategoryRepository>>> ImportCategoriesAsync(IFormFile file)
        {
            var categories = CsvParser.ParseCSV<Category, CategoryCSVMap>(file);
            if(categories is null)
            {
                var exception = new Exception("Error occured while reading CSV file");
                return new Result<List<CategoryRepository>>(exception);
            }
            _categoryRepository.ImportCategories(categories);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<List<CategoryRepository>>(categories.Take(10).ToList());

            }
            catch
            {
                var exception = new Exception("Error occured while writing in database");
                return new Microsoft.AspNetCore.Http.Results<List<CategoryRepository>>(exception);
            }
        }*/

    }
}
