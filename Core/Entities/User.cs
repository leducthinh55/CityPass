using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class User
    {
        [Key]
        public String Uid { get; set; }

        public virtual ICollection<UserPass> UserPasses { get; set; }
    }
}
