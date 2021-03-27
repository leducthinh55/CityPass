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
using System.IO;
using Firebase.Storage;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/v1.0/ticket-types")]
    [ApiController]
    public class TicketTypesController : ControllerBase
    {
        private readonly ITicketTypeService _ITicketTypeService;
        private readonly ITicketService _ITicketService;
        private readonly IMapper _mapper;
        private readonly IUploadFile _iUploadFile;
        public TicketTypesController(ITicketTypeService ITicketTypeService, IMapper mapper, ITicketService ITicketService, IUploadFile iUploadFile)
        {
            _ITicketTypeService = ITicketTypeService;
            _ITicketService = ITicketService;
            _mapper = mapper;
            _iUploadFile = iUploadFile;
        }
        [HttpGet("{id}")]
        public IActionResult GetTicketType(Guid id)
        {
            try
            {
                var TicketType = _ITicketTypeService.GetAllTicketType(_ => _.Id == id, _ => _.Atrraction, _ => _.Atrraction.City).FirstOrDefault();
                
                if (TicketType == null) return NotFound();
                var result = _mapper.Map<TicketTypeDetailVM>(TicketType);
                return Ok(result);
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
                var list = _ITicketTypeService.GetAllTicketType(_ => _.IsDelete == ticketType.IsDelete, _ => _.Atrraction, _ => _.Atrraction.City, _ => _.Atrraction.Category, _ => _.Tickets);
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
                if (ticketType.CategoryId != 0)
                {
                    list = list.Where(_ => _.Atrraction.Category.Id == ticketType.CategoryId);
                }
                if (ticketType.PriceFrom != 0 && ticketType.PriceTo != 0)
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
        [Authorize(Policy = "Admin")]
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
                    AtrractionId = ticketType.AtrractionId,
                    CreateAt = DateTime.Now
                };
                _ITicketTypeService.AddTicketType(TicketType);
                await _ITicketTypeService.SaveTicketType();
                IList<IFormFile> imageUpload = ticketType.ImageUpload.ToList();
                
                List<String> listImage = new List<string>();
                if (imageUpload.Count > 0)
                {
                    for (int i = 0; i < imageUpload.Count; i++)
                    {
                        var file = imageUpload[i];
                        var link = await _iUploadFile.uploadFile(file, TicketType.Id.ToString());
                        listImage.Add(link);
                    }
                }
                TicketType.UrlImage = String.Join(";", listImage);
                _ITicketTypeService.UpdateTicketType(TicketType);
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
        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateTicketType([FromForm] TicketTypeUM ticketTypeUM)
        {
            try
            {

                var TicketType = _ITicketTypeService.GetTicketTypeById(ticketTypeUM.Id);
                if (TicketType == null) return NotFound();
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;

                IList<IFormFile> imageUpload = ticketTypeUM.ImageUpload.ToList();
                List<String> listImage = new List<string>();
                if (imageUpload.Count > 0)
                {
                    for (int i = 0; i < imageUpload.Count; i++)
                    {
                        var file = imageUpload[i];
                        //Stream ms = file.OpenReadStream();
                        //var cancellation = new CancellationTokenSource();
                        //var task = new FirebaseStorage(
                        //"citypass131999.appspot.com",
                        //new FirebaseStorageOptions
                        //{
                        //    AuthTokenAsyncFactory = () => Task.FromResult("eyJhbGciOiJSUzI1NiIsImtpZCI6IjRlMDBlOGZlNWYyYzg4Y2YwYzcwNDRmMzA3ZjdlNzM5Nzg4ZTRmMWUiLCJ0eXAiOiJKV1QifQ.eyJhZG1pbiI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL2NpdHlwYXNzMTMxOTk5IiwiYXVkIjoiY2l0eXBhc3MxMzE5OTkiLCJhdXRoX3RpbWUiOjE2MTY1NTQzNTksInVzZXJfaWQiOiJscURaSE45T05tZU12MWJQdmk4SjVzS1VQR1YyIiwic3ViIjoibHFEWkhOOU9ObWVNdjFiUHZpOEo1c0tVUEdWMiIsImlhdCI6MTYxNjU1NDM1OSwiZXhwIjoxNjE2NTU3OTU5LCJlbWFpbCI6InRoaW5oQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJ0aGluaEBnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.Fo4-suiJN2srzcicYejDGGero5nFmNj-SWfgMA-YcLbfgfHgA1P8gcg1Y9-Kc2bJTffEhpYTiqgUgft9TRMgDSyUsUt4xLpNC77KqjbGEPM5iqrMOepJAm6A6XLizq9s-ija2BIIrUXDkIHkTaVwC_Q5zsVcmiRZkUo8A_RHW-U7-4juCGkKs8o0tjvrSi-64XDcl8OLUbCWeACYfeJw2lcQvH4I4NgZrdXJcgUi9oHQJ8CqyqmUWdIwnLgF6ETRIgolflK0kjNWpO9HA0b2Avu4zggp9DQfPlw3wID3WZG8Pe7c-2Og3pKj8ymVOZnqegvemJkWLtu7Qqin0RynPA"),
                        //    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                        //})
                        //.Child("ticket-type")
                        //.Child($"{ticketTypeUM.Id}")
                        //.Child(file.Name + DateTime.Now.ToString())
                        //.PutAsync(ms, cancellation.Token);
                        var link = await _iUploadFile.uploadFile(file, ticketTypeUM.Id.ToString());
                        listImage.Add(link);
                    }
                }
                listImage.AddRange(ticketTypeUM.UrlImages);
                TicketType.UrlImage = String.Join(";", listImage);
                TicketType.Name = ticketTypeUM.Name;
                //TicketType.UrlImage = ticketTypeUM.UrlImage;
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
        [Authorize(Policy = "Admin")]
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
