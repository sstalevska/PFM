using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PFM.Commands;
using PFM.Models;
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
            var e = new List<ValidationError>();

            if (!IsFileCsvFormat(file))
            {
                e.Add(new ValidationError("file", "invalid-format", "The file provided is not in csv format."));
                var response = new
                {
                    errors = e
                };
                return BadRequest(response);
            }
            try
            {
                await _categoryService.ReadCSV(file.OpenReadStream());
                return Ok("Categories imported successfully.");
            }
            catch
            {
                e.Add(new ValidationError("file", "invalid-data", "The file provided doesn't contain required data fields."));
                return BadRequest(e);
                ;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? parentcode = null) {
            var response = await _categoryService.GetCategories(parentcode);
            if (response.Errors.Count() > 0)
            {
                return BadRequest(new { errors = response.Errors });
            }
            return Ok(response.Data);
        }
        public bool IsFileCsvFormat(IFormFile file)
        {
            var validExtensions = new[] { ".csv" };
            var fileExtension = Path.GetExtension(file.FileName);
            if (validExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }


    }
}
