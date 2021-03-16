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
        public decimal ChildrenPrice { get; set; }
        public bool IsSelling { get; set; }
        public int ExpireDuration { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual ICollection<UserPass> UserPasses { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }

    }
}
