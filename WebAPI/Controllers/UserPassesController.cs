using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebAPI.Utils;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/user-passes")]
    public class UserPassesController : Controller
    {
        private readonly IUserPassService _iUserPassService;
        private readonly ITicketService _iTicketService;
        private readonly ITicketTypeService _iTicketTypeService;
        private readonly ICollectionService _iCollectionService;
        private readonly IPassService _iPassService;
        private readonly IMapper _mapper;
        public UserPassesController(IUserPassService iUserPassService,
            ITicketService iTicketService,
            ITicketTypeService iTicketTypeService,
            ICollectionService iCollectionService,
            IPassService iPassService,
            IMapper mapper)
        {
            _iUserPassService = iUserPassService;
            _iTicketService = iTicketService;
            _iTicketTypeService = iTicketTypeService;
            _iCollectionService = iCollectionService;
            _iPassService = iPassService;
            _mapper = mapper;
        }
        //[HttpGet("check-user-pass-valid")]
        //public IActionResult CheckUserPassValid(Guid UserPassId, Guid TicketTypeId)
        //{
        //    var userPass = _iUserPassService.GetUserPassById(UserPassId);
        //    if(userPass == null || userPass.WillExpireAt >= DateTime.Now)
        //    {
        //        return BadRequest(false);
        //    }
        //    var pass = _iPassService.GetPassById(userPass.PassId);
        //    var tickets = _iTicketService.GetAllTicket(_ => _.UserPassId == UserPassId).Select(_ => _.TicketTypeId).ToList();
        //    var collections = _iCollectionService.GetAllCollection(_ => _.PassId == pass.Id, _ => _.TicketTypeInCollections).ToList();
        //    bool checkExisted = false;
        //    for(int i = 0; i< collections.Count; i++)
        //    {
        //        var collectionTypeInCollections = collections[i].TicketTypeInCollections.ToList();
        //        int maxConstrain = collections[i].MaxConstrain;
        //        collectionTypeInCollections.ForEach(c =>
        //        {
        //            if(c.TicketTypeId == TicketTypeId)
        //            {
        //                checkExisted = true;
        //            }
        //        });
        //    }
        //    if(!checkExisted)
        //    {
        //        return Ok(false);
        //    }
        //    return Ok();
        //}
        [HttpGet("check-user-pass-valid")]
        public async Task<IActionResult> CheckUserPassValid(Guid UserPassId, Guid TicketTypeId)
        {
            try
            {
                var userPass = _iUserPassService.GetUserPassById(UserPassId);
                if (userPass == null)
                {
                    return BadRequest(false);
                }
                if (userPass.WillExpireAt >= DateTime.Now)
                {
                    return Ok(false);
                }
                var ticket = _iTicketService
                    .GetAllTicket(_ => _.UserPassId == userPass.Id && _.TicketTypeId == TicketTypeId && _.UsedAt == null)
                    .FirstOrDefault();
                if (ticket != null)
                {
                    ticket.UsedAt = DateTime.Now;
                    _iTicketService.UpdateTicket(ticket);
                    await _iTicketService.SaveTicket();
                    return Ok(true);
                }
                return BadRequest(false);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("use-history")]
        public IActionResult GetHistory(Guid userPassId)
        {
            try
            {
                var userPass = _iUserPassService.GetUserPassById(userPassId);
                var userPassVM = _mapper.Map<UserPassVM>(userPass);
                var ticket = _iTicketService.GetAllTicket(_ => _.UserPassId == userPassId).ToList();
                var ticketUsed = ticket.Where(_ => _.UsedAt != null).ToList();
                var ticketInUse = ticket.Where(_ => _.UsedAt == null).ToList();
                userPassVM.ticketUsed = ticketUsed;
                userPassVM.ticketInUse = ticketInUse;
                return Ok(userPassVM);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("user-pass-avaiable")]
        public IActionResult GetUserPassAvaiable(String uId)
        {
            try
            {
                var list = _iUserPassService.GetAllUserPass(_ => _.UserUid == uId && _.WillExpireAt > DateTime.Now, _ => _.UsingTickets).ToList();
                for (int i = list.Count - 1; i > -1; i--)
                {
                    var userPass = list[i];
                    bool check = true;
                    for (int j = 0; j < userPass.UsingTickets.Count; j++)
                    {
                        var ticket = userPass.UsingTickets.ToList()[j];
                        if (ticket.UsedAt == null)
                        {
                            check = false;
                        }
                    }
                    userPass.UsingTickets = null;
                    if (check)
                    {
                        list.RemoveAt(i);
                    }
                }
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("user-pass-expire")]
        public IActionResult GetUserPassExpire(String uId)
        {
            try
            {
                var list = _iUserPassService.GetAllUserPass(_ => _.UserUid == uId, _ => _.UsingTickets).ToList();
                var listExpire = _iUserPassService.GetAllUserPass(_ => _.UserUid == uId && _.WillExpireAt < DateTime.Now).ToList();
                for (int i = list.Count - 1; i > -1; i--)
                {
                    var userPass = list[i];
                    bool check = true;
                    for (int j = 0; j < userPass.UsingTickets.Count; j++)
                    {
                        var ticket = userPass.UsingTickets.ToList()[j];
                        if (ticket.UsedAt != null)
                        {
                            check = false;
                        }
                    }
                    userPass.UsingTickets = null;
                    if (check)
                    {
                        list.RemoveAt(i);
                    }
                }
                list.AddRange(listExpire);
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserPass(UserPassCM userPassCM)
        {
            try
            {
                var userPass = _mapper.Map<UserPass>(userPassCM);
                var pass = _iPassService.GetPassById(userPassCM.PassId);
                if (pass == null)
                {
                    return NotFound("User Pass not found");
                }
                userPass.BoughtAt = DateTime.Now;
                userPass.WillExpireAt = DateTime.Now.AddDays(pass.ExpireDuration);

                userPassCM.UserPassDetailCMs.ForEach(_ =>
                {
                    for (int i = 0; i < _.Quantity; i++)
                    {
                        List<Ticket> listTicketType = new List<Ticket>();
                        var userPassAdd = CloneGeneric.Clone<UserPass>(userPass);
                        userPassAdd.IsChildren = _.IsChildren;
                        userPassAdd.PriceWhenBought = _.IsChildren ? pass.ChildrenPrice : pass.Price;
                        _iUserPassService.AddUserPass(userPassAdd);
                        userPassCM.TicketTypeIds.ForEach(_ =>
                        {
                            listTicketType.Add(new Ticket()
                            {
                                TicketTypeId = _,
                                UserPassId = userPassAdd.Id
                            });
                        });
                        _iTicketService.AddRangeTicket(listTicketType);
                    }
                });

                var result = await _iUserPassService.SaveUserPass();
                if (!result)
                {
                    return BadRequest("Can not create user pass");
                }
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}