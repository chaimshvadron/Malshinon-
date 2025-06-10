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


        public People GetPersonById(int personId)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "SELECT * FROM people WHERE Id = @Id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", personId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new People
                                {
                                    Id = reader.GetInt32("Id"),
                                    FirstName = reader.GetString("FirstName"),
                                    LastName = reader.GetString("LastName"),
                                    SecetCode = reader.GetString("SecetCode"),
                                    Type = reader.GetString("Type"),
                                    NumReports = reader.GetInt32("NumReports"),
                                    NumMentions = reader.GetInt32("NumMentions")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving person: " + ex.Message);
            }

            return null;
        }
        
        public People IncrementReportCount(int personId)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "UPDATE people SET num_reports = num_reports + 1 WHERE Id = @Id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", personId);
                        command.ExecuteNonQuery();
                    }

                    return GetPersonById(personId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error incrementing report count: " + ex.Message);
                return null;
            }
        }
    }
}