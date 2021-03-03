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
    public interface ICategoryService
    {
        void AddCategory(Category Category);
        void UpdateCategory(Category Category);
        void DeleteCategory(Expression<Func<Category, bool>> where);
        void DeleteCategory(Category category);
        Category GetCategoryById(int Id);
        Category GetCategory(Expression<Func<Category, bool>> where);
        IQueryable<Category> GetAllCategory(Expression<Func<Category, bool>> where, params Expression<Func<Category, object>>[] includes);
        IQueryable<Category> GetAllCategory();
        Task<bool> SaveCategory();
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _iRespository;
        public CategoryService(ICategoryRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddCategory(Category Category)
        {
            _iRespository.Add(Category);
        }

        public void DeleteCategory(Expression<Func<Category, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateCategory(Category Category)
        {
            _iRespository.Update(Category);
        }

        public Category GetCategoryById(int Id)
        {
            return _iRespository.GetById(Id);
        }

        public Category GetCategory(Expression<Func<Category, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<Category> GetAllCategory(Expression<Func<Category, bool>> where, params Expression<Func<Category, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<Category> GetAllCategory()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveCategory()
        {
            return await _iRespository.Commit();
        }

        public void DeleteCategory(Category category)
        {
            _iRespository.Delete(category);
        }
    }
}
