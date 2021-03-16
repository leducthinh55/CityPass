using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Ticket : BaseEntity
    {
        public DateTime? UsedAt { get; set; }

        public Guid UserPassId { get; set; }
        public UserPass UserPass { get; set; }

        public Guid TicketTypeId { get; set; }

        public TicketType TicketType {get;set;}
    }
}
