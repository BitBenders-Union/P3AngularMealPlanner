using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;

        public CategoryController(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // get all categories
        [HttpGet]
        public IActionResult Get() 
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());

            if (categories == null || categories.Count() == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

        // get category by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if(!_categoryRepository.CategoriesExists(id))
                return NotFound();

            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(id));

            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(category);
        }

        // get category from recipeId
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetCategoryByRecipeId(int recipeId) 
        {
            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategoryByRecipeId(recipeId));

            if(!ModelState.IsValid)
                return BadRequest();

            if(category == null)
                return NotFound();

            return Ok(category);

        }


    }
}
