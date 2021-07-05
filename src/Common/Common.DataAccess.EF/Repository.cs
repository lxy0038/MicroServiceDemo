using Common.DataAccess.EF.Extensions;
using Common.Insfratructure.Data;
using Common.Insfratructure.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Common.DataAccess.EF
{
    public class BaseRepository : IRepository
    {
        private readonly CommonContext _commonContext;

        public BaseRepository(
            CommonContext _commonContext)
        {
            this._commonContext = _commonContext;
        }

        #region Insert
        public int Add<T>(T entity) where T : ModelBase
        {
            if (entity == null) return 0;
            _commonContext.Set<T>().Add(entity);
            return _commonContext.SaveChanges();
        }

        public async Task<int> AddAsync<T>(T entity) where T : ModelBase
        {
            if (entity == null) return 0;
            _commonContext.Set<T>().Add(entity);
            return await _commonContext.SaveChangesAsync();
        }

        public T AddGet<T>(T entity) where T : ModelBase
        {
            if (entity == null) return entity;
            _commonContext.Set<T>().Add(entity);
            try
            {
                _commonContext.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<T> AddGetAsync<T>(T entity) where T : ModelBase
        {
            if (entity == null) return entity;
            _commonContext.Set<T>().Add(entity);
            try
            {
                await _commonContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int BatchAdd<T>(List<T> entities, bool hasKey = false) where T : ModelBase
        {
            if (entities == null) return 0;
            var sucCount = entities.Count;
            if (sucCount == 0)
            {
                return 0;
            }
            var sourcedt = entities.ToDataTable(hasKey);
            string csv = sourcedt.ToCsv();
            var entityMap = EFExtension.GetEntityMap(typeof(T), _commonContext);
            if (entityMap == null)
            {
                throw new ArgumentException("Could not load the entity mapping information for the source.", "source");
            }

            try
            {
                var columns = new List<string>();
                foreach (DataColumn item in sourcedt.Columns)
                {
                    string columnName = entityMap.PropertyMaps.Where(p => p.PropertyName == item.ColumnName).Select(p => p.ColumnName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        columns.Add(columnName);
                    }
                }
                byte[] array = Encoding.UTF8.GetBytes(csv);
                MemoryStream stream = new MemoryStream(array);
                var conn = _commonContext.Database.GetDbConnection() as MySqlConnection;
                var loader = new MySqlBulkLoader(conn)
                {
                    SourceStream = stream,
                    Local = true,
                    LineTerminator = EFExtension.IsWin() ? "\r\n" : "\n",
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    TableName = entityMap.TableName,
                    CharacterSet = "UTF8"
                };
                loader.Columns.AddRange(columns);
                sucCount = loader.Load();
                return sucCount;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                sourcedt.Clear();
                sourcedt.Dispose();
            }
        }

        public async Task<int> BatchAddAsync<T>(List<T> entities, bool hasKey = false) where T : ModelBase
        {
            if (entities == null) return 0;
            var sucCount = entities.Count;
            if (sucCount == 0)
            {
                return 0;
            }
            var sourcedt = entities.ToDataTable(hasKey);
            string csv = sourcedt.ToCsv();
            var entityMap = EFExtension.GetEntityMap(typeof(T), _commonContext);
            if (entityMap == null)
            {
                throw new ArgumentException("Could not load the entity mapping information for the source.", "source");
            }

            try
            {
                var columns = new List<string>();
                foreach (DataColumn item in sourcedt.Columns)
                {
                    string columnName = entityMap.PropertyMaps.Where(p => p.PropertyName == item.ColumnName).Select(p => p.ColumnName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        columns.Add(columnName);
                    }
                }
                byte[] array = Encoding.UTF8.GetBytes(csv);
                MemoryStream stream = new MemoryStream(array);
                var conn = _commonContext.Database.GetDbConnection() as MySqlConnection;
                var loader = new MySqlBulkLoader(conn)
                {
                    SourceStream = stream,
                    Local = true,
                    LineTerminator = EFExtension.IsWin() ? "\r\n" : "\n",
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    TableName = entityMap.TableName,
                    CharacterSet = "UTF8"
                };
                loader.Columns.AddRange(columns);
                sucCount = await loader.LoadAsync();
                return sucCount;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                sourcedt.Clear();
                sourcedt.Dispose();
            }
        }

        public int AddRange<T>(List<T> entities) where T : ModelBase
        {
            _commonContext.AddRange(entities);
            return _commonContext.SaveChanges();
        }

        public async Task<int> AddRangeAsync<T>(List<T> entities) where T : ModelBase
        {
            _commonContext.AddRange(entities);
            return await _commonContext.SaveChangesAsync();
        }
        #endregion

        #region Update
        public bool Update<T>(T entity) where T : ModelBase
        {
            if (entity == null) return true;
            _commonContext.Update(entity);
            return _commonContext.SaveChanges() > 0;
        }
        public async Task<bool> UpdateAsync<T>(T entity) where T : ModelBase
        {
            if (entity == null) return true;
            _commonContext.Update(entity);
            return await _commonContext.SaveChangesAsync() > 0;
        }

        public bool Update<T>(Expression<Func<T, T>> epsUpdate, Expression<Func<T, bool>> where) where T : ModelBase
        {
            if (where == null)
            {
                throw new Exception("不允许整张表进行更新");
            }


            return _commonContext.Set<T>().Where(where).Update(epsUpdate) > 0;
        }

        public async Task<bool> UpdateAsync<T>(Expression<Func<T, T>> epsUpdate, Expression<Func<T, bool>> where) where T : ModelBase
        {
            if (where == null)
            {
                throw new Exception("不允许整张表进行更新");
            }


            return await _commonContext.Set<T>().Where(where).UpdateAsync(epsUpdate) > 0;
        }

        public bool UpdateRange<T>(ICollection<T> entities) where T : ModelBase
        {
            if (entities == null) return true;
            _commonContext.UpdateRange(entities);
            return _commonContext.SaveChanges() > 0;
        }

        public bool BatchUpdate<T>(List<T> entities) where T : ModelBase
        {
            if (entities == null) return true;
            var sql = EFExtension.GetMysqlUpdateSqls<T>(entities, _commonContext);
            _commonContext.Database.SetCommandTimeout(30000);
            var updateCount = _commonContext.Database.ExecuteSqlRaw(sql);

            return updateCount > 0;
        }

        public async Task<bool> BatchUpdateAsync<T>(List<T> entities) where T : ModelBase
        {
            if (entities == null) return true;
            var sql = EFExtension.GetMysqlUpdateSqls<T>(entities, _commonContext);
            var updateCount = await _commonContext.Database.ExecuteSqlRawAsync(sql);

            return updateCount > 0;
        }

        public async Task<bool> UpdateRangeAsync<T>(ICollection<T> entities) where T : ModelBase
        {
            if (entities == null) return true;
            _commonContext.UpdateRange(entities);
            return await _commonContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region Delete
        public int Delete<T>(params string[] ids) where T : ModelBase
        {
            foreach (var entity in ids.Select(id => _commonContext.Set<T>().Find(id)))
            {
                if (entity != null)
                    _commonContext.Set<T>().Remove(entity);
            }

            return _commonContext.SaveChanges();
        }

        public int Delete<T>(Expression<Func<T, bool>> whereLambda) where T : ModelBase
        {
            if (whereLambda == null)
            {
                throw new Exception("不允全表删除");
            }

            int count = _commonContext.Set<T>().Where(whereLambda).Delete();

            return count;
        }

        public bool Delete<T>(T entity) where T : ModelBase
        {
            _commonContext.Set<T>().Remove(entity);
            return _commonContext.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync<T>(T entity) where T : ModelBase
        {
            _commonContext.Set<T>().Remove(entity);
            return await _commonContext.SaveChangesAsync() > 0;
        }
        #endregion

        #region Query
        public bool IsExist<T>(Expression<Func<T, bool>> where = null) where T : ModelBase
        {
            if (where != null)
            {
                return _commonContext.Set<T>().AsNoTracking().Where(where).Any();
            }
            return _commonContext.Set<T>().AsNoTracking().Any();
        }

        public async Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> where = null) where T : ModelBase
        {
            if (where != null)
            {
                return await _commonContext.Set<T>().AsNoTracking().Where(where).AnyAsync();
            }
            return await _commonContext.Set<T>().AsNoTracking().AnyAsync();
        }

        public int GetTotal<T>(Expression<Func<T, bool>> where = null) where T : ModelBase
        {
            if (where != null)
            {
                return _commonContext.Set<T>().AsNoTracking().Where(where).Count();
            }
            return _commonContext.Set<T>().AsNoTracking().Count();
        }

        public async Task<int> GetTotalAsync<T>(Expression<Func<T, bool>> where = null) where T : ModelBase
        {
            if (where != null)
            {
                return await _commonContext.Set<T>().AsNoTracking().Where(where).CountAsync();
            }
            return await _commonContext.Set<T>().AsNoTracking().CountAsync();
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> whereLambda) where T : ModelBase
        {
            return await _commonContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(whereLambda);
        }
        public T GetEntity<T>(Expression<Func<T, bool>> whereLambda, params string[] paths) where T : ModelBase
        {
            var query = GetQueryable(whereLambda, paths);
            if (query.Count() > 0)
            {
                return query.FirstOrDefault();
            }
            else
            {
                return null;
            }
            //return  _commonContext.Set<T>().AsNoTracking().AsQueryable().FirstOrDefault(whereLambda);
        }
        public async Task<T> Get<T>(Expression<Func<T, bool>> whereLambda, params string[] paths) where T : ModelBase
        {
            var query = _commonContext.Set<T>().AsNoTracking().AsQueryable();

            if (paths == null) return await query.FirstOrDefaultAsync(whereLambda);
            foreach (var item in paths)
            {
                query = query.Include(item);
            }

            return await query.FirstOrDefaultAsync(whereLambda);
        }

        public IQueryable<T> GetEntities<T>(
            Expression<Func<T, bool>> whereLambda,
            params string[] paths) where T : ModelBase
        {
            return GetQueryable(whereLambda, paths);
        }

        public IQueryable<T> GetOrderPageEntities<T>(
           int pageSize,
           int pageIndex,
           out int total,
           Expression<Func<T, bool>> whereLambda,
           Expression<Func<T, object>> orderByLambda,
           bool isAsc,
           params string[] paths) where T : ModelBase
        {
            total = GetQueryable(whereLambda, paths).Where(whereLambda).Count();
            if (isAsc)
            {
                return GetQueryable(whereLambda, paths)
                        .OrderBy(orderByLambda)
                        .Skip(pageSize * (pageIndex - 1))
                        .Take(pageSize);
            }

            return GetQueryable(whereLambda, paths)
                    .OrderByDescending(orderByLambda)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize);
        }

        public decimal GetCount<T>(Expression<Func<T, bool>> expression) where T : ModelBase
        {
            var count = _commonContext.Set<T>().Count(expression);
            return count;
        }

        public decimal GetSum<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> sumLambda) where T : ModelBase
        {
            var sum = _commonContext.Set<T>().Where(expression).Sum(sumLambda);
            return sum;
        }

        public List<IGrouping<S, T>> GetGroupList<T, S>(Expression<Func<T, S>> groupBy) where T : ModelBase
        {
            var _list = _commonContext.Set<T>().AsNoTracking().GroupBy<T, S>(groupBy);

            return _list.ToList();
        }


        private IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> expression, params string[] paths) where T : ModelBase
        {
            var queryTable = _commonContext.Set<T>().AsNoTracking().AsQueryable();
            if (expression != null)
                queryTable = queryTable.Where(expression);
            if (paths == null || !paths.Any()) return queryTable;

            foreach (var item in paths)
                queryTable = queryTable.Include(item);

            return queryTable;
        }
        #endregion

    }
}
