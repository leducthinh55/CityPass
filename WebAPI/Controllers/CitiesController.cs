using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure.Data;
using Service;
using AutoMapper;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _ICityService;
        public CitiesController(ICityService ICityService)
        {
            _ICityService = ICityService;
        }
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            try
            {
                var city = _ICityService.GetCityById(id);
                if (city == null) return NotFound();
                return Ok(city);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetCity([FromQuery] String Name, [FromQuery] DefaultSearch defaultSearch)
        {
            try
            {
                var list = _ICityService.GetAllCity();
                if (!String.IsNullOrWhiteSpace(Name))
                {
                    list = list.Where(_ => _.Name.ToLower().Contains(Name.ToLower()));
                }
                int total = list.Count();
                list = GenericSorter<City>.Sort(list, "Name");
                var data = list.Skip(defaultSearch.PageIndex * defaultSearch.PageSize)
                    .Take(defaultSearch.PageSize);
                return Ok(new { data, total });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCity(String Name)
        {
            try
            {
                var City = new City
                {
                    Name = Name
                };
                _ICityService.AddCity(City);
                bool result = await _ICityService.SaveCity();
                if (!result)
                {
                    return BadRequest("Can not create City");
                }
                return StatusCode(201, new { Id = City.Id });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCity(int Id, String Name)
        {
            try
            {

                var City = _ICityService.GetCityById(Id);
                if (City == null) return NotFound();
                City.Name = Name;
                _ICityService.UpdateCity(City);
                bool result = await _ICityService.SaveCity();
                if (!result)
                {
                    return BadRequest("Can not update City");
                }
                return Ok(City);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                var City = _ICityService.GetCityById(id);
                if (City == null) return NotFound();
                _ICityService.DeleteCity(City);
                await _ICityService.SaveCity();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
