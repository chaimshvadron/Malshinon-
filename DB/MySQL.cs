using MySql.Data.MySqlClient;

namespace Malshinon.DB
{
    public class MySQL
    {
        string ConnectionString = "server=localhost;port=3306;user=root;password=;database=malshinon;SslMode=None";
        public MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
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