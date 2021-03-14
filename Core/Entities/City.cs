using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual ICollection<Attraction> Atrractions { get; set; }
    }
}
