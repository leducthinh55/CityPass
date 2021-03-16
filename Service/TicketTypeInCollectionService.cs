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
    public interface ITicketTypeInCollectionService
    {
        void AddTicketTypeInCollection(TicketTypeInCollection TicketTypeInCollection);
        void UpdateTicketTypeInCollection(TicketTypeInCollection TicketTypeInCollection);
        void DeleteTicketTypeInCollection(Expression<Func<TicketTypeInCollection, bool>> where);
        void DeleteTicketTypeInCollection(TicketTypeInCollection TicketTypeInCollection);
        TicketTypeInCollection GetTicketTypeInCollectionById(Guid Id);
        TicketTypeInCollection GetTicketTypeInCollection(Expression<Func<TicketTypeInCollection, bool>> where);
        IQueryable<TicketTypeInCollection> GetAllTicketTypeInCollection(Expression<Func<TicketTypeInCollection, bool>> where, params Expression<Func<TicketTypeInCollection, object>>[] includes);
        IQueryable<TicketTypeInCollection> GetAllTicketTypeInCollection();
        Task<bool> SaveTicketTypeInCollection();
    }

    public class TicketTypeInCollectionService : ITicketTypeInCollectionService
    {
        private readonly ITicketTypeInCollectionRepository _iRespository;
        public TicketTypeInCollectionService(ITicketTypeInCollectionRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddTicketTypeInCollection(TicketTypeInCollection TicketTypeInCollection)
        {
            _iRespository.Add(TicketTypeInCollection);
        }

        public void DeleteTicketTypeInCollection(Expression<Func<TicketTypeInCollection, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateTicketTypeInCollection(TicketTypeInCollection TicketTypeInCollection)
        {
            _iRespository.Update(TicketTypeInCollection);
        }

        public TicketTypeInCollection GetTicketTypeInCollectionById(Guid Id)
        {
            return _iRespository.GetById(Id);
        }

        public TicketTypeInCollection GetTicketTypeInCollection(Expression<Func<TicketTypeInCollection, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<TicketTypeInCollection> GetAllTicketTypeInCollection(Expression<Func<TicketTypeInCollection, bool>> where, params Expression<Func<TicketTypeInCollection, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<TicketTypeInCollection> GetAllTicketTypeInCollection()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveTicketTypeInCollection()
        {
            return await _iRespository.Commit();
        }

        public void DeleteTicketTypeInCollection(TicketTypeInCollection TicketTypeInCollection)
        {
            _iRespository.Delete(TicketTypeInCollection);
        }
    }
}
