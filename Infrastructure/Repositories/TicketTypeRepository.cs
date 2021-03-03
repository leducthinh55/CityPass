using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface ITicketTypeRepository : IRepository<TicketType, Guid>
    {

    }
    public class TicketTypeRepository : Repository<TicketType, Guid>, ITicketTypeRepository
    {
        public TicketTypeRepository(CityPassContext context) : base(context)
        {
        }
    }
}
