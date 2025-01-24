using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;


namespace BookingService
{
    public class SQLiteAccess
    {
        private string connectionString;

        // Constructor to initialize the connection string
        public SQLiteAccess(string databaseFile)
        {
            connectionString = $"Data Source={databaseFile};Version=3;";
        }

        // Open a SQLite connection
        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // Method to execute a non-query (INSERT, UPDATE, DELETE)
        public void ExecuteNonQuery(string query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to retrieve data (SELECT) and return as a DataTable
        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            return dataTable;
        }
    }
}
