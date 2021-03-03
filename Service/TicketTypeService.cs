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
    public interface ITicketTypeService
    {
        void AddTicketType(TicketType TicketType);
        void UpdateTicketType(TicketType TicketType);
        void DeleteTicketType(Expression<Func<TicketType, bool>> where);
        void DeleteTicketType(TicketType TicketType);
        TicketType GetTicketTypeById(Guid Id);
        TicketType GetTicketType(Expression<Func<TicketType, bool>> where);
        IQueryable<TicketType> GetAllTicketType(Expression<Func<TicketType, bool>> where, params Expression<Func<TicketType, object>>[] includes);
        IQueryable<TicketType> GetAllTicketType();
        Task<bool> SaveTicketType();
    }

    public class TicketTypeService : ITicketTypeService
    {
        private readonly ITicketTypeRepository _iRespository;
        public TicketTypeService(ITicketTypeRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddTicketType(TicketType TicketType)
        {
            _iRespository.Add(TicketType);
        }

        public void DeleteTicketType(Expression<Func<TicketType, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateTicketType(TicketType TicketType)
        {
            _iRespository.Update(TicketType);
        }

        public TicketType GetTicketTypeById(Guid Id)
        {
            return _iRespository.GetById(Id);
        }

        public TicketType GetTicketType(Expression<Func<TicketType, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<TicketType> GetAllTicketType(Expression<Func<TicketType, bool>> where, params Expression<Func<TicketType, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<TicketType> GetAllTicketType()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveTicketType()
        {
            return await _iRespository.Commit();
        }

        public void DeleteTicketType(TicketType TicketType)
        {
            _iRespository.Delete(TicketType);
        }
    }
}
