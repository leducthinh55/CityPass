using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Collection : BaseEntity
    {
        public int MaxConstrain { get; set; }

        public virtual ICollection<TicketTypeInCollection> TicketTypeInCollections { get; set; }

        public Guid PassId { get; set; }
        public Pass Passes { get; set; }    
    }
}
