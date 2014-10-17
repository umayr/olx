using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using olx.Properties;

namespace olx.Helpers
{
    class Database
    {
        private readonly SqlCommand _command;
        private readonly SqlConnection _connection;
        public Database()
        {
            _command = new SqlCommand();
            _connection = new SqlConnection { ConnectionString = GetConnectionString() };
            _command.Connection = _connection;
        }

        public bool Execute(string query)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                _command.CommandText = query;
                _command.CommandType = CommandType.Text;
                bool result = _command.ExecuteNonQuery() > 0;
                _connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ExecuteProcedure(string procedureName, List<SqlParameter> Params)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = procedureName;
                _command.Parameters.Clear();
                _command.Parameters.AddRange(Params.ToArray());
                int rowaffected = _command.ExecuteNonQuery();
                _connection.Close();

                return rowaffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string ExecuteProcedureScalar(string procedureName, List<SqlParameter> Params)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                _command.Parameters.Clear();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = procedureName;
                _command.Parameters.AddRange(Params.ToArray());
                var rowaffected = _command.ExecuteScalar();
                _connection.Close();

                return (string)rowaffected;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private static string GetConnectionString()
        {
            byte[] data = Convert.FromBase64String(Settings.Default.ConnectionString);
            return Encoding.UTF8.GetString(data);
        }
    }
}
