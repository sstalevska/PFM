﻿using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PFM.Commands;
using PFM.Database.Entities;
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


        public async Task<IEnumerable<PFM.Database.Entities.CategoryEntity>> ReadCSV(Stream file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace("-", ""),
                HeaderValidated = null,
                MissingFieldFound = null
            };

            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, config);
            var categories = csv.GetRecords<CategoryCSVCommand>();
            List<PFM.Database.Entities.CategoryEntity> categoryEntities = new List<PFM.Database.Entities.CategoryEntity>();

            HashSet<string> processedCategoryCodes = new HashSet<string>();

            foreach (var c in categories)
            {
                // proverka za duplikat vo csv
                if (!processedCategoryCodes.Contains(c.code))
                {
                    // proverka za duplikat vo baza
                    bool isDuplicateInDatabase = await _categoryRepository.IsDuplicateCategory(c.code);

                    if (!isDuplicateInDatabase)
                    {
                        var categoryEntity = _mapper.Map<PFM.Database.Entities.CategoryEntity>(c);
                        categoryEntities.Add(categoryEntity);

                        // isprocesirano id se dodava vo hashset
                        processedCategoryCodes.Add(c.code);
                    }

                }
            }

            await _categoryRepository.ImportCategories(categoryEntities);

            return categoryEntities;
        }

    }
}
