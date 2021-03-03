using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IPassRepository : IRepository<Pass, Guid>
    {

    }
    public class PassRepository : Repository<Pass, Guid>, IPassRepository
    {
        public PassRepository(CityPassContext context) : base(context)
        {
        }
    }
}
