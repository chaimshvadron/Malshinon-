using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace Malshinon.DB
{
    public class MySQL
    {
        private string _ConnectionString = "server=localhost;port=3306;user=root;password=;database=malshinon;SslMode=None";
        public MySqlConnection OpenConnection()
        {
            var stackTrace = new StackTrace();
            var callingMethod = stackTrace.GetFrame(1)?.GetMethod();
            Console.WriteLine($"DB connection opened by: {callingMethod?.DeclaringType?.FullName}.{callingMethod?.Name}");

            MySqlConnection connection = new MySqlConnection(_ConnectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Database connection established successfully.");
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error connecting to the database: " + ex.Message);
                return null;
            }
        }
    }
}