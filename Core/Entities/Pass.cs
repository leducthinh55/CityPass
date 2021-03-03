using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Pass : BaseEntity
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public bool IsSelling { get; set; }
        public int ExpireDuration { get; set; }
        
        public Pass PassChildren { get; set; }

        public virtual ICollection<UserPass> UserPasses { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }

    }
}
