using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Malshinon.models;

namespace Malshinon.DAL
{
    public class AlertDAL
    {
        private readonly string _connectionString;

        public AlertDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertAlert(Alert alert)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(@"INSERT INTO Alerts (target_id, window_start, window_end, created_at, reason) VALUES (@target_id, @window_start, @window_end, @created_at, @reason);", conn);
                cmd.Parameters.AddWithValue("@target_id", alert.TargetId);
                cmd.Parameters.AddWithValue("@window_start", alert.WindowStart);
                cmd.Parameters.AddWithValue("@window_end", alert.WindowEnd);
                cmd.Parameters.AddWithValue("@created_at", alert.CreatedAt);
                cmd.Parameters.AddWithValue("@reason", alert.Reason);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Alert> GetAllAlerts()
        {
            var alerts = new List<Alert>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, target_id, window_start, window_end, created_at, reason FROM Alerts ORDER BY created_at DESC", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        alerts.Add(new Alert
                        {
                            Id = reader.GetInt32("id"),
                            TargetId = reader.GetInt32("target_id"),
                            WindowStart = reader.GetDateTime("window_start"),
                            WindowEnd = reader.GetDateTime("window_end"),
                            CreatedAt = reader.GetDateTime("created_at"),
                            Reason = reader.GetString("reason")
                        });
                    }
                }
            }
            return alerts;
        }
    }
}
