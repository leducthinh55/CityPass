using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class UserPassVM
    {
        public DateTime WillExpireAt { get; set; }
        public DateTime BoughtAt { get; set; }
        public String Feedback { get; set; }
        public decimal PriceWhenBought { get; set; }
        public int Rate { get; set; }
        public bool IsChildren { get; set; }
        public int NumberOfUsed { get; set; }

        public virtual ICollection<TicketVM> ticketUsed { get; set; }
        public virtual ICollection<TicketVM> ticketInUse { get; set; }

        public Pass Pass { get; set; }
    }

    //public class UserPassCM
    //{
    //    public String UserUid { get; set; }
    //    public List<UserPassDetailCM> UserPassDetailCMs { get; set; }
    //    public Guid PassId { get; set; }
    //    public List<Guid> TicketTypeIds { get; set; } 
    //}

    //public class UserPassDetailCM {
    //    public bool IsChildren { get; set; }
    //    public int Quantity { get; set; }
    //}

    public class UserPassCM
    {
        public String UserUid { get; set; }
        public int QuantiyChildren { get; set; }
        public int QuantiyAdult { get; set; }
        public Guid PassId { get; set; }
        public List<Guid> TicketTypeIds { get; set; }
    }
}
