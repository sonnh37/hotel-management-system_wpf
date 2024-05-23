using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Managements
{
    public class BaseManagement<TModel> where TModel : BaseModel
    {
        private readonly DbContext _dbContext;
        public BaseManagement()
        {

        }
        public BaseManagement(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Add(TModel) + AddRange(IEnumerable<TModel>)
        public void Add(TModel model)
        {

            try
            {
                DbSet.Add(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddRange(IEnumerable<TModel> models)
        {

            try
            {
                if (models.Any())
                {
                    DbSet.AddRange(models);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update(TModel) + UpdateRange(IEnumerable<TModel>)
        public void Update(TModel model)
        {
            try
            {
                DbSet.Update(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateRange(IEnumerable<TModel> models)
        {

            try
            {
                if (models.Any())
                {
                    DbSet.UpdateRange(models);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete(TModel) + DeleteRange(IEnumerable<TModel>)
        public void Delete(TModel model)
        {
            DbSet.Remove(model);
        }

        public void DeleteRange(IEnumerable<TModel> models)
        {
            DbSet.RemoveRange(models);
        }
        #endregion

        public List<TModel> GetAll(Expression<Func<TModel, bool>> predicate)
        {
            List<TModel> result;
            try
            {
                var queryable = GetQueryable(predicate);
                result = queryable.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;

        }

        public List<TModel> GetAll()
        {
            List<TModel> result;
            try
            {
                var queryable = GetQueryable<TModel>();
                result = queryable.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public IQueryable<T> GetQueryable<T>()
            where T : BaseModel
        {
            IQueryable<T> queryable = GetDbSet<T>(); // like DbSet in this
            return queryable;

        }

        public IQueryable<TModel> GetQueryable(Expression<Func<TModel, bool>> predicate)
        {
            IQueryable<TModel> queryable = GetQueryable<TModel>();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            return queryable;
        }

        protected DbSet<TModel> DbSet
        {
            get
            {
                var dbSet = GetDbSet<TModel>();
                return dbSet;
            }
        }
        protected DbSet<T> GetDbSet<T>() where T : BaseModel
        {
            var dbSet = _dbContext.Set<T>();
            return dbSet;
        }
    }
}
