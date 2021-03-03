using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class TicketTypeInCollection
    {
        public Guid TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }

        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
    }
}
