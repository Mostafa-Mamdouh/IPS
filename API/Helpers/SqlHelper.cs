using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace API.Helpers
{
    public class SqlHelper
    {
        public static SqlParameter CreateSqlParameter(string parameterName, SqlDbType sqlDbType, object value)
        {
            SqlParameter parameter = new SqlParameter(parameterName, sqlDbType)
            {
                IsNullable = true,
                Value = value ?? DBNull.Value,
                Direction = ParameterDirection.Input
            };
            return parameter;
        }
    }
}
