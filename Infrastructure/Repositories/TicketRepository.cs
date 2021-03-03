using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface ITicketRepository : IRepository<Ticket, Guid>
    {

    }
    public class TicketRepository : Repository<Ticket, Guid>, ITicketRepository
    {
        public TicketRepository(CityPassContext context) : base(context)
        {
        }
    }
}
