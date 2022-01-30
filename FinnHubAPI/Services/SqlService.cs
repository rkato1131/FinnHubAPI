using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FinnHubAPI.Services
{
    public class SqlService : IDisposable
    {
        public string ConnectionString { get; set; }
        
        public SqlService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlDataReader ExecDataReaderSP(string SPName, List<DBParameter> parameters, int timeout)
        {
            DBParameter param = null;
            SqlParameter sqlParam = null;
            SqlDataReader reader = null;
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (SqlCommand command = new SqlCommand(SPName, connection))
            {
                command.CommandTimeout = timeout;
                command.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < parameters.Count; i++)
                {
                    sqlParam = new SqlParameter();
                    param = parameters[i];

                    sqlParam.ParameterName = param.Name;
                    sqlParam.Size = param.Size;
                    sqlParam.Direction = param.Direction;
                    sqlParam.SqlDbType = param.Type;
                    sqlParam.Value = param.Value;

                    command.Parameters.Add(sqlParam);
                }

                try
                {
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {
                    throw;
                }
            }

            return reader;
        }

        public string GetSQLStringValue(string fieldName, SqlDataReader reader)
        {
            return reader[fieldName].GetType() == typeof(DBNull) ? string.Empty : (string)reader[fieldName];
        }

        public void Dispose() { }
    }

    
    public class DBCommand
    {
        public string ProcedureName { get; set; }
        public List<DBParameter> DBParameters { get; set; }
    }

    public class DBParameter
    {
        public DBParameter(string name, object value, int size, SqlDbType type, ParameterDirection direction)
        {
            Name = name;
            Value = value;
            Size = size;
            Type = type;
            Direction = direction;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public int Size { get; set; }
        public SqlDbType Type { get; set; }
        public ParameterDirection Direction { get; set; }
    }
}