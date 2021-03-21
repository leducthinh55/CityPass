using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User, String>
    {

    }
    public class UserRepository : Repository<User, String>, IUserRepository
    {
        public UserRepository(CityPassContext context) : base(context)
        {
        }
    }
}
