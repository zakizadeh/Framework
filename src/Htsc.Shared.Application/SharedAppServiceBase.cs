using Abp.Application.Services;
using Abp.Dependency;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Htsc.Shared.ExcelHelper;
using Htsc.Shared.Configuration;

namespace Htsc.Shared
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class SharedAppServiceBase : ApplicationService
    {
        #region Field
        protected readonly IIocManager IocManager;
        #endregion

        #region Ctor
        protected SharedAppServiceBase(IIocManager iocManager)
        {
            IocManager = iocManager;
            LocalizationSourceName = SharedConsts.LocalizationSourceName;
        }
        #endregion
       
        #region Filter




        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }





        #endregion



        #region OrderBy

        public Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            if (!typeof(TSource).GetProperties().Any(x => x.Name.Equals(propertyName)))
                propertyName = typeof(TSource).GetProperties().FirstOrDefault().Name;

            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(object));

            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

        public IOrderedQueryable<TSource>
        OrderBy<TSource>(IQueryable<TSource> source, string propertyName, bool isAsc = true)
        {
            if (isAsc)
                return source.OrderBy(GetExpression<TSource>(propertyName));
            return source.OrderByDescending(GetExpression<TSource>(propertyName));
        }
        #endregion

        #region Map
        private const string isNewItemConst = "isNewItem";
        protected virtual TEntity MapJsonToEntity<TEntity>(TEntity entity, string jsonValues, out bool isNewItem)
        {
            var dto = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonValues);
            if (dto.ContainsKey(isNewItemConst))
            {
                bool.TryParse(dto[isNewItemConst].ToString(), out isNewItem);
                dto.Remove(isNewItemConst);
            }
            else
            {
                isNewItem = false;
            }
            var model = JsonConvert.DeserializeObject<TEntity>(jsonValues);
            var type = model.GetType();
            foreach (var kv in dto)
            {
                var first = char.ToUpper(kv.Key[0]).ToString();
                var key = kv.Key.Remove(0, 1);
                key = key.Insert(0, first);
                var value = type.GetProperty(key).GetValue(model);
                type.GetProperty(key).SetValue(entity, value);
            }

            return entity;
        }
        #endregion

        #region GetAll for Dapper
        public IEnumerable<T> GetAll<T>(string rawQuery)
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            var configuration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            string connection = configuration.GetConnectionString(SharedConsts.ConnectionStringName);
            using (IDbConnection db = new SqlConnection(connection))
            {
                var result = db.Query<T>(rawQuery, null, null, true, SharedConsts.Timeout);
                return result;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string rawQuery, string productionName = SharedConsts.EnvironmentProductionName)
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            var configuration = AppConfigurations.Get(coreAssemblyDirectoryPath, productionName);
            string connection = configuration.GetConnectionString(SharedConsts.ConnectionStringName);
            using (IDbConnection db = new SqlConnection(connection))
            {
                var result = await db.QueryAsync<T>(rawQuery, null, null, SharedConsts.Timeout, null);
                return result;
            }
        }
        #endregion


        /// <summary>
        /// خواندن فایل از کش
        /// </summary>
        /// <returns></returns>
        protected (byte[], string) ReadFileFromCache(string key)
        {
            var tempFileCacheManager = IocManager.Resolve<ITempFileCacheManager>();

            return tempFileCacheManager.GetFileWithName(key);

        }
    }
}