using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShephardTech.Financials.Domain.Extensions
{
    public static class SprocRepositoryExtensions
    {
        /// <summary>
        /// Creates an initial DbCommand object based on a stored procedure name
        /// </summary>
        /// <param name="context">target database context</param>
        /// <param name="storedProcName">target procedure name</param>
        /// <param name="prependDefaultSchema">Prepend the default schema name to <paramref name="storedProcName"/> if explicitly defined in <paramref name="context"/></param>
        /// <param name="commandTimeout">Command timeout in seconds. Default is 30.</param>
        /// <returns></returns>
        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName,
            bool prependDefaultSchema = true, short commandTimeout = 30)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = commandTimeout;

            if (prependDefaultSchema)
            {
                var schemaName = context.Model["DefaultSchema"];
                if (schemaName != null)
                {
                    storedProcName = $"{schemaName}.{storedProcName}";
                }
            }

            cmd.CommandText = storedProcName;
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }

        public static DbCommand LoadSqlQuery(this DbContext context, string cmdText,
            bool prependDefaultSchema = true, short commandTimeout = 30)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = commandTimeout;

            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;

            return cmd;
        }


        /// <summary>
        /// Creates a DbParameter object and adds it to a DbCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="configureParam"></param>
        /// <returns></returns>
        public static DbCommand WithSqlParam(this DbCommand cmd, params (string, object)[] nameValues)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != System.Data.CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            foreach (var item in nameValues)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = item.Item1;
                param.Value = item.Item2 ?? DBNull.Value;
                cmd.Parameters.Add(param);
            }

            return cmd;
        }



        public static async Task<List<T>> ExecuteStoreProcedure<T>(this DbCommand command) where T : class
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    await command.Connection.OpenAsync();

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.MapToList<T>();
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        static List<T> MapToList<T>(this DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            var colMapping = dr.GetColumnSchema()
                                .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                                .ToDictionary(key => key.ColumnName.ToLower());



            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var item in props)
                    {
                        if (!item.CanWrite) continue;
                        var val = dr.GetValue(colMapping[item.Name.ToLower()].ColumnOrdinal.Value);
                        item.SetValue(obj, val == DBNull.Value ? null : val);
                    }

                    objList.Add(obj);
                }
            }

            return objList;

        }



    }
}
