using Malshinon.DB;
using Malshinon.models;
using MySql.Data.MySqlClient;
using Malshinon.Utils;
using System.Dynamic;
using Mysqlx.Crud;

namespace Malshinon.DAL
{

    public class PeopleDAL
    {
        private MySQL _db;

        public PeopleDAL()
        {
            _db = new MySQL();
        }


        public People? AddNewPeople(People peopl)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "INSERT INTO people (first_name, last_name, secret_code, type, num_reports, num_mentions) " +
                                   "VALUES (@first_name, @last_name, @secret_code, @type, @num_reports, @num_mentions);";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@first_name", peopl.FirstName);
                        command.Parameters.AddWithValue("@last_name", peopl.LastName);
                        command.Parameters.AddWithValue("@secret_code", peopl.SecetCode);
                        command.Parameters.AddWithValue("@type", peopl.Type);
                        command.Parameters.AddWithValue("@num_reports", peopl.NumReports);
                        command.Parameters.AddWithValue("@num_mentions", peopl.NumMentions);

                        command.ExecuteNonQuery();
                        long insertedId = command.LastInsertedId;
                        if (insertedId > 0)
                        {
                            return GetPersonById((int)insertedId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding new person: " + ex.Message);
                return null;
            }
            return null;
        }


        public People? GetPersonById(int personId)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "SELECT * FROM people WHERE id = @id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", personId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return PeopleMapper.MapFromReader(reader);
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


        public People? GetPersonBySecretCode(string secretCode)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "SELECT * FROM people WHERE secret_code = @secret_code;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@secret_code", secretCode);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return PeopleMapper.MapFromReader(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving person by secret code: " + ex.Message);
            }

            return null;
        }


        public People? IncrementReportCount(int personId)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "UPDATE people SET num_reports = num_reports + 1 WHERE id = @id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", personId);
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


        public People? IncrementMentionCount(int personId)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "UPDATE people SET num_mentions = num_mentions + 1 WHERE id = @id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", personId);
                        command.ExecuteNonQuery();
                    }

                    return GetPersonById(personId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error incrementing mention count: " + ex.Message);
                return null;
            }
        }

        public People? UpdateType(int personId, string newType)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "UPDATE people SET type = @type WHERE id = @id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@type", newType);
                        command.Parameters.AddWithValue("@id", personId);
                        command.ExecuteNonQuery();
                    }

                    return GetPersonById(personId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating person type: " + ex.Message);
                return null;
            }
        }

        public People? UpdateDangerStatus(int personId, bool dangerStatus)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "UPDATE people SET DangerStatus = @danger_status WHERE id = @id;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@danger_status", dangerStatus);
                        command.Parameters.AddWithValue("@id", personId);
                        command.ExecuteNonQuery();
                    }

                    return GetPersonById(personId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating danger status: " + ex.Message);
                return null;
            }
        }

        public List<People> GetPeopleByType(string type)
        {
            List<People> peopleList = new List<People>();
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "SELECT * FROM people WHERE type = @type;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@type", type);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(PeopleMapper.MapFromReader(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving people by type: " + ex.Message);
            }

            return peopleList;
        }
        
        public List<People> GetPeoplesByDangerStatus(bool dangerStatus)
        {
            List<People> peopleList = new List<People>();
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "SELECT * FROM people WHERE DangerStatus = @danger_status;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@danger_status", dangerStatus);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(PeopleMapper.MapFromReader(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving people by danger status: " + ex.Message);
            }

            return peopleList;
        }
    }
}