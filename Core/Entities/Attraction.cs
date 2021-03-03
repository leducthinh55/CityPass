using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class Attraction
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Address { get; set; }
        public Boolean IsTemporarityClosed { get; set; }

        public virtual ICollection<TicketType> TicketTypes { get; set; }

        public virtual ICollection<WorkingTime> WorkingTimes { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
