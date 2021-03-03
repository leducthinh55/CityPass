using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class AttractionSM
    {
        public String Name { get; set; }
        public String City { get; set; }
        public Boolean? IsTemporarityClosed { get; set; } 
    }

    public class AttractionVM
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Address { get; set; }
        public Boolean IsTemporarityClosed { get; set; } = false;
        public String City { get; set; }
    }
    
    public class AttractionCM
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public String Address { get; set; }
        public Boolean IsTemporarityClosed { get; set; } = false;
        public int CityId { get; set; }
        public int CategoryId { get; set; }
    }

    public class AttractionUM
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Address { get; set; }
        public Boolean IsTemporarityClosed { get; set; } = false;
    }
}
