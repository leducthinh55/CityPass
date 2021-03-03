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
    public interface ICollectionService
    {
        void AddCollection(Collection Collection);
        void UpdateCollection(Collection Collection);
        void DeleteCollection(Expression<Func<Collection, bool>> where);
        void DeleteCollection(Collection Collection);
        Collection GetCollectionById(Guid Id);
        Collection GetCollection(Expression<Func<Collection, bool>> where);
        IQueryable<Collection> GetAllCollection(Expression<Func<Collection, bool>> where, params Expression<Func<Collection, object>>[] includes);
        IQueryable<Collection> GetAllCollection();
        Task<bool> SaveCollection();
    }

    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository _iRespository;
        public CollectionService(ICollectionRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddCollection(Collection Collection)
        {
            _iRespository.Add(Collection);
        }

        public void DeleteCollection(Expression<Func<Collection, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateCollection(Collection Collection)
        {
            _iRespository.Update(Collection);
        }

        public Collection GetCollectionById(Guid Id)
        {
            return _iRespository.GetById(Id);
        }

        public Collection GetCollection(Expression<Func<Collection, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<Collection> GetAllCollection(Expression<Func<Collection, bool>> where, params Expression<Func<Collection, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<Collection> GetAllCollection()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveCollection()
        {
            return await _iRespository.Commit();
        }

        public void DeleteCollection(Collection Collection)
        {
            _iRespository.Delete(Collection);
        }
    }
}
