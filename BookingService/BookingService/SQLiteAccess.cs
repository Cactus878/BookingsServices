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

        public List<string> ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            List<string> list = new List<string>();
            //DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Add parameters to the command if any
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list;
        }
    }
}