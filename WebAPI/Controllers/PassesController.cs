using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPI.Controllers
{
    public class PassesController : Controller
    {
        private readonly IUserPassService _iUserPassService;
        private readonly ITicketService _iTicketService;
        private readonly ITicketTypeService _iTicketTypeService;
        private readonly ICollectionService _iCollectionService;
        private readonly IPassService _iPassService;
        public PassesController(IUserPassService iUserPassService,
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
        [HttpGet("{id}")]
        public IActionResult GetPass(Guid id)
        {
            try
            {
                var pass = _iPassService.GetAllPass(_ => _.Id == id).FirstOrDefault();
                if (pass == null) return NotFound();
                return Ok(pass);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}