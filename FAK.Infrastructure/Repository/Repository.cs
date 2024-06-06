using Dapper;
using FAK.Domain.Entities;
using FAK.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using FAK.Persistance;
using AutoMapper;
using Newtonsoft.Json.Linq;
using FAK.Infrastructure.Services;

namespace FAK.Infrastructure.Repository
{
    public partial class Repository : IDisposable, IRepository
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<Logs> _logger;
        private readonly IMapper _mapper;
        private IDbContextTransaction _transaction;

        public Repository(AppDbContext dbContext, ILogger<Logs> logger, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
            _mapper = mapper;
        }


        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public async void BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

     
        public async Task<(bool Commited, string Message)> RollbackAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
                //var r = await _dbContext.SaveChangesAsync();
                string msgSuccess = null;

                msgSuccess = "Commited";

                // _logger.LogInformation(msgSuccess);
                return (true, "Commited");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message);
                return (false, "Trouble happened! \n" + ex.Message);
            }
        }

        public IQueryable<T> ClassQueryableAsyncWithWhere<T, C>(Expression<Func<T, bool>> whereString) where T : BaseEntity where C : class
        {
            return _dbContext.Set<T>().AsQueryable<T>().Where(whereString);
        }
        public IQueryable<T> BaseQueryableAsync<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().AsQueryable<T>();
        }

        
        public void Execute(string spName, object param, CommandType? commandType = null)
        {
            _dbContext.Database.GetDbConnection().Execute(spName, param, commandType:
               commandType ?? CommandType.StoredProcedure);
        }
        public async Task<List<T>> ListAsync<T>(int takeMaxRows = 0) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            if (takeMaxRows > 0)
            {
                query = query.Take(takeMaxRows).OrderBy(o => o.Id);
            }
            else
            {
                var Countop = ListAsyncWithWhere<T>(a => a.Id == a.Id).ConfigureAwait(true).GetAwaiter().GetResult().Count();
                query = query.Take(Countop).OrderBy(o => o.Id);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsync<T>() where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsync<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.EagerLoadInclude(includes);
            if (takeMaxRows > 0)
            {
                query = query.Take(takeMaxRows).OrderBy(o => o.Id);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsync<T>(params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.EagerLoadInclude(includes);
            return await query.ToListAsync();
        }

        public async Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.Where(whereString);

            return await query.ToListAsync();
        }
        public async Task<T> FirstAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.Where(whereString);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<T>> ListAsyncWithWhereIgnoreFilter<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var query = _dbContext.Set<T>().IgnoreQueryFilters().AsNoTracking() as IQueryable<T>;
            query = query.Where(whereString);
            return await query.ToListAsync();
        }

        public List<T> ListWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.Where(whereString);

            return query.ToList();
        }

        public IQueryable<T> QueryableAsyncWithWhereAsync<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.Where(whereString);

            return query;
        }

        public async Task<T> GetByIdAsync<T>(string id) where T : BaseEntityIdString
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.Where(e => e.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id, params Expression<Func<T, object>>[] includes) where T : BaseEntityIdString
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.EagerLoadInclude(includes).Where(i => i.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync<T>(long id) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.Where(i => i.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsyncIgnoreFilter<T>(long id) where T : BaseEntity
        {
            var query = _dbContext.Set<T>().IgnoreQueryFilters().AsNoTracking() as IQueryable<T>;
            query = query.Where(i => i.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync<T>(long id, params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var query = _dbContext.Set<T>() as IQueryable<T>;
            query = query.EagerLoadInclude(includes).Where(i => i.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<T> GetQueryable<T>() where T : BaseEntity
        {
            var rItem = _dbContext.Set<T>().AsQueryable<T>();
            return rItem;
        }

        public IQueryable<T> GetQueryable<T>(int takeMaxRows = 0) where T : BaseEntity
        {
            var rItem = _dbContext.Set<T>().AsQueryable<T>();
            if (takeMaxRows > 0)
            {
                rItem = rItem.Take(takeMaxRows).OrderBy(o => o.Id);
            }
            return rItem;
        }

        public async Task<IQueryable<T>> GetQueryableAsync<T>(int takeMaxRows = 0) where T : BaseEntity
        {
            var rItem = await _dbContext.Set<T>().AsQueryable<T>().ToListAsync<T>();
            if (takeMaxRows > 0)
            {
                rItem = rItem.Take(takeMaxRows).OrderBy(o => o.Id).ToList();
            }
            return rItem.AsQueryable<T>();
        }

        public async Task<IQueryable<T>> GetQueryableAsync<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var rItem = _dbContext.Set<T>().AsQueryable<T>();
            var rItemIncludes = await rItem.EagerLoadInclude(includes).ToListAsync<T>();
            if (takeMaxRows > 0)
            {
                rItemIncludes = rItemIncludes.Take(takeMaxRows).OrderBy(o => o.Id).ToList();
            }
            return rItemIncludes.AsQueryable<T>();
        }

        public IQueryable<T> GetQueryable<T>(int takeMaxRows = 0, params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var rItem = _dbContext.Set<T>().AsQueryable<T>();
            rItem = rItem.EagerLoadInclude(includes);
            if (takeMaxRows > 0)
            {
                rItem = rItem.Take(takeMaxRows).OrderBy(o => o.Id);
            }
            return rItem;
        }

        public async Task<IQueryable<T>> GetQueryableAsync<T>(params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var rItem = _dbContext.Set<T>().AsQueryable<T>();
            var rItemIncludes = await rItem.EagerLoadInclude(includes).ToListAsync<T>();
            return rItemIncludes.AsQueryable<T>();
        }

        public IQueryable<T> GetQueryable<T>(params Expression<Func<T, object>>[] includes) where T : BaseEntity
        {
            var rItem = _dbContext.Set<T>().AsQueryable<T>();
            rItem = rItem.EagerLoadInclude(includes);
            return rItem;
        }

        public async Task<string> GetMsgFromDBAsync(string code, params object[] args)
        {
            try
            {
                if (_dbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    _dbContext.Database.GetDbConnection().Open();
                }
                var r = "";
                string query = @"SELECT  mm.MessageText FROM dbo.MasterMessage AS mm WHERE mm.MessageCode=@MessageCode and mm.IsDeleted=0 ";
                var _MessageText = (List<string>)await executeProcedure<string>(query, new { MessageCode = code }, CommandType.Text);
                var q = _MessageText.FirstOrDefault();
                if (q == null)
                {
                    return r;
                }
                else
                {
                    q = q.Replace("[", "{").Replace("]", "}");
                    if (args != null && args.Length > 0)
                    {
                        r = string.Format(q, args);
                    }
                    else
                    {
                        r = q;
                    }
                }
                return r;
            }
            catch (System.Exception ex)
            {

                if (ex.InnerException != null) { return ("Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return args[0].ToString();
                    //return ("Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<string> GetSystemConfigFromDBAsync(string systemCategory, string systemSubCategory, string systemCode, params object[] args)
        {
            try
            {
                if (_dbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                {
                    _dbContext.Database.GetDbConnection().Open();
                }
                var r = "";
                string query = @"SELECT msc.SystemValue FROM dbo.MasterSystemConfig AS msc WHERE msc.SystemCategory=@SystemCategory AND msc.SystemSubCategory=@SystemSubCategory AND msc.SystemCode=@SystemCode AND msc.Active=1 AND msc.IsDeleted=0 ";
                var _SystemValue = (List<string>)await executeProcedure<string>(query, new { SystemCategory = systemCategory, SystemSubCategory = systemSubCategory, SystemCode = systemCode }, CommandType.Text);
                var q = _SystemValue.FirstOrDefault();
                if (q == null)
                {
                    return r;
                }
                else
                {
                    q = q.Replace("[", "{").Replace("]", "}");
                    if (args != null && args.Length > 0)
                    {
                        r = string.Format(q, args);
                    }
                    else
                    {
                        r = q;
                    }
                }
                return r;
            }
            catch (System.Exception ex)
            {

                if (ex.InnerException != null) { return ("Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    // return ("Trouble happened! \n" + ex.Message);
                    return args[0].ToString();
                }
            }
        }

        public string GetMsgFromDB(string code, params object[] args)
        {
            var r = "";
            var query = _dbContext.Set<MasterMessage>() as IQueryable<MasterMessage>;
            query = query.Where(w => w.MessageCode == code && w.Active == true);

            var q = query.Select(s => s.MessageText).FirstOrDefault();
            if (q == null)
            {
                return r;
            }
            else
            {
                q = q.Replace("[", "{").Replace("]", "}");
                if (args != null && args.Length > 0)
                {
                    r = string.Format(q, args);
                }
                else
                {
                    r = q;
                }
            }
            return r;
        }
        

        public async Task<(bool Added, string Message)> AddManyAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                await _dbContext.Set<T>().AddAsync(entity);

                //  var msgSuccess = await GetMsgFromDBAsync("MWOSSTD009I", entity.Id);

                //  _logger.LogInformation(msgSuccess + ", " + entity);

                return (true, "Data successfully added, but not commited yet!");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public (bool Added, string Message) AddMany<T>(T entity) where T : BaseEntity
        {
            try
            {
                _dbContext.Set<T>().Add(entity);

                //var msgSuccess = GetMsgFromDB("MWOSSTD009I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);

                return (true, "Data successfully");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public (bool Commited, string Message) Commit()
        {
            try
            {
                var r = _dbContext.SaveChanges();
                string msgSuccess = null;
                msgSuccess = "Commited";
                return (true, msgSuccess);
            }
            catch (DbUpdateException ex1)
            {
                var LastEntries = ex1.Entries.LastOrDefault();
                var ValueExisting = LastEntries.Metadata.Model.FindEntityType(LastEntries.Metadata.ClrType).GetProperties().OrderBy(a => a.Name).ToList();
                int ErrorPossition = 0;
                string Emptys = string.Empty;
                string JoinErrMsg = string.Empty;
                foreach (var aa in ValueExisting)
                {
                    var ColumnTypeOrder = LastEntries.CurrentValues.Properties.OrderBy(a => a.Name).ToList();  //.Properties [ErrorPossition]
                    var ColumnType = ColumnTypeOrder[ErrorPossition].FieldInfo.FieldType.Name;
                    try
                    {
                        var PassValue = LastEntries.CurrentValues[ValueExisting[ErrorPossition].Name];
                        var Eml = ValueExisting[ErrorPossition].GetMaxLength();
                        if ((PassValue != null ? PassValue.ToString().Length : 0) > Eml)
                        {
                            JoinErrMsg += @"Column( " + aa.Name + " )|Acceptable Length( " + Eml + " )" + " , ";
                        }
                    }
                    catch (Exception)
                    {
                    }

                    ErrorPossition++;
                }

                var DetailError = "Action( " + LastEntries.State + " )|Table( " + LastEntries.Metadata.ClrType.Name + " )|" + JoinErrMsg;
                if (ex1.InnerException != null)
                {
                    return (false, "Error! : " + ex1.Message + ",Detail : " + DetailError + ", InnerMsg : " + ex1.InnerException.Message);
                }
                else
                {
                    return (false, "Error! : " + ex1.Message + ",Detail : " + DetailError);
                }
            }
            catch (NotSupportedException ex4)
            {
                if (ex4.InnerException != null) { return (false, "Trouble happened! \n" + ex4.Message + "\n" + ex4.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex4.Message);
                }
            }
            catch (ObjectDisposedException ex5)
            {
                if (ex5.InnerException != null) { return (false, "Trouble happened! \n" + ex5.Message + "\n" + ex5.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex5.Message);
                }
            }
            catch (InvalidOperationException ex6)
            {
                if (ex6.InnerException != null) { return (false, "Trouble happened! \n" + ex6.Message + "\n" + ex6.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex6.Message);
                }
            }

        }
        public async Task<(bool Commited, string Message)> CommitSync()
        {
            try
            {
                var r = await _dbContext.SaveChangesAsync();
                //  string msgSuccess = null;

                // msgSuccess = "Commited";

                // _logger.LogInformation(msgSuccess);
                return (true, "Commited");
            }
            catch (DbUpdateException ex1)
            {
                var LastEntries = ex1.Entries.LastOrDefault();
                var ValueExisting = LastEntries.Metadata.Model.FindEntityType(LastEntries.Metadata.ClrType).GetProperties().OrderBy(a => a.Name).ToList();
                int ErrorPossition = 0;
                string Emptys = string.Empty;
                string JoinErrMsg = string.Empty;
                foreach (var aa in ValueExisting)
                {
                    var ColumnTypeOrder = LastEntries.CurrentValues.Properties.OrderBy(a => a.Name).ToList();  //.Properties [ErrorPossition]
                    var ColumnType = ColumnTypeOrder[ErrorPossition].FieldInfo.FieldType.Name;
                    try
                    {
                        var PassValue = LastEntries.CurrentValues[ValueExisting[ErrorPossition].Name];
                        var Eml = ValueExisting[ErrorPossition].GetMaxLength();
                        if ((PassValue != null ? PassValue.ToString().Length : 0) > Eml)
                        {
                            JoinErrMsg += @"Column( " + aa.Name + " )|Acceptable Length( " + Eml + " )" + " , ";
                        }
                    }
                    catch (Exception)
                    {
                    }

                    ErrorPossition++;
                }

                var DetailError = "Action( " + LastEntries.State + " )|Table( " + LastEntries.Metadata.ClrType.Name + " )|" + JoinErrMsg;
                if (ex1.InnerException != null)
                {
                    return (false, "Error! : " + ex1.Message + ",Detail : " + DetailError + ", InnerMsg : " + ex1.InnerException.Message);
                }
                else
                {
                    return (false, "Error! : " + ex1.Message + ",Detail : " + DetailError);
                }
            }
            catch (NotSupportedException ex4)
            {
                if (ex4.InnerException != null) { return (false, "Trouble happened! \n" + ex4.Message + "\n" + ex4.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex4.Message);
                }
            }
            catch (ObjectDisposedException ex5)
            {
                if (ex5.InnerException != null) { return (false, "Trouble happened! \n" + ex5.Message + "\n" + ex5.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex5.Message);
                }
            }
            catch (InvalidOperationException ex6)
            {
                if (ex6.InnerException != null) { return (false, "Trouble happened! \n" + ex6.Message + "\n" + ex6.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex6.Message);
                }
            }

        }

        public async Task<(bool Commited, string Message)> CommitForUploadSync()
        {
            try
            {
                var r = await _dbContext.SaveChangesAsync();
                //string msgSuccess = null;

                //msgSuccess = await GetMsgFromDBAsync("MWOSSTD010I") + "| " + r + " Row Processed";

                //_logger.LogInformation(msgSuccess);

                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message + "|0");
                }
            }
        }
        public (bool Added, string Message) Add<T>(T entity) where T : BaseEntity
        {
            try
            {
                _dbContext.Set<T>().Add(entity);

                _dbContext.SaveChanges();
                //var msgSuccess = GetMsgFromDB("MWOSSTD011I", entity.Id);
                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Added, string Message)> AddAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                _dbContext.Set<T>().Add(entity);

                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD011I", entity.Id);
                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Added, string Message)> AddAsyncNotCommited<T>(T entity) where T : BaseEntity
        {
            try
            {
                _dbContext.Set<T>().Add(entity);

                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD011I", entity.Id);
                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public (bool Added, string Message) AddAsyncForUser<T>(T entity) where T : BaseEntityIdString
        {
            try
            {
                _dbContext.Set<T>().Add(entity);

                _dbContext.SaveChanges();
                //var msgSuccess = GetMsgFromDB("MWOSSTD011I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                return (true, "Success");
                //return (true, msgSuccess);
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Added, string Message)> AddAsyncForUserAsync<T>(T entity) where T : BaseEntityIdString
        {
            try
            {
                _dbContext.Set<T>().Add(entity);

                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD011I", entity.Id);
                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);

                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Updated, string Message)> UpdateAsyncWithWhere<T>(T entity, Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().Where(whereString).SingleOrDefaultAsync();

                _dbContext.Entry(item).State = EntityState.Modified;
                entity.Id = item.Id;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                //await _dbContext.SaveChangesAsync(); Punya Agassi jangan Diaktivin ,pasangin sama Commitasync
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Updated, string Message)> UpdateWithWhereOverrideCreatedAt<T>(bool Async, T entity, Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            try
            {
                T item = null;
                if (Async)
                {
                    item = await _dbContext.Set<T>().Where(whereString).SingleOrDefaultAsync();
                }
                else
                {
                    item = _dbContext.Set<T>().Where(whereString).SingleOrDefault();
                }
                _dbContext.Entry(item).State = EntityState.Modified;
                entity.Id = item.Id;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", entity.Id);
                //return (true, msgSuccess);

                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Updated, string Message)> UpdateWithWhereNotCommited<T>(bool Async, T entity, Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            try
            {
                T item = null;
                if (Async)
                {
                    item = await _dbContext.Set<T>().Where(whereString).SingleOrDefaultAsync();
                }
                else
                {
                    item = _dbContext.Set<T>().Where(whereString).SingleOrDefault();
                }
                _dbContext.Entry(item).State = EntityState.Modified;
                entity.Id = item.Id;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", entity.Id);
                //return (true, msgSuccess);

                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Updated, string Message)> UpdateAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == entity.Id);

                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);

                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Updated, string Message)> UpdateAsyncIgnoreFilter<T>(T entity) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().IgnoreQueryFilters().SingleOrDefaultAsync(e => e.Id == entity.Id);

                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);

                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Updated, string Message)> UpdateManyAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == entity.Id);
                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Updated, string Message)> UpdateManyAsyncIgnoreFilter<T>(T entity) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().IgnoreQueryFilters().SingleOrDefaultAsync(e => e.Id == entity.Id);
                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                var msgSuccess = "Success Updating";//await GetMsgFromDBAsync("MWOSSTD013I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                return (true, msgSuccess);
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Updated, string Message)> UpdateAsyncForUser<T>(T entity) where T : BaseEntityIdString
        {
            try
            {
                var item = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == entity.Id);

                _dbContext.Entry(item).State = EntityState.Modified;
                _dbContext.Entry(item).CurrentValues.SetValues(entity);
                _dbContext.Entry(item).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(item).Property(x => x.CreatedAt).IsModified = false;
                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MSG003G", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }





        public async Task<(bool Deleted, string Message)> DeleteAsync<T>(long id) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);

                _dbContext.Remove<T>(item);

                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD031I", id);

                //_logger.LogInformation(msgSuccess);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Deleted, string Message)> DeleteManyAsync<T>(long id) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);

                _dbContext.Remove<T>(item);
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD031I", id);

                //_logger.LogInformation(msgSuccess);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Deleted, string Message)> PhysicalDeleteWithWhereIgnoreFilter<T>(bool Async, Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            try
            {
                List<T> item = null;

                if (Async)
                {
                    item = await _dbContext.Set<T>().IgnoreQueryFilters().AsNoTracking().Where(whereString).ToListAsync();
                }
                else
                {
                    item = _dbContext.Set<T>().IgnoreQueryFilters().AsNoTracking().Where(whereString).ToList();
                }
                if (item.Count < 1)
                {
                    return (true, "Perhatian !,Data Tidak Ditemukan Dan Dilewatkan ,Mungkin Data Yang Dicari Adalah Data Baru");
                }
                else
                {
                    _dbContext.RemoveRange(item);
                    //var msgSuccess = GetMsgFromDBAsync("MWOSSTD031I", item.FirstOrDefault().Id).ConfigureAwait(true).GetAwaiter().GetResult();
                    //return (true, msgSuccess);
                    return (true, "Success");
                }


            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Deleted, string Message)> DeleteAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                var item = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == entity.Id);

                _dbContext.Remove<T>(item);

                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD031I", entity.Id);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                //return (true, msgSuccess);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Deleted, string Message)> DeleteAsync<T>(string id) where T : BaseEntityIdString
        {
            try
            {
                var item = await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id);

                _dbContext.Remove<T>(item);

                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD031I", id);

                //_logger.LogInformation("Data successfully deleted");
                return (true, "Data successfully deleted");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public long GetCountRowTableIgnoreFilter<T>(Expression<Func<T, bool>>? WhereString, Expression<Func<T, bool>>? GroupBy) where T : BaseEntity
        {
            return _dbContext.Set<T>().IgnoreQueryFilters().AsNoTracking().Where(WhereString).GroupBy(GroupBy).Count();
        }

        

        public async Task<List<string>> DistinctAsync<T>(Expression<Func<T, string>> ColumnToDistinct) where T : BaseEntity
        {
            try
            {
                var query = _dbContext.Set<T>() as IQueryable<T>;
                var query2 = await query.Select(ColumnToDistinct).Distinct().ToListAsync();
                return query2;
            }
            catch (Exception ex)
            {
                throw new Exception("Trouble happened! \n" + ex.Message);
            }
        }
        public async Task<List<string>> DistinctAsyncWithWhere<T>(Expression<Func<T, bool>> where, Expression<Func<T, string>> ColumnToDistinct) where T : BaseEntity
        {
            try
            {
                var query = _dbContext.Set<T>() as IQueryable<T>;
                var queryWhere = query.Where(where) as IQueryable<T>;

                var QueryRes = await queryWhere.Select(ColumnToDistinct).Distinct().ToListAsync();
                return QueryRes;
            }
            catch (Exception ex)
            {
                throw new Exception("Trouble happened! \n" + ex.Message);
            }
        }
        public List<T> RawSqlQuery<T>(string query, Dictionary<string, string> param, Func<DbDataReader, T> map)
        {
            var entities = new List<T>();

            //using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            //{

            var command = _dbContext.Database.GetDbConnection().CreateCommand();

            command.CommandText = query;

            command.CommandType = CommandType.StoredProcedure;

            foreach (var p in param)
            {
                command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter(p.Key, p.Value));
            }
            command.CommandTimeout = 0;

            _dbContext.Database.OpenConnection();

            //using (var result = command.ExecuteReader())
            //{
            var result = command.ExecuteReader();
            while (result.Read())
            {
                entities.Add(map(result));
            }

            return entities;
            //}
            //}
        }
        public async Task<List<T>> DynamicParamQueryAsync<T>(string query, DynamicParameters param = null, IDbTransaction Dbtransact = null,
               bool isStoredProcedure = true, CommandType CmdType = CommandType.StoredProcedure)
        {
            return (await _dbContext.Database.GetDbConnection().QueryAsync<T>(query, param, Dbtransact, commandTimeout: 300, isStoredProcedure ?
                CommandType.StoredProcedure : CmdType == CommandType.StoredProcedure ? CommandType.Text : CmdType)).ToList();
        }
        public List<T> DynamicParamQuery<T>(string query, DynamicParameters param = null, IDbTransaction Dbtransact = null,
               bool isStoredProcedure = true, CommandType CmdType = CommandType.StoredProcedure)
        {
            return _dbContext.Database.GetDbConnection().Query<T>(query, param, Dbtransact, commandTimeout: 120).ToList();
        }
        public async Task<List<T>> QueryAsync<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true)
        {
            if (isStoredProcedure)
            {
                return (await _dbContext.Database.GetDbConnection().QueryAsync<T>(query, param, commandTimeout: 120, commandType: CommandType.StoredProcedure)).ToList();
            }

            return (await _dbContext.Database.GetDbConnection().QueryAsync<T>(query, param, commandTimeout: 120)).AsList();
        }
        public async Task<List<T>> QueryJobAsync<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true)
        {
            if (isStoredProcedure)
            {
                return (await _dbContext.Database.GetDbConnection().QueryAsync<T>(query, param, commandTimeout: 5000, commandType: CommandType.StoredProcedure)).ToList();
            }

            return (await _dbContext.Database.GetDbConnection().QueryAsync<T>(query, param, commandTimeout: 5000)).AsList();
        }

        public async Task ExecuteQueryAsync(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true)
        {
            if (isStoredProcedure)
            {
                await _dbContext.Database.GetDbConnection().ExecuteAsync(query, param, commandTimeout: 120, commandType: CommandType.StoredProcedure);
            }

            await _dbContext.Database.GetDbConnection().ExecuteAsync(query, param, commandTimeout: 120);
        }
        public async Task<IEnumerable<TRes>> executeProcedure<TRes>(string spName, object param, CommandType? commandType = null)
        {
            return await _dbContext.Database.GetDbConnection().QueryAsync<TRes>(spName, param, commandTimeout: 500, commandType:
                commandType ?? CommandType.StoredProcedure);
        }
        public IEnumerable<TRes> executeProcedureNonAsync<TRes>(string spName, object param, CommandType? commandType = null)
        {
            return _dbContext.Database.GetDbConnection().Query<TRes>(spName, param, commandTimeout: 120, commandType:
                commandType ?? CommandType.StoredProcedure);
        }
        public async Task<List<object>> executeProcedureMultiple(string spName, object param, params Func<GridReader, object>[] readerFuncs)
        {
            //var conn = _dbContext.Database.GetDbConnection();f
            var returnResults = new List<object>();

            var gridReader = await _dbContext.Database.GetDbConnection().QueryMultipleAsync(spName, param, commandTimeout: 5000, commandType: CommandType.StoredProcedure);

            foreach (var readerFunc in readerFuncs)
            {
                var obj = readerFunc(gridReader);
                returnResults.Add(obj);
            }


            return returnResults;
        }
        public List<object> executeProcedureMultipleNonAsync(string spName, object param, params Func<GridReader, object>[] readerFuncs)
        {
            //var conn = _dbContext.Database.GetDbConnection();
            var returnResults = new List<object>();

            var gridReader = _dbContext.Database.GetDbConnection().QueryMultiple(spName, param, commandTimeout: 5000, commandType: CommandType.StoredProcedure);

            foreach (var readerFunc in readerFuncs)
            {
                var obj = readerFunc(gridReader);
                returnResults.Add(obj);
            }


            return returnResults;
        }
        public async Task<List<object>> ExecuteQueryMultiple(string Query, object param, Int32 CommandTimeout = 12000, params Func<GridReader, object>[] readerFuncs)
        {
            //var conn = _dbContext.Database.GetDbConnection();
            var returnResults = new List<object>();

            var gridReader = await _dbContext.Database.GetDbConnection().QueryMultipleAsync(Query, param, commandTimeout: CommandTimeout, commandType: CommandType.Text);

            foreach (var readerFunc in readerFuncs)
            {
                var obj = readerFunc(gridReader);
                returnResults.Add(obj);
            }


            return returnResults;
        }
        public List<object> ExecuteQueryMultipleNonAsync(string Query, object param, Int32 CommandTimeout = 12000, params Func<GridReader, object>[] readerFuncs)
        {
            //var conn = _dbContext.Database.GetDbConnection();
            var returnResults = new List<object>();

            var gridReader = _dbContext.Database.GetDbConnection().QueryMultiple(Query, param, commandTimeout: CommandTimeout, commandType: CommandType.Text);

            foreach (var readerFunc in readerFuncs)
            {
                var obj = readerFunc(gridReader);
                returnResults.Add(obj);
            }


            return returnResults;
        }
        public async Task ExecuteAsync(string spName, object param, CommandType? commandType = null)
        {
            await _dbContext.Database.GetDbConnection().ExecuteAsync(spName, param, commandTimeout: 500, commandType:
                commandType ?? CommandType.StoredProcedure);
        }
        public void ExecuteNotAsync(string spName, object param, CommandType? commandType = null)
        {
            _dbContext.Database.GetDbConnection().Execute(spName, param, commandTimeout: 120, commandType:
               commandType ?? CommandType.StoredProcedure);
        }
        public async Task ExecuteScalarAsync(string spName, object param)
        {

            await _dbContext.Database.GetDbConnection().ExecuteScalarAsync(spName, param);
        }


        public async Task<List<object>> getMultiple(string sql, object parameters = null, bool isStoredProcedure = false, params Func<GridReader, object>[] readerFuncs)
        {
            var returnResults = new List<object>();
            var gridReader = await _dbContext.Database.GetDbConnection().QueryMultipleAsync(sql, parameters, null, commandTimeout: 120, (isStoredProcedure ? CommandType.StoredProcedure : null));
            foreach (var readerFunc in readerFuncs)
            {
                var obj = readerFunc(gridReader);
                returnResults.Add(obj);
            }
            return returnResults;
        }


        public List<T> Query<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true)
        {
            if (isStoredProcedure)
            {
                return (_dbContext.Database.GetDbConnection().Query<T>(query, param, commandType: CommandType.StoredProcedure)).ToList();
            }
            return (_dbContext.Database.GetDbConnection().Query<T>(query, param).ToList());
        }

        public List<T> QueryJob<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = true)
        {
            if (isStoredProcedure)
            {
                return (_dbContext.Database.GetDbConnection().Query<T>(query, param, commandTimeout: 5000, commandType: CommandType.StoredProcedure)).ToList();
            }
            return (_dbContext.Database.GetDbConnection().Query<T>(query, param, commandTimeout: 5000).ToList());
        }

        public (bool ExecRes, List<T> EntityRes) ExtendedRawSqlQuery<T>(bool JustExecute, CommandType SqlCommandType,
            string query, Dictionary<string, string> param, Func<DbDataReader, T> map)
        {
            var entities = new List<T>();

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = SqlCommandType;

                foreach (var p in param)
                {
                    command.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter(p.Key, p.Value));
                }
                command.CommandTimeout = 0;

                _dbContext.Database.OpenConnection();
                if (JustExecute)
                {
                    var Result = command.ExecuteNonQuery();
                    return (Result == 0 ? true : false, null);
                }
                else
                {
                    using (var result = command.ExecuteReader())
                    {
                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                entities.Add(map(result));
                            }
                        }
                        else
                        {
                            entities.Add(map(null));
                        }

                        return (true, entities);

                    }
                }
            }
        }

        public async Task<(bool Updated, string Message)> UpdateBulkAsync<T>(IList<T> entity) where T : BaseEntity
        {

            try
            {

                await _dbContext.SaveChangesAsync();
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", entity);

                //_logger.LogInformation(msgSuccess + ", " + entity);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public async Task<(bool Updated, string Message)> UpdateDataRangeAsync<T>(List<T> entities) where T : BaseEntity
        {
            try
            {
                _dbContext.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();
                //var id = string.Join(',', entities.Select(x => x.Id));
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD013I", id);
                //_logger.LogInformation(msgSuccess + ", " + entities);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }

        public async Task<(bool Added, string Message)> AddRangeAsync<T>(List<T> entities) where T : BaseEntity
        {
            try
            {
                _dbContext.Set<T>().AddRange(entities);

                await _dbContext.SaveChangesAsync();
                //var id = string.Join(',', entities.Select(x => x.Id));
                //var msgSuccess = await GetMsgFromDBAsync("MWOSSTD011I", id);
                //_logger.LogInformation(msgSuccess + ", " + id);
                return (true, "Success");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null) { return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message); }
                else
                {
                    return (false, "Trouble happened! \n" + ex.Message);
                }
            }
        }
        public bool AnyWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            var q = _dbContext.Set<T>().Where(whereString).Any();
            return q;
        }
        
        //var WhereFunction = new Dictionary<int, Expression<Func<UserMaintenanceList, string>>>
        
        public string SpDebugBuilder(string SpName, Dictionary<string, object> Paramz, DynamicParameters DynamicParam, bool IsProcedure)
        {
            Dictionary<string, object> Param = new Dictionary<string, object>();
            if (Paramz == null)
            {
                DynamicParam.ParameterNames.ToList().ForEach(a => { Param.Add(a, DynamicParam.Get<object>(a)); });
            }

            if (IsProcedure)
            {
                string Out = "Exec " + SpName + "";
                Param.ToList().ForEach(a =>
                {
                    Out += string.Format("{0}{1}{0},", Convert.ToChar(39), a.Value);
                }
                );
                return Out.Substring(0, Out.Length - 1);
            }
            else
            {
                Param.ToList().ForEach(a =>
                {
                    SpName = SpName.Replace(a.Key != null ? a.Key.ToString() : string.Empty, a.Value != null ? a.Value.ToString() : string.Empty, StringComparison.InvariantCultureIgnoreCase);
                }
                );
                return SpName;
            }
        }
        public string BuildStringFromClass<T>(string json, string Joiner) where T : class
        {
            string value = string.Empty;
            try
            {

                Type typeParameterType = typeof(T);
                var c = typeParameterType.GetProperties();

                long i = 0;
                foreach (var item in c)
                {

                    string adds = JObject.Parse(json)[item.Name].ToString();
                    if (i < 1)
                    {

                        value = adds;
                    }
                    else
                    {
                        value += Joiner + adds;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return value;
        }

        public Dictionary<string, string> GetPropertyFromClass<T>() where T : class
        {
            var value = new Dictionary<string, string>();
            try
            {

                Type typeParameterType = typeof(T);
                var c = typeParameterType.GetProperties();

                long i = 0;
                foreach (var item in c)
                {

                    value.Add(item.Name, item.PropertyType.Name);
                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return value;
        }
        public async Task<(bool Generated, string Message)> excuteSp<T>(T itemDTO, string sp)
        {



            var reader = await _dbContext.Database.GetDbConnection().ExecuteAsync(sp, itemDTO, commandType: CommandType.StoredProcedure);

            var msgSuccess = await GetMsgFromDBAsync("MWOSSTD012I");

            _logger.LogInformation(msgSuccess);
            //_dbContext.Database.GetDbConnection().Close();
            return (true, msgSuccess);

            //}
        }

        private bool disposed = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
