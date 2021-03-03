using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPassesController : Controller
    {
        private readonly IUserPassService _iUserPassService;
        private readonly ITicketService _iTicketService;
        private readonly ITicketTypeService _iTicketTypeService;
        private readonly ICollectionService _iCollectionService;
        private readonly IPassService _iPassService;
        public UserPassesController(IUserPassService iUserPassService,
            ITicketService iTicketService,
            ITicketTypeService iTicketTypeService,
            ICollectionService iCollectionService,
            IPassService iPassService)
        {
            _iUserPassService = iUserPassService;
            _iTicketService = iTicketService;
            _iTicketTypeService = iTicketTypeService;
            _iCollectionService = iCollectionService;
            _iPassService = iPassService;
        }
        [HttpGet]
        public IActionResult CheckUserPassValid(Guid UserPassId, Guid TicketTypeId)
        {
            var userPass = _iUserPassService.GetUserPassById(UserPassId);
            if(userPass == null || userPass.WillExpireAt >= DateTime.Now)
            {
                return BadRequest(false);
            }
            var pass = _iPassService.GetPassById(userPass.PassId);
            var tickets = _iTicketService.GetAllTicket(_ => _.UserPassId == UserPassId).Select(_ => _.TicketTypeId).ToList();
            var collections = _iCollectionService.GetAllCollection(_ => _.PassId == pass.Id, _ => _.TicketTypeInCollections).ToList();
            bool checkExisted = false;
            for(int i = 0; i< collections.Count; i++)
            {
                var collectionTypeInCollections = collections[i].TicketTypeInCollections.ToList();
                int maxConstrain = collections[i].MaxConstrain;
                collectionTypeInCollections.ForEach(c =>
                {
                    if(c.TicketTypeId == TicketTypeId)
                    {
                        checkExisted = true;
                    }
                });
            }
            if(!checkExisted)
            {
                return Ok(false);
            }
            return Ok();
        }
    }
}