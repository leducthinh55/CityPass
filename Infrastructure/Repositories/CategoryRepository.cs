using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface ICategoryRepository : IRepository<Category, int>
    {

    }
    public class CategoryRepository : Repository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(CityPassContext context) : base(context)
        {
        }
    }
}
