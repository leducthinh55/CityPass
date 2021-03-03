using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IUserPassRepository : IRepository<UserPass, Guid>
    {

    }
    public class UserPassRepository : Repository<UserPass, Guid>, IUserPassRepository
    {
        public UserPassRepository(CityPassContext context) : base(context)
        {
        }
    }
}
