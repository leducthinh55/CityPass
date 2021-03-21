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
    public interface IUserService
    {
        void AddUser(User User);
        void UpdateUser(User User);
        void DeleteUser(Expression<Func<User, bool>> where);
        void DeleteUser(User User);
        User GetUserById(String Id);
        User GetUser(Expression<Func<User, bool>> where);
        IQueryable<User> GetAllUser(Expression<Func<User, bool>> where, params Expression<Func<User, object>>[] includes);
        IQueryable<User> GetAllUser();
        Task<bool> SaveUser();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _iRespository;
        public UserService(IUserRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddUser(User User)
        {
            _iRespository.Add(User);
        }

        public void DeleteUser(Expression<Func<User, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateUser(User User)
        {
            _iRespository.Update(User);
        }

        public User GetUserById(String Id)
        {
            return _iRespository.GetById(Id);
        }

        public User GetUser(Expression<Func<User, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<User> GetAllUser(Expression<Func<User, bool>> where, params Expression<Func<User, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<User> GetAllUser()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveUser()
        {
            return await _iRespository.Commit();
        }

        public void DeleteUser(User User)
        {
            _iRespository.Delete(User);
        }
    }
}
