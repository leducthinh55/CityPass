using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class CollectionVM
    {
        public Guid Id { get; set; }

        public int MaxConstrain { get; set; }

        public virtual ICollection<TicketType> TicketTypes { get; set; }
    }

    public class CollectionCM
    {
        public int MaxConstrain { get; set; }

        public virtual ICollection<Guid> TicketTypeIds { get; set; }
    }
    public class CollectionUM
    {
        public Guid Id { get; set; }
        public int MaxConstrain { get; set; }

        public virtual ICollection<Guid> TicketTypeIds { get; set; }
    }

}
