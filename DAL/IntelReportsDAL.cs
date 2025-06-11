using Malshinon.DB;
using Malshinon.models;
using MySql.Data.MySqlClient;
namespace Malshinon.DAL
{
    public class IntelReportsDAL
    {
        private MySQL _db;
        public IntelReportsDAL()
        {
            _db = new MySQL();
        }

        public IntelReport? AddNewIntelReport(IntelReport report)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "INSERT INTO intel_reports (reporter_id, target_id, text, timestamp) " +
                                   "VALUES (@reporter_id, @target_id, @text, @timestamp);";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@reporter_id", report.ReporterId);
                        command.Parameters.AddWithValue("@target_id", report.TargetId);
                        command.Parameters.AddWithValue("@text", report.Text);
                        command.Parameters.AddWithValue("@timestamp", report.Timestamp);

                        command.ExecuteNonQuery();
                        long insertedId = command.LastInsertedId;
                        if (insertedId > 0)
                        {
                            return GetIntelReportById((int)insertedId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding new intel report: " + ex.Message);
                return null;
            }
            return null;
        }


        public IntelReport? GetIntelReportById(int reportId)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "SELECT * FROM intel_reports WHERE id = @id;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", reportId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new IntelReport
                                {
                                    Id = reader.GetInt32("id"),
                                    ReporterId = reader.GetInt32("reporter_id"),
                                    TargetId = reader.GetInt32("target_id"),
                                    Text = reader.GetString("text"),
                                    Timestamp = reader.GetDateTime("timestamp")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving intel report: " + ex.Message);
            }
            return null;
        }

        public double? GetAverageReportLengthByReporterId(int reporterId)
        {
            try
            {
                using (var connection = _db.OpenConnection())
                {
                    string query = "SELECT AVG(CHAR_LENGTH(text)) AS avg_length FROM intel_reports WHERE reporter_id = @reporter_id;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@reporter_id", reporterId);
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            Console.WriteLine($"Average report length for reporter {reporterId}: {result}");
                            return Convert.ToDouble(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating average report length: " + ex.Message);
            }
            return null;
        }
    }
}