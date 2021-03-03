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

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
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
        [Authorize]
        public IActionResult GetAttraction(int id)
        {
            try
            {
                var attraction = _IAtrractionService.GetAtrractionById(id);
                if (attraction == null) return NotFound();
                var result = _mapper.Map<AttractionVM>(attraction);
                return Ok(result);
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
                var list = _IAtrractionService.GetAllAtrraction(null, _ => _.City);
                if (!String.IsNullOrWhiteSpace(attractionSM.Name))
                {
                    list = list.Where(_ => _.Name.ToLower().Contains(attractionSM.Name.ToLower()));
                }
                if(attractionSM.IsTemporarityClosed != null)
                {
                    list = list.Where(_ => _.IsTemporarityClosed == attractionSM.IsTemporarityClosed);
                }
                if(!String.IsNullOrWhiteSpace(attractionSM.City))
                {
                    list = list.Where(_ => _.City.Name.ToLower() == attractionSM.City.ToLower());
                }
                int total = list.Count();
                list = GenericSorter<Attraction>.Sort(list, defaultSearch.SortBy = "Name", defaultSearch.SortDir);
                var data = list.Skip(defaultSearch.PageIndex * defaultSearch.PageSize)
                    .Take(defaultSearch.PageSize)
                    .Select(_ => _mapper.Map<AttractionVM>(_)).ToList();
                return Ok(new { data, total });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAttraction(AttractionCM attractionCM)
        {
            try
            {
                var city = _ICityService.GetCityById(attractionCM.CityId);
                if(city == null)
                {
                    return BadRequest();
                }

                var category = _ICategoryService.GetCategoryById(attractionCM.CategoryId);
                if (category == null)
                {
                    return BadRequest();
                }
                var attraction = _mapper.Map<Attraction>(attractionCM);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAttraction(AttractionUM attractionUM)
        {
            try
            {

                var attraction = _IAtrractionService.GetAtrractionById(attractionUM.Id);
                if (attraction == null) return NotFound();
                attraction.Name = attractionUM.Name;
                attraction.Address = attractionUM.Address;
                attraction.Description = attractionUM.Description;
                attraction.IsTemporarityClosed = attractionUM.IsTemporarityClosed;
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