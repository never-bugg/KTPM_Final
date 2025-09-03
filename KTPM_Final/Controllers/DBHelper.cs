using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Model
{
    public class DBHelper
    {
        private SqlConnection _connection;

        public DBHelper()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            _connection = new SqlConnection(connectionString);
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }

        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }
    }
}
