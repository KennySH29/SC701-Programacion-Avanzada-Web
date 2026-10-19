using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetCategories")]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await categoryRepository.ReadAsync() ?? [];
            return categories.Select(CategoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            if (category == null)
                return NotFound();
            return Ok(CategoryDTO.ConvertFrom(category));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] CategoryDTO categoryDTO)
        {
            var category = CategoryDTO.ConvertTo(categoryDTO);
            category.CategoryId = 0;
            category.LastModified = DateTime.Now;
            var result = await categoryRepository.CreateAsync(category);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] CategoryDTO categoryDTO)
        {
            var category = await categoryRepository.FindAsync(id);

            if (category == null)
                return NotFound();

            category.CategoryName = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.ModifiedBy = categoryDTO.ModifiedBy;
            category.LastModified = DateTime.Now;

            var result = await categoryRepository.UpdateAsync(category);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var category = await categoryRepository.FindAsync(id);

            if (category == null)
                return NotFound();

            var result = await categoryRepository.DeleteAsync(category);
            return Ok(result);
        }
    }
}

