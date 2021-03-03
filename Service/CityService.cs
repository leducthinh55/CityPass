using Core.Entities;
using Infrastructure.Infrastructure;
using Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Service
{
    public interface ICityService
    {
        void AddCity(City City);
        void UpdateCity(City City);
        void DeleteCity(Expression<Func<City, bool>> where);
        void DeleteCity(City city);
        City GetCityById(int Id);
        City GetCity(Expression<Func<City, bool>> where);
        IQueryable<City> GetAllCity(Expression<Func<City, bool>> where, params Expression<Func<City, object>>[] includes);
        IQueryable<City> GetAllCity();
        Task<bool> SaveCity();
    }

    public class CityService : ICityService
    {
        private readonly ICityRepository _iRespository;
        public CityService(ICityRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddCity(City City)
        {
            _iRespository.Add(City);
        }

        public void DeleteCity(Expression<Func<City, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateCity(City City)
        {
            _iRespository.Update(City);
        }

        public City GetCityById(int Id)
        {
            return _iRespository.GetById(Id);
        }

        public City GetCity(Expression<Func<City, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<City> GetAllCity(Expression<Func<City, bool>> where, params Expression<Func<City, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<City> GetAllCity()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveCity()
        {
            return await _iRespository.Commit();
        }

        public void DeleteCity(City city)
        {
            _iRespository.Delete(city);
        }
    }
}
