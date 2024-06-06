using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FAK.Domain.Entities;
using static Dapper.SqlMapper;
using Dapper;

namespace FAK.Infrastructure.Interface
{
    public partial interface IRepository
    {
        void BeginTransaction();
        void BeginTransactionAsync();

        //Pindah Ke Ext
        //IQueryable<T> GetQueryableAsSingleQuery<T>() where T : BaseEntity;
        Task<(bool Commited, string Message)> RollbackAsync();

        IQueryable<T> ClassQueryableAsyncWithWhere<T, C>(Expression<Func<T, bool>> whereString) where T : BaseEntity where C : class;
        IQueryable<T> BaseQueryableAsync<T>() where T : BaseEntity;

        Task<(bool Updated, string Message)> UpdateWithWhereNotCommited<T>(bool Async, T entity, Expression<Func<T, bool>> whereString) where T : BaseEntity;
        Task<(bool Updated, string Message)> UpdateWithWhereOverrideCreatedAt<T>(bool Async, T entity, Expression<Func<T, bool>> whereString) where T : BaseEntity;
        Task<(bool Deleted, string Message)> PhysicalDeleteWithWhereIgnoreFilter<T>(bool Async, Expression<Func<T, bool>> whereString) where T : BaseEntity;

        Task<string> GetMsgFromDBAsync(string code, params object[] args);

        string GetMsgFromDB(string code, params object[] args);

        Task<List<T>> ListAsync<T>() where T : BaseEntity;

        Task<List<T>> ListAsync<T>(int takeMaxRows = 0) where T : BaseEntity;
        Task<List<T>> ListAsync<T>(params Expression<Func<T, object>>[] includes) where T : BaseEntity;

        Task<List<T>> ListAsync<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity;
        Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;

        Task<List<T>> ListAsyncWithWhereIgnoreFilter<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
        List<T> ListWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
        IQueryable<T> QueryableAsyncWithWhereAsync<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
        Task<T> GetByIdAsync<T>(long id) where T : BaseEntity;

        Task<T> GetByIdAsyncIgnoreFilter<T>(long id) where T : BaseEntity;
        Task<T> GetByIdAsync<T>(string id) where T : BaseEntityIdString;
        Task<T> GetByIdAsync<T>(long id, params Expression<Func<T, object>>[] includes) where T : BaseEntity;
        Task<T> GetByIdAsync<T>(string id, params Expression<Func<T, object>>[] includes) where T : BaseEntityIdString;
        IQueryable<T> GetQueryable<T>() where T : BaseEntity;

        IQueryable<T> GetQueryable<T>(int takeMaxRows = 0) where T : BaseEntity;

        IQueryable<T> GetQueryable<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity;

        IQueryable<T> GetQueryable<T>(params Expression<Func<T, object>>[] includes) where T : BaseEntity;
        Task<IQueryable<T>> GetQueryableAsync<T>(int takeMaxRows = 0) where T : BaseEntity;
        Task<IQueryable<T>> GetQueryableAsync<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity;

        Task<IQueryable<T>> GetQueryableAsync<T>(params Expression<Func<T, object>>[] includes) where T : BaseEntity;
        (bool Added, string Message) Add<T>(T entity) where T : BaseEntity;
        Task<(bool Added, string Message)> AddAsync<T>(T entity) where T : BaseEntity;
        Task<(bool Added, string Message)> AddAsyncNotCommited<T>(T entity) where T : BaseEntity;
        Task<(bool Added, string Message)> AddManyAsync<T>(T entity) where T : BaseEntity;

        Task<(bool Added, string Message)> AddAsyncForUserAsync<T>(T entity) where T : BaseEntityIdString;

        Task<(bool Updated, string Message)> UpdateAsync<T>(T entity) where T : BaseEntity;

        Task<(bool Updated, string Message)> UpdateAsyncIgnoreFilter<T>(T entity) where T : BaseEntity;
        Task<(bool Updated, string Message)> UpdateManyAsync<T>(T entity) where T : BaseEntity;

        Task<(bool Updated, string Message)> UpdateManyAsyncIgnoreFilter<T>(T entity) where T : BaseEntity;

        Task<(bool Updated, string Message)> UpdateAsyncForUser<T>(T entity) where T : BaseEntityIdString;
        Task<(bool Deleted, string Message)> DeleteAsync<T>(long id) where T : BaseEntity;
        Task<(bool Deleted, string Message)> DeleteAsync<T>(string id) where T : BaseEntityIdString;

        Task<(bool Deleted, string Message)> DeleteAsync<T>(T entity) where T : BaseEntity;
        Task<(bool Commited, string Message)> CommitSync();
        (bool Commited, string Message) Commit();

        Task<(bool Commited, string Message)> CommitForUploadSync();



        long GetCountRowTableIgnoreFilter<T>(Expression<Func<T, bool>>? WhereString, Expression<Func<T, bool>>? GroupBy) where T : BaseEntity;

        
        Task<List<string>> DistinctAsync<T>(Expression<Func<T, string>> ColumnToDistinct) where T : BaseEntity;

        Task<List<string>> DistinctAsyncWithWhere<T>(Expression<Func<T, bool>> where, Expression<Func<T, string>> ColumnToDistinct) where T : BaseEntity;
        List<T> RawSqlQuery<T>(string query, Dictionary<string, string> param, Func<DbDataReader, T> map);
        List<T> DynamicParamQuery<T>(string query, DynamicParameters param = null, IDbTransaction Dbtransact = null,
               bool isStoredProcedure = true, CommandType CmdType = CommandType.StoredProcedure);
        Task<List<T>> DynamicParamQueryAsync<T>(string query, DynamicParameters param = null, IDbTransaction Dbtransact = null,
               bool isStoredProcedure = true, CommandType CmdType = CommandType.StoredProcedure);
        Task<List<T>> QueryAsync<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = false);

        Task<List<T>> QueryJobAsync<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = false);

        Task<(bool Added, string Message)> AddRangeAsync<T>(List<T> entities) where T : BaseEntity;
        Task<(bool Updated, string Message)> UpdateDataRangeAsync<T>(List<T> entities) where T : BaseEntity;
        (bool ExecRes, List<T> EntityRes) ExtendedRawSqlQuery<T>(bool JustExecute, System.Data.CommandType SqlCommandType,
            string query, Dictionary<string, string> param, Func<DbDataReader, T> map);
        bool AnyWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
       
        Task<List<object>> getMultiple(string sql, object parameters = null, bool isStoredProcedure = false, params Func<GridReader, object>[] readerFuncs);

        Task<IEnumerable<TRes>> executeProcedure<TRes>(string spName, object param, CommandType? commandType = null);
        Task ExecuteAsync(string spName, object param, CommandType? commandType = null);
        Task ExecuteScalarAsync(string spName, object param);
        List<T> Query<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true);
        List<T> QueryJob<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true);
        Task<(bool Generated, string Message)> excuteSp<T>(T itemDTO, string sp);

        Task<(bool Updated, string Message)> UpdateAsyncWithWhere<T>(T entity, Expression<Func<T, bool>> whereString) where T : BaseEntity;
        void Execute(string spName, object param, CommandType? commandType = null);        
        void ExecuteNotAsync(string spName, object param, CommandType? commandType = null);


        Task<List<object>> executeProcedureMultiple(string spName, object param, params Func<GridReader, object>[] readerFuncs);
        Task<List<object>> ExecuteQueryMultiple(string Query, object param, Int32 CommandTimeout = 12000, params Func<GridReader, object>[] readerFuncs);
        Task<string> GetSystemConfigFromDBAsync(string systemCategory, string systemSubCategory, string systemCode, params object[] args);
        string SpDebugBuilder(string SpName, Dictionary<string, object> Param, DynamicParameters DynamicParam, bool IsProcedure);
        string BuildStringFromClass<T>(string json, string Joiner) where T : class;
        Dictionary<string, string> GetPropertyFromClass<T>() where T : class;
        Task<(bool Deleted, string Message)> DeleteManyAsync<T>(long id) where T : BaseEntity;
        Task ExecuteQueryAsync(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true);


        IEnumerable<TRes> executeProcedureNonAsync<TRes>(string spName, object param, CommandType? commandType = null);
        List<object> executeProcedureMultipleNonAsync(string spName, object param, params Func<GridReader, object>[] readerFuncs);
        List<object> ExecuteQueryMultipleNonAsync(string Query, object param, Int32 CommandTimeout = 12000, params Func<GridReader, object>[] readerFuncs);
        Task<T> FirstAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
    }
}
