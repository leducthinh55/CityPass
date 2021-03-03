using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IAttractionRepository : IRepository<Attraction, int> 
    {
        // khi IAttractionRepository có những phương thức đặc thù mà Repository<Attraction, int> không có, ta định nghĩ method đó tại đây
        
    }
    public class AttractionRepository : Repository<Attraction, int>, IAttractionRepository
    {
        public AttractionRepository(CityPassContext context) : base(context)
        {
        }
    }
}
