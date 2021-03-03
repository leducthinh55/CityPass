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
    public interface IUserPassService
    {
        void AddUserPass(UserPass UserPass);
        void UpdateUserPass(UserPass UserPass);
        void DeleteUserPass(Expression<Func<UserPass, bool>> where);
        void DeleteUserPass(UserPass UserPass);
        UserPass GetUserPassById(Guid Id);
        UserPass GetUserPass(Expression<Func<UserPass, bool>> where);
        IQueryable<UserPass> GetAllUserPass(Expression<Func<UserPass, bool>> where, params Expression<Func<UserPass, object>>[] includes);
        IQueryable<UserPass> GetAllUserPass();
        Task<bool> SaveUserPass();
    }

    public class UserPassService : IUserPassService
    {
        private readonly IUserPassRepository _iRespository;
        public UserPassService(IUserPassRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddUserPass(UserPass UserPass)
        {
            _iRespository.Add(UserPass);
        }

        public void DeleteUserPass(Expression<Func<UserPass, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateUserPass(UserPass UserPass)
        {
            _iRespository.Update(UserPass);
        }

        public UserPass GetUserPassById(Guid Id)
        {
            return _iRespository.GetById(Id);
        }

        public UserPass GetUserPass(Expression<Func<UserPass, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<UserPass> GetAllUserPass(Expression<Func<UserPass, bool>> where, params Expression<Func<UserPass, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<UserPass> GetAllUserPass()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveUserPass()
        {
            return await _iRespository.Commit();
        }

        public void DeleteUserPass(UserPass UserPass)
        {
            _iRespository.Delete(UserPass);
        }
    }
}
