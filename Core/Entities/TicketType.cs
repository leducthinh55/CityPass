using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class TicketType : BaseEntity
    {
        public String Name { get; set; }
        public decimal? AdultPrice { get; set; }
        public decimal? ChildrenPrice { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public virtual ICollection<TicketTypeInCollection> TicketTypeInCollections { get; set; }

        public int AtrractionId { get; set; }
        public Attraction Atrraction { get; set; }
    }
}
