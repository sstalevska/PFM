using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PFM.Commands;
using PFM.Models.Enums;
using PFM.Services;
using System.Globalization;

namespace PFM.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsFromCSV([FromForm] IFormFile file)
        {
            var categories = await _categoryService.ReadCSV(file.OpenReadStream());

            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? parentcode = null) {
            var categories = await _categoryService.GetCategories(parentcode);
            return Ok(categories);
        }


    }
}
