using Common.Insfratructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Insfratructure.Data
{
    public interface IRepository
    {
        int Add<T>(T entity) where T : ModelBase;

        Task<int> AddAsync<T>(T entity) where T : ModelBase;

        public T AddGet<T>(T entity) where T : ModelBase;

        Task<T> AddGetAsync<T>(T entity) where T : ModelBase;

        int BatchAdd<T>(List<T> entities, bool hasKey = false) where T : ModelBase;

        Task<int> BatchAddAsync<T>(List<T> entities, bool hasKey = false) where T : ModelBase;

        int AddRange<T>(List<T> entities) where T : ModelBase;

        Task<int> AddRangeAsync<T>(List<T> entities) where T : ModelBase;

        bool Update<T>(T entity) where T : ModelBase;

        Task<bool> UpdateAsync<T>(T entity) where T : ModelBase;

        bool Update<T>(Expression<Func<T, T>> epsUpdate, Expression<Func<T, bool>> where) where T : ModelBase;

        Task<bool> UpdateAsync<T>(Expression<Func<T, T>> epsUpdate, Expression<Func<T, bool>> where) where T : ModelBase;

        bool UpdateRange<T>(ICollection<T> entities) where T : ModelBase;

        Task<bool> UpdateRangeAsync<T>(ICollection<T> entities) where T : ModelBase;

        //bool BatchUpdate(List<T> entities);

        //Task<bool> BatchUpdateAsync(List<T> entities);

        int Delete<T>(params string[] ids) where T : ModelBase;

        int Delete<T>(Expression<Func<T, bool>> whereLambda) where T : ModelBase;

        bool Delete<T>(T entity) where T : ModelBase;

        Task<bool> DeleteAsync<T>(T entity) where T : ModelBase;

        bool IsExist<T>(Expression<Func<T, bool>> where = null) where T : ModelBase;

        Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> where = null) where T : ModelBase;

        int GetTotal<T>(Expression<Func<T, bool>> where = null) where T : ModelBase;

        Task<int> GetTotalAsync<T>(Expression<Func<T, bool>> where = null) where T : ModelBase;

        Task<T> Get<T>(Expression<Func<T, bool>> whereLambda) where T : ModelBase;

        Task<T> Get<T>(Expression<Func<T, bool>> whereLambda, params string[] paths) where T : ModelBase;

        T GetEntity<T>(Expression<Func<T, bool>> whereLambda, params string[] paths) where T : ModelBase;
        IQueryable<T> GetEntities<T>(Expression<Func<T, bool>> whereLambda, params string[] paths) where T : ModelBase;

        IQueryable<T> GetOrderPageEntities<T>(
           int pageSize,
           int pageIndex,
           out int total,
           Expression<Func<T, bool>> whereLambda,
           Expression<Func<T, object>> orderByLambda,
           bool isAsc,
           params string[] paths) where T : ModelBase;

        decimal GetCount<T>(Expression<Func<T, bool>> expression) where T : ModelBase;

        decimal GetSum<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> sumLambda) where T : ModelBase;

        List<IGrouping<S, T>> GetGroupList<T, S>(Expression<Func<T, S>> groupBy) where T : ModelBase;
    }
}
