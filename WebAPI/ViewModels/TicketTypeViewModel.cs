using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class TicketTypeSM
    {
        public String Name { get; set; }
        public decimal PriceFrom { get; set; } = 0;
        public decimal PriceTo { get; set; } = Decimal.MaxValue;
        public int PriceType { get; set; }
        public bool IsDelete { get; set; } = false;
        public String UrlImage { get; set; }
        public String Atrraction { get; set; }
        public String City { get; set; }
    }
    public class TicketTypeCM
    {
        public String Name { get; set; }
        public decimal? AdultPrice { get; set; }
        public decimal? ChildrenPrice { get; set; }
        public String UrlImage { get; set; }

        public int AtrractionId { get; set; }
    }

    public class TicketTypeVM
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public decimal? AdultPrice { get; set; }
        public decimal? ChildrenPrice { get; set; }
        public String UrlImage { get; set; }
        public String City { get; set; }
        public String Atrraction { get; set; }
        public bool IsDelete { get; set; }
    }

    public class TicketTypeUM
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public decimal? AdultPrice { get; set; }
        public decimal? ChildrenPrice { get; set; }
        public String UrlImage { get; set; }
        public int AtrractionId { get; set; }
    }
}
