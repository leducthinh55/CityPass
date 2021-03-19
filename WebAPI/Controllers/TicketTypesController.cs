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
using WebAPI.ViewModels;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [Route("api/ticket-types")]
    [ApiController]
    public class TicketTypesController : ControllerBase
    {
        private readonly ITicketTypeService _ITicketTypeService;
        private readonly IMapper _mapper;
        public TicketTypesController(ITicketTypeService ITicketTypeService, IMapper mapper)
        {
            _ITicketTypeService = ITicketTypeService;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public IActionResult GetTicketType(Guid id)
        {
            try
            {
                var TicketType = _ITicketTypeService.GetAllTicketType(_ => _.Id == id, _ => _.Atrraction, _ => _.Atrraction.City).FirstOrDefault();
                if (TicketType == null) return NotFound();
                return Ok(TicketType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IActionResult GetTicketType([FromQuery] TicketTypeSM ticketType, [FromQuery] DefaultSearch defaultSearch)
        {
            try
            {
                var list = _ITicketTypeService.GetAllTicketType(_ => _.IsDelete == ticketType.IsDelete, _ => _.Atrraction, _ => _.Atrraction.City);
                if (!String.IsNullOrWhiteSpace(ticketType.Name))
                {
                    list = list.Where(_ => _.Name.ToLower().Contains(ticketType.Name.ToLower()));
                }
                if (!String.IsNullOrWhiteSpace(ticketType.Attraction))
                {
                    list = list.Where(_ => _.Atrraction.Name.ToLower().Contains(ticketType.Attraction.ToLower()));
                } 
                if (!String.IsNullOrWhiteSpace(ticketType.City))
                {
                    list = list.Where(_ => _.Atrraction.City.Name.ToLower().Contains(ticketType.City.ToLower()));
                }
                if(ticketType.PriceFrom != 0 && ticketType.PriceTo != 0)
                {
                    list = list.Where(_ => _.AdultPrice >= ticketType.PriceFrom && _.AdultPrice <= ticketType.PriceTo);
                }
                int total = list.Count();
                switch (defaultSearch.SortBy)
                {
                    case "name":
                        list = GenericSorter.Sort(list, _ => _.Name, defaultSearch.SortDir);
                        break;
                    case "attraction":
                        list = GenericSorter.Sort(list, _ => _.Atrraction.Name, defaultSearch.SortDir);
                        break;
                    case "city":
                        list = GenericSorter.Sort(list, _ => _.Atrraction.City.Name, defaultSearch.SortDir);
                        break;
                    case "adultPrice":
                        list = GenericSorter.Sort(list, _ => _.AdultPrice, defaultSearch.SortDir);
                        break;
                    case "childrenPrice":
                        list = GenericSorter.Sort(list, _ => _.ChildrenPrice, defaultSearch.SortDir);
                        break;
                    default:
                        list = GenericSorter.Sort(list, _ => _.CreateAt, defaultSearch.SortDir);
                        break;
                }
                var data = list.Skip(defaultSearch.PageIndex)
                   .Take(defaultSearch.PageSize)
                   .Select(_ => _mapper.Map<TicketTypeVM>(_))
                   .ToList();
                return Ok(new { data, total });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicketType([FromForm] TicketTypeCM ticketType)
        {
            try
            {
                var TicketType = new TicketType
                {
                    Name = ticketType.Name,
                    AdultPrice = ticketType.AdultPrice,
                    ChildrenPrice = ticketType.ChildrenPrice,
                    AtrractionId = ticketType.AtrractionId
                };
                _ITicketTypeService.AddTicketType(TicketType);
                bool result = await _ITicketTypeService.SaveTicketType();
                if (!result)
                {
                    return BadRequest("Can not create TicketType");
                }
                return StatusCode(201, new { Id = TicketType.Id });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTicketType([FromBody] TicketTypeUM ticketTypeUM)
        {
            try
            {

                var TicketType = _ITicketTypeService.GetTicketTypeById(ticketTypeUM.Id);
                if (TicketType == null) return NotFound();
                TicketType.Name = ticketTypeUM.Name;
                TicketType.UrlImage = ticketTypeUM.UrlImage;
                TicketType.AdultPrice = ticketTypeUM.AdultPrice;
                TicketType.ChildrenPrice = ticketTypeUM.ChildrenPrice;
                TicketType.AtrractionId = ticketTypeUM.AtrractionId;
                _ITicketTypeService.UpdateTicketType(TicketType);
                bool result = await _ITicketTypeService.SaveTicketType();
                if (!result)
                {
                    return BadRequest("Can not update TicketType");
                }
                return Ok(TicketType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketType(Guid id)
        {
            try
            {
                var TicketType = _ITicketTypeService.GetTicketTypeById(id);
                if (TicketType == null) return NotFound();
                TicketType.IsDelete = false;
                _ITicketTypeService.UpdateTicketType(TicketType);
                await _ITicketTypeService.SaveTicketType();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
