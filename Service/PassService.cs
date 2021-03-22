using Core.Entities;
using Infrastructure.Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public interface IPassService
    {
        void AddPass(Pass Pass);
        void AddRangePass(IEnumerable<Pass> Passes);
        void UpdatePass(Pass Pass);
        void DeletePass(Expression<Func<Pass, bool>> where);
        void DeletePass(Pass Pass);
        Pass GetPassById(Guid Id);
        Pass GetPass(Expression<Func<Pass, bool>> where);
        IQueryable<Pass> GetAllPass(Expression<Func<Pass, bool>> where, params Expression<Func<Pass, object>>[] includes);
        IQueryable<Pass> GetAllPass(Expression<Func<Pass, bool>> where,Func<IQueryable<Pass>, IIncludableQueryable<Pass, object>> includes);
        IQueryable<Pass> GetAllPass();
        Task<bool> SavePass();
    }

    public class PassService : IPassService
    {
        private readonly IPassRepository _iRespository;
        public PassService( IPassRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddPass(Pass Pass)
        {
            _iRespository.Add(Pass);
        }

        public void DeletePass(Expression<Func<Pass, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdatePass(Pass Pass)
        {
            _iRespository.Update(Pass);
        }

        public Pass GetPassById(Guid Id)
        {
            return _iRespository.GetById(Id);
        }

        public Pass GetPass(Expression<Func<Pass, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<Pass> GetAllPass(Expression<Func<Pass, bool>> where, params Expression<Func<Pass, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<Pass> GetAllPass()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SavePass()
        {
            return await _iRespository.Commit();
        }

        public void DeletePass(Pass Pass)
        {
            _iRespository.Delete(Pass);
        }

        public void AddRangePass(IEnumerable<Pass> Passes)
        {
            _iRespository.AddRange(Passes);
        }

        public IQueryable<Pass> GetAllPass(Expression<Func<Pass, bool>> where,Func<IQueryable<Pass>, IIncludableQueryable<Pass, object>> includes)
        {
            return _iRespository.GetAll(where, includes);
        }
    }
}
