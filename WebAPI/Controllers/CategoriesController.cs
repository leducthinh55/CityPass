using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebAPI.Utils;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/v1.0/[Controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _ICategoryService;
        public CategoriesController(ICategoryService ICategoryService)
        {
            _ICategoryService = ICategoryService;
        }
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            try
            {
                var Category = _ICategoryService.GetCategoryById(id);
                if (Category == null) return NotFound();
                return Ok(Category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetCategory([FromQuery] String Name, [FromQuery] DefaultSearch defaultSearch)
        {
            try
            {
                var list = _ICategoryService.GetAllCategory();
                if (!String.IsNullOrWhiteSpace(Name))
                {
                    list = list.Where(_ => _.Name.ToLower().Contains(Name.ToLower()));
                }
                int total = list.Count();
                var data = list.Skip(defaultSearch.PageIndex)
                   .Take(defaultSearch.PageSize)
                   .ToList();
                return Ok(new { data, total });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(String Name)
        {
            try
            {
                var Category = new Category
                {
                    Name = Name
                };
                _ICategoryService.AddCategory(Category);
                bool result = await _ICategoryService.SaveCategory();
                if (!result)
                {
                    return BadRequest("Can not create Category");
                }
                return StatusCode(201, new { Id = Category.Id });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int Id, String Name)
        {
            try
            {

                var Category = _ICategoryService.GetCategoryById(Id);
                if (Category == null) return NotFound();
                Category.Name = Name;
                _ICategoryService.UpdateCategory(Category);
                bool result = await _ICategoryService.SaveCategory();
                if (!result)
                {
                    return BadRequest("Can not update Category");
                }
                return Ok(Category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var Category = _ICategoryService.GetCategoryById(id);
                if (Category == null) return NotFound();
                _ICategoryService.DeleteCategory(Category);
                await _ICategoryService.SaveCategory();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}