using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface ICollectionRepository : IRepository<Collection, Guid>
    {

    }
    public class CollectionRepository : Repository<Collection, Guid>, ICollectionRepository
    {
        public CollectionRepository(CityPassContext context) : base(context)
        {
        }
    }
}
