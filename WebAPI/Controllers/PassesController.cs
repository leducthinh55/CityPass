using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Service;
using WebAPI.Utils;
using WebAPI.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class PassesController : Controller
    {
        private readonly IUserPassService _iUserPassService;
        private readonly ITicketService _iTicketService;
        private readonly ITicketTypeInCollectionService _iTicketTypeInCollectionService;
        private readonly ITicketTypeService _iTicketTypeService;
        private readonly ICollectionService _iCollectionService;
        private readonly IPassService _iPassService;
        private readonly IMapper _mapper;
        public PassesController(IUserPassService iUserPassService,
            ITicketService iTicketService,
            ITicketTypeService iTicketTypeService,
            ICollectionService iCollectionService,
            IPassService iPassService,
            ITicketTypeInCollectionService iTicketTypeInCollectionService,
            IMapper mapper)
        {
            _iUserPassService = iUserPassService;
            _iTicketService = iTicketService;
            _iTicketTypeService = iTicketTypeService;
            _iCollectionService = iCollectionService;
            _iPassService = iPassService;
            _iTicketTypeInCollectionService = iTicketTypeInCollectionService;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public IActionResult GetPass(Guid id)
        {
            try
            {
                var pass = _iPassService.GetAllPass(_ => _.Id == id, _ => _.Collections).FirstOrDefault();
                if (pass == null) return NotFound();
                var result = _mapper.Map<PassVM>(pass);
                result.Collections = new List<CollectionVM>();
                pass.Collections.ToList().ForEach(_ =>
                {

                    var collection = _iCollectionService.GetAllCollection(v => v.Id == _.Id, v => v.TicketTypeInCollections).FirstOrDefault();
                    var collectionVM = _mapper.Map<CollectionVM>(collection);
                    collectionVM.TicketTypes = new List<TicketType>();
                    var ticketTypeInCollections = collection.TicketTypeInCollections;
                    ticketTypeInCollections.ToList().ForEach(t =>
                    {
                        var ticketType = _iTicketTypeService.GetTicketTypeById(t.TicketTypeId);
                        ticketType.TicketTypeInCollections = null;
                        collectionVM.TicketTypes.Add(ticketType);
                    });
                    result.Collections.Add(collectionVM);
                });
                var userPasses = _iUserPassService.GetAllUserPass(_ => _.PassId == id && _.Rate != 0);
                var totalRate = userPasses.Count();
                var rateSum = userPasses.Sum(_ => _.Rate);
                result.Rate = totalRate != 0 ? (double)rateSum / (double)totalRate : 5;
                result.Feedbacks = userPasses.Select(_ => _.Feedback).ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetPass(decimal priceFrom, decimal priceTo, String name, int cityId, Guid? ticketTypeId, [FromQuery] DefaultSearch defaultSearch)
        {
            try
            {
                name = name ?? "";
                var ticketTypeInCollection = _iTicketTypeInCollectionService.GetAllTicketTypeInCollection(null, _ => _.TicketType.Atrraction.City);
                var b = ticketTypeInCollection.ToList();
                if (cityId != 0)
                {
                    // get list collectionId where that collection belong to CityId
                    ticketTypeInCollection = ticketTypeInCollection
                        .Where(_ => _.TicketType.Atrraction.City.Id == cityId);
                }
                if (ticketTypeId != null)
                {
                    // get list collectionId where that collection belong to CityId
                    ticketTypeInCollection = ticketTypeInCollection
                        .Where(_ => _.TicketTypeId == ticketTypeId);
                    var a = ticketTypeInCollection.ToList();
                }
                var collectionId = ticketTypeInCollection.GroupBy(_ => _.CollectionId).Select(_ => _.Key).ToList();
                //var list = new List<Pass>();
                //var listCollection = new List<Collection>();
                var listPassId = _iCollectionService.GetAllCollection(_ => collectionId.Contains(_.Id)).Select(_ => _.PassId).Distinct();
                //collectionId.ForEach(id =>
                //{
                //    listCollection.Add(_iCollectionService.GetCollectionById(id));
                //});
                //var listPassId = listCollection.GroupBy(_ => _.PassId).Select(_ => _.Key).ToList();
                //listPassId.ForEach(id =>
                //{
                //    list.Add(_iPassService.GetPassById(id));
                //});
                var list = _iPassService.GetAllPass(_ => listPassId.Contains(_.Id));
                list = list.Where(_ => _.Name.ToLower().Contains(name.ToLower()));

                if (priceFrom != 0 && priceTo != 0)
                {
                    list = list.Where(_ => _.Price >= priceFrom && _.Price <= priceTo);
                }
                int total = list.ToList().Count();
                if (defaultSearch.SortDir > 0)
                {
                    switch (defaultSearch.SortBy)
                    {
                        case "name":
                            list = list.OrderByDescending(_ => _.Name);
                            break;
                        case "price":
                            list = list.OrderByDescending(_ => _.Price);
                            break;
                        default:
                            list = list.OrderByDescending(_ => _.CreateAt);
                            break;
                    }
                }
                else
                {
                    switch (defaultSearch.SortBy)
                    {
                        case "name":
                            list = list.OrderBy(_ => _.Name);
                            break;
                        case "price":
                            list = list.OrderBy(_ => _.Price);
                            break;
                        default:
                            list = list.OrderBy(_ => _.CreateAt);
                            break;
                    }
                }
                list = list.Skip(defaultSearch.PageIndex)
                    .Take(defaultSearch.PageSize);
                var data = new List<PassVM>();
                list.ToList().ForEach(v =>
                {
                    var pass = _iPassService.GetAllPass(_ => _.Id == v.Id).Include(_ => _.UserPasses).Include(_ => _.Collections).ThenInclude(_ => _.TicketTypeInCollections).FirstOrDefault();
                    var userPassRate = pass.UserPasses.Where(_ => _.Rate != 0);
                    var rateSum = userPassRate.Sum(_ => _.Rate);
                    var totalRate = userPassRate.Count();
                    var NumberOfTicketType = pass.Collections.Sum(_ => _.TicketTypeInCollections.Count);
                    var passVM = _mapper.Map<PassVM>(v);
                    passVM.Rate = totalRate != 0 ? (double)rateSum / (double)totalRate : 5;
                    passVM.NumberOfTicketType = NumberOfTicketType;
                    data.Add(passVM);
                });
                return Ok(new { data, total });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePass([FromBody] PassCM passCM)
        {
            try
            {
                var pass = _mapper.Map<Pass>(passCM);
                _iPassService.AddPass(pass);
                passCM.Collections.ToList().ForEach(_ =>
                {
                    var collection = new Collection
                    {
                        MaxConstrain = _.MaxConstrain,
                        PassId = pass.Id
                    };
                    _iCollectionService.AddCollection(collection);
                    _.TicketTypeIds.ToList().ForEach(t =>
                    {
                        _iTicketTypeInCollectionService.AddTicketTypeInCollection(new TicketTypeInCollection()
                        {
                            TicketTypeId = t,
                            CollectionId = collection.Id
                        });
                    });
                });
                await _iPassService.SavePass();
                return StatusCode(201, pass);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePass([FromBody] PassUM passUM)
        {
            try
            {

                var passInDb = _iPassService.GetPassById(passUM.Id);
                var pass = _mapper.Map<Pass>(passUM);
                if (!passInDb.IsSelling)
                {
                    passInDb.Name = pass.Name;
                    passInDb.Description = pass.Description;
                    passInDb.Price = pass.Price;
                    passInDb.ExpireDuration = pass.ExpireDuration;
                    passInDb.ChildrenPrice = pass.ChildrenPrice;
                    passInDb.UrlImage = pass.UrlImage;
                    _iPassService.UpdatePass(passInDb);
                    _iTicketTypeInCollectionService.DeleteTicketTypeInCollection(_ => _.Collection.PassId == passUM.Id);
                    _iCollectionService.DeleteCollection(_ => _.PassId == passUM.Id);
                    await _iPassService.SavePass();
                }
                else
                {
                    _iPassService.AddPass(pass);
                }

                passUM.Collections.ToList().ForEach(_ =>
                {
                    var collection = new Collection
                    {
                        MaxConstrain = _.MaxConstrain,
                        PassId = pass.Id
                    };
                    _iCollectionService.AddCollection(collection);
                    _.TicketTypeIds.ToList().ForEach(t =>
                    {
                        _iTicketTypeInCollectionService.AddTicketTypeInCollection(new TicketTypeInCollection()
                        {
                            TicketTypeId = t,
                            CollectionId = collection.Id
                        });
                    });
                });
                await _iPassService.SavePass();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}