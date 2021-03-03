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
    public interface IAtrractionService
    {
        void AddAtrraction(Attraction Atrraction);
        void UpdateAtrraction(Attraction Atrraction);
        void DeleteAtrraction(Expression<Func<Attraction, bool>> where);
        void DeleteAtrraction(Attraction Atrraction);
        Attraction GetAtrractionById(int Id);
        Attraction GetAtrraction(Expression<Func<Attraction, bool>> where);
        IQueryable<Attraction> GetAllAtrraction(Expression<Func<Attraction, bool>> where, params Expression<Func<Attraction, object>>[] includes);
        IQueryable<Attraction> GetAllAtrraction();
        Task<bool> SaveAtrraction();
    }

    public class AtrractionService : IAtrractionService
    {
        private readonly IAttractionRepository _iRespository;
        public AtrractionService(IAttractionRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddAtrraction(Attraction Atrraction)
        {
            _iRespository.Add(Atrraction);
        }

        public void DeleteAtrraction(Expression<Func<Attraction, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateAtrraction(Attraction Atrraction)
        {
            _iRespository.Update(Atrraction);
        }

        public Attraction GetAtrractionById(int Id)
        {
            return _iRespository.GetById(Id);
        }

        public Attraction GetAtrraction(Expression<Func<Attraction, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<Attraction> GetAllAtrraction(Expression<Func<Attraction, bool>> where, params Expression<Func<Attraction, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<Attraction> GetAllAtrraction()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveAtrraction()
        {
            return await _iRespository.Commit();
        }

        public void DeleteAtrraction(Attraction attraction)
        {
            _iRespository.Delete(attraction);
        }
    }
}
