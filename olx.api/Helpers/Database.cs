using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace olx.api.Helpers
{
    class Database : IDisposable
    {
        private readonly SqlCommand _command;
        private readonly SqlConnection _connection;

        public SqlCommand SqlCommand
        {
            get { return _command; }
        }
        public SqlConnection SqlConnection
        {
            get { return _connection; }
        }
        public Database()
        {
            _command = new SqlCommand();
            _connection = new SqlConnection { ConnectionString = GetConnectionString() };
            _command.Connection = _connection;
        }

        public SqlDataReader View(string viewName)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                _command.CommandText = string.Format("SELECT * FROM {0}", viewName);
                _command.CommandType = CommandType.Text;
                SqlDataReader reader = _command.ExecuteReader();
                //_connection.Close();
                return reader;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        public SqlDataReader Pagination(string viewName, int iterator, int top = 100)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                _command.CommandText = string.Format("SELECT TOP({0}) * FROM {1} WHERE id > {2} order by id", top, viewName, iterator);
                _command.CommandType = CommandType.Text;
                SqlDataReader reader = _command.ExecuteReader();
                //_connection.Close(); 
                return reader;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        public SqlDataReader ViewWithCondition(string viewName, string whereClause)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                _command.CommandText = string.Format("SELECT * FROM {0} WHERE {1}", viewName, whereClause);
                _command.CommandType = CommandType.Text;
                SqlDataReader reader = _command.ExecuteReader();
                return reader;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
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
        public object ExecuteScalar(string query)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                _command.CommandText = query;
                _command.CommandType = CommandType.Text;
                object result = _command.ExecuteScalar();
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

        public void Dispose()
        {
            SqlConnection.Close();
            SqlConnection.Dispose();
            SqlCommand.Dispose();
        }

        private static string GetConnectionString()
        {
            byte[] data = Convert.FromBase64String(Properties.Settings.Default.ConnectionString);
            return Encoding.UTF8.GetString(data);
        }
    }
}
