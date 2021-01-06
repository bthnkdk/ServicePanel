using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Web.UI.Helper
{
    public static class DbHelper
    {
        static string connectionString;

        [ThreadStatic]
        static SqlConnection connection;

        static DbHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;
        }

        public static bool OpenConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
            }
            if (connection.State == ConnectionState.Closed ||
                connection.State == ConnectionState.Broken)
            {
                connection.Open();
                return true;
            }
            return false;
        }

        static void CloseConnection(bool close)
        {
            if (close)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }

        static void SetParameterToCommand(SqlCommand command, string sql, params object[] parameterValues)
        {
            string pattern = @"@\w+";
            var parameterNamesFromQuery = Regex.Matches(sql, pattern);
            if (parameterNamesFromQuery.Count != parameterValues.Length)
                throw new Exception("Parametre eksik veya fazla!");

            for (int i = 0; i < parameterNamesFromQuery.Count; i++)
            {
                command.Parameters.AddWithValue(parameterNamesFromQuery[i].Value, parameterValues[i] ?? DBNull.Value);
            }
        }

        public static object ExecuteScalarSP(string name, Dictionary<string, object> parameters)
        {
            bool con = OpenConnection();
            using (SqlCommand cmd = new SqlCommand(name, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }
                try
                {
                    return cmd.ExecuteScalar();
                }
                finally
                {
                    CloseConnection(con);
                }
            }
        }

        public static object ExecuteScalar(string sql, params object[] parameterValues)
        {
            bool con = OpenConnection();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                SetParameterToCommand(cmd, sql, parameterValues);

                try
                {
                    return cmd.ExecuteScalar();
                }
                finally
                {
                    CloseConnection(con);
                }
            }
        }

        public static int ExecuteNonQuery(string sql, params object[] parameterValues)
        {
            bool con = OpenConnection();
            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                SetParameterToCommand(cmd, sql, parameterValues);

                try
                {
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    CloseConnection(con);
                }
            }
        }

        public static IDataReader ExecuteReader(string sql, params object[] parameterValues)
        {
            bool con = OpenConnection();
            SqlCommand cmd = new SqlCommand(sql, connection);
            SetParameterToCommand(cmd, sql, parameterValues);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static void FillTable(DataTable dataTable, string sql, params object[] parameterValues)
        {
            bool con = OpenConnection();
            using (SqlDataAdapter dap = new SqlDataAdapter(sql, connection))
            {
                dap.SelectCommand.CommandTimeout = 100;
                SetParameterToCommand(dap.SelectCommand, sql, parameterValues);

                try
                {
                    dap.Fill(dataTable);
                }
                finally
                {
                    CloseConnection(con);
                }
            }
        }
    }
}