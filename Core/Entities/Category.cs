using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String UrlImage { get; set; }
        public String Icon { get; set; }
        public String ThemeColor { get; set; }
        public virtual ICollection<Attraction> Atrractions { get; set; }
    }
}
