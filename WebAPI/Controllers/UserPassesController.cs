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
    [Route("api/v1.0/user-passes")]
    public class UserPassesController : Controller
    {
        private readonly IUserPassService _iUserPassService;
        private readonly ITicketService _iTicketService;
        private readonly ITicketTypeService _iTicketTypeService;
        private readonly ICollectionService _iCollectionService;
        private readonly IPassService _iPassService;
        private readonly IUserService _iUserService;
        private readonly IMapper _mapper;
        public UserPassesController(IUserPassService iUserPassService,
            ITicketService iTicketService,
            ITicketTypeService iTicketTypeService,
            ICollectionService iCollectionService,
            IPassService iPassService,
            IUserService iUserService,
            IMapper mapper)
        {
            _iUserPassService = iUserPassService;
            _iTicketService = iTicketService;
            _iTicketTypeService = iTicketTypeService;
            _iCollectionService = iCollectionService;
            _iPassService = iPassService;
            _mapper = mapper;
            _iUserService = iUserService;
        }
        
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
                if (userPass.WillExpireAt < DateTime.Now)
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
                    return Ok(ticket);
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
                if(userPass == null)
                {
                    return NotFound();
                }
                var userPassVM = _mapper.Map<UserPassVM>(userPass);
                var ticket = _iTicketService.GetAllTicket(_ => _.UserPassId == userPassId, _ => _.TicketType);
                var ticketUsed = ticket.Where(_ => _.UsedAt != null).Select(_ => _mapper.Map<TicketVM>(_)).ToList();
                var ticketInUse = ticket.Where(_ => _.UsedAt == null).Select(_ => _mapper.Map<TicketVM>(_)).ToList();
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
                var list = _iUserPassService.GetAllUserPass(_ => _.UserUid == uId && _.WillExpireAt > DateTime.Now, _ => _.UsingTickets, _ => _.Pass).ToList();
                var result = new List<UserPassVM>();
                for (int i = list.Count - 1; i > -1; i--)
                {
                    var userPass = list[i];
                    int used = 0;
                    int total = userPass.UsingTickets.Count;
                    for (int j = 0; j < total; j++)
                    {
                        var ticket = userPass.UsingTickets.ToList()[j];
                        if (ticket.UsedAt != null)
                        {
                            used++;
                        }
                    }
                    userPass.UsingTickets = null;
                    if (used == total)
                    {
                        list.RemoveAt(i);
                    }
                    else
                    {
                        var userPassVm = _mapper.Map<UserPassVM>(userPass);
                        userPassVm.NumberOfUsed = used;
                        userPassVm.Pass.UserPasses = null;
                        result.Add(userPassVm);
                    }   
                }
                return Ok(result);
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
                var list = _iUserPassService.GetAllUserPass(_ => _.UserUid == uId, _ => _.UsingTickets,_ => _.Pass).ToList();
                var listExpire = _iUserPassService.GetAllUserPass(_ => _.UserUid == uId && _.WillExpireAt < DateTime.Now, _ => _.Pass).ToList();
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
                var data = list.Select(_ => _mapper.Map<UserPassExpireVM>(_));
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("history-ticket-check")]
        public IActionResult GetHistoryTicketCheck(Guid ticketTypeId)
        {
            try
            {
                var Ticket = _iTicketService.GetAllTicket(_ => _.TicketTypeId == ticketTypeId && _.UsedAt.Value.Date == DateTime.Now.Date);
                return Ok(Ticket);
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
                var user = _iUserService.GetUserById(userPassCM.UserUid);
                if(user == null)
                {
                    user = new User() { Uid = userPassCM.UserUid };
                    _iUserService.AddUser(user);
                }
                var userPass = _mapper.Map<UserPass>(userPassCM);
                userPass.UserUid = user.Uid;
                var pass = _iPassService.GetPassById(userPassCM.PassId);
                if(!pass.IsSelling)
                {
                    pass.IsSelling = true;
                    _iPassService.UpdatePass(pass);
                }
                if (pass == null)
                {
                    return NotFound("User Pass not found");
                }
                userPass.BoughtAt = DateTime.Now;
                userPass.WillExpireAt = DateTime.Now.AddDays(pass.ExpireDuration);

                List<UserPass> listAdd = new List<UserPass>();
                var childrenPrice = pass.ChildrenPrice;
                var adultPrice = pass.Price;
                
                for (int i = 0; i< userPassCM.QuantiyAdult; i++)
                {
                    UserPass passAdd = CloneGeneric.Clone<UserPass>(userPass);
                    passAdd.IsChildren = false;
                    passAdd.PriceWhenBought = adultPrice;
                    _iUserPassService.AddUserPass(passAdd);
                    List<Ticket> listTicketType = new List<Ticket>();
                    userPassCM.TicketTypeIds.ForEach(_ =>
                    {
                        listTicketType.Add(new Ticket()
                        {
                            TicketTypeId = _,
                            UserPassId = passAdd.Id
                        });
                    });
                    _iTicketService.AddRangeTicket(listTicketType);
                }
                for (int i = 0; i < userPassCM.QuantiyChildren; i++)
                {
                    UserPass passAdd = CloneGeneric.Clone<UserPass>(userPass);
                    passAdd.IsChildren = true;
                    passAdd.PriceWhenBought = childrenPrice;
                    _iUserPassService.AddUserPass(passAdd);
                    List<Ticket> listTicketType = new List<Ticket>();
                    userPassCM.TicketTypeIds.ForEach(_ =>
                    {
                        listTicketType.Add(new Ticket()
                        {
                            TicketTypeId = _,
                            UserPassId = passAdd.Id
                        });
                    });
                    _iTicketService.AddRangeTicket(listTicketType);
                }
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