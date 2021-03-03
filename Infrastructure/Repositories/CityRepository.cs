using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface ICityRepository : IRepository<City, int>
    {

    }
    public class CityRepository : Repository<City, int>, ICityRepository
    {
        public CityRepository(CityPassContext context) : base(context)
        {
        }
    }
}
