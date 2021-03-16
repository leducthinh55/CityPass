using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class UserPass : BaseEntity
    {
        public DateTime WillExpireAt { get; set; }
        public DateTime BoughtAt { get; set; }
        public String Feedback { get; set; }
        public decimal PriceWhenBought { get; set; }
        public int Rate { get; set; }
        public bool IsChildren { get; set; }

        public virtual ICollection<Ticket> UsingTickets { get; set; }

        public Guid PassId { get; set; }
        public Pass Pass { get; set; }

        public String UserUid { get; set; }
        public User User { get; set; }
    }
}
