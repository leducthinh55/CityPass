using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class TicketVM
    {
        public DateTime? UsedAt { get; set; }

        public Guid UserPassId { get; set; }

        public Guid TicketTypeId { get; set; }

        public String TicketTypeName { get; set; }
    }
}
