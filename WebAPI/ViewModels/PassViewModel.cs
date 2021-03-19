using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels
{
    public class PassVM 
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public decimal ChildrenPrice { get; set; }
        public String UrlImage { get; set; }
        public decimal Price { get; set; }
        public bool IsSelling { get; set; }
        public int ExpireDuration { get; set; }
        public double Rate { get; set; }
        public virtual ICollection<String> Feedbacks { get; set; }
        
        public virtual ICollection<CollectionVM> Collections { get; set; }
    }
    public class PassCM
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public String UrlImage { get; set; }
        public int ExpireDuration { get; set; }
        public decimal ChildrenPrice { get; set; }

        public virtual ICollection<CollectionCM> Collections { get; set; }
    }

    public class PassUM
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public String UrlImage { get; set; }
        public bool IsSelling { get; set; } = false;
        public int ExpireDuration { get; set; }
        public decimal ChildrenPrice { get; set; }

        public virtual ICollection<CollectionUM> Collections { get; set; }
    }
}
