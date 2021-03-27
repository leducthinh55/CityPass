using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebAPI.Utils;
using WebAPI.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class AttractionsController : Controller
    {
        private readonly IAtrractionService _IAtrractionService;
        private readonly ICategoryService _ICategoryService;
        private readonly ICityService _ICityService;
        private readonly IMapper _mapper;
        private readonly IMailService _IMailService;
        public AttractionsController(IAtrractionService IAtrractionService, ICategoryService ICategoryService, ICityService ICityService, IMapper mapper, IMailService IMailService)
        {
            _IAtrractionService = IAtrractionService;
            _ICategoryService = ICategoryService;
            _ICityService = ICityService;
            _mapper = mapper;
            _IMailService = IMailService;
        }
        [HttpGet("{id}")]
        public IActionResult GetAttraction(int id)
        {
            try
            {
                var attraction = _IAtrractionService.GetAllAtrraction(_ => _.Id == id, _ => _.City, _ => _.Category, _ => _.WorkingTimes).FirstOrDefault();
                if (attraction == null) return NotFound();
                return Ok(attraction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAttraction([FromQuery] AttractionSM attractionSM, [FromQuery] DefaultSearch defaultSearch)
        {
            try
            {
                var list = _IAtrractionService.GetAllAtrraction(null, _ => _.City, _ => _.Category);
                if (!String.IsNullOrWhiteSpace(attractionSM.City))
                {
                    list = list.Where(_ => _.City.Name.ToLower() == attractionSM.City.ToLower());
                }
                if (!String.IsNullOrWhiteSpace(attractionSM.Name))
                {
                    list = list.Where(_ => _.Name.ToLower().Contains(attractionSM.Name.ToLower()));
                }
                if (attractionSM.IsTemporarityClosed != null)
                {
                    list = list.Where(_ => _.IsTemporarityClosed == attractionSM.IsTemporarityClosed);
                }
                if (!String.IsNullOrWhiteSpace(attractionSM.Category))
                {
                    list = list.Where(_ => _.Category.Name.ToLower() == attractionSM.Category.ToLower());
                }
                int total = list.ToList().Count();
                switch (defaultSearch.SortBy)
                {
                    case "name":
                        list = GenericSorter.Sort(list, _ => _.Name, defaultSearch.SortDir);
                        break;
                    case "category":
                        list = GenericSorter.Sort(list, _ => _.Category.Name, defaultSearch.SortDir);
                        break;
                    case "city":
                        list = GenericSorter.Sort(list, _ => _.City.Name, defaultSearch.SortDir);
                        break;
                    default:
                        list = GenericSorter.Sort(list, _ => _.CreateAt, defaultSearch.SortDir);
                        break;
                }
                var data = list.Skip(defaultSearch.PageIndex)
                    .Take(defaultSearch.PageSize)
                    .Select(_ => _mapper.Map<AttractionVM>(_))
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
        public async Task<IActionResult> CreateAttraction(AttractionCM attractionCM)
        {
            try
            {
                var city = _ICityService.GetCity(_ => _.Name == attractionCM.CityName);
                if (city == null)
                {
                    city = new City() { Name = attractionCM.CityName };
                    _ICityService.AddCity(city);
                    await _ICityService.SaveCity();
                }

                var category = _ICategoryService.GetCategoryById(attractionCM.CategoryId);
                if (category == null)
                {
                    return BadRequest();
                }
                var attraction = _mapper.Map<Attraction>(attractionCM);
                attraction.CityId = city.Id;
                attraction.CreateAt = DateTime.Now;
                _IAtrractionService.AddAtrraction(attraction);
                bool result = await _IAtrractionService.SaveAtrraction();
                if (!result)
                {
                    return BadRequest("Can not create attraction");
                }
                return StatusCode(201, new { Id = attraction.Id });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateAttraction(AttractionUM attractionUM)
        {
            try
            {
                var city = _ICityService.GetCity(_ => _.Name == attractionUM.CityName);
                if (city == null)
                {
                    city = new City() { Name = attractionUM.CityName };
                    _ICityService.AddCity(city);
                    await _ICityService.SaveCity();
                }
                var attraction = _IAtrractionService.GetAtrractionById(attractionUM.Id);
                if (attraction == null) return NotFound();
                attraction.Name = attractionUM.Name;
                attraction.Address = attractionUM.Address;
                attraction.Description = attractionUM.Description;
                attraction.IsTemporarityClosed = attractionUM.IsTemporarityClosed;
                attraction.CategoryId = attractionUM.CategoryId;
                attraction.CityId = city.Id;
                attraction.Lat = attractionUM.Lat;
                attraction.Lng = attractionUM.Lng;
                _IAtrractionService.UpdateAtrraction(attraction);
                bool result = await _IAtrractionService.SaveAtrraction();
                if (!result)
                {
                    return BadRequest("Can not update attraction");
                }
                return Ok(attraction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttraction(int id)
        {
            try
            {
                var attraction = _IAtrractionService.GetAtrractionById(id);
                if (attraction == null) return NotFound();
                _IAtrractionService.DeleteAtrraction(attraction);
                await _IAtrractionService.SaveAtrraction();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}