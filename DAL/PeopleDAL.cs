using Malshinon.DB;
using Malshinon.models;
using MySql.Data.MySqlClient;
using Malshinon.Utils;

namespace Malshinon.DAL
{

    public class PeopleDAL
    {
        private MySQL _db;

        public PeopleDAL()
        {
            _db = new MySQL();
        }

        public People AddNewPeople(People peopl)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "INSERT INTO people (FirstName, LastName, SecetCode, Type, NumReports, NumMentions) " +
                                   "VALUES (@FirstName, @LastName, @SecetCode, @Type, @NumReports, @NumMentions);";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", peopl.FirstName);
                        command.Parameters.AddWithValue("@LastName", peopl.LastName);
                        command.Parameters.AddWithValue("@SecetCode", peopl.SecetCode);
                        command.Parameters.AddWithValue("@Type", peopl.Type);
                        command.Parameters.AddWithValue("@NumReports", peopl.NumReports);
                        command.Parameters.AddWithValue("@NumMentions", peopl.NumMentions);

                        command.ExecuteNonQuery();
                    }

                    return peopl;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding new person: " + ex.Message);
                return null;
            }
        }
    }
}