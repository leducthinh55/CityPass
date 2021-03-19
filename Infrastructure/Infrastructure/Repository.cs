using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Infrastructure
{
    public interface IRepository<T, TKey>
    {
        T GetById(TKey Id);

        T Get(Expression<Func<T, bool>> where);

        void Update(T entity);

        void Add(T entity);

        void AddRange(IEnumerable<T> list);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        IQueryable<T> GetAll(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAll();

        Task<bool> Commit();
    }

    public abstract class Repository<T, Tkey> : IRepository<T, Tkey> where T : class where Tkey : IEquatable<Tkey>
    {

        private CityPassContext _context;

        private DbSet<T> dbSet;

        protected Repository(CityPassContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            T x = dbSet.FirstOrDefault(where);
            return x;
        }

        public virtual T GetById(Tkey Id)
        {
            var entity = dbSet.Find(Id);
            if(entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> list)
        {
            _context.AddRange(list);
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var listRemove = dbSet.Where(where);
            dbSet.RemoveRange(listRemove);
        }

        public virtual void Update(T entity)
        {
            //dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbSet.AsNoTracking(); ;
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result = dbSet;
            if (where != null)
            {
                result = dbSet.Where(where);
            }
            foreach (var expression in includes)
            {
                result = result.Include(expression);
            }
            return result.AsNoTracking();
        }

        public virtual async Task<bool> Commit()
        {
            return await _context.Commit();
        }

        
    }
}
