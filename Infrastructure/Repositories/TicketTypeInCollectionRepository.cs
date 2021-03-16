using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface ITicketTypeInCollectionRepository : IRepository<TicketTypeInCollection, Guid>
    {

    }
    public class TicketTypeInCollectionRepository : Repository<TicketTypeInCollection, Guid>, ITicketTypeInCollectionRepository
    {
        public TicketTypeInCollectionRepository(CityPassContext context) : base(context)
        {
        }
    }
}
