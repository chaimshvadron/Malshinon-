using Malshinon.models;
using MySql.Data.MySqlClient;

namespace Malshinon.Utils
{
    public static class PeopleMapper
    {
        public static People MapFromReader(MySqlDataReader reader)
        {
            return new People
            {
                Id = reader.GetInt32("id"),
                FirstName = reader.GetString("first_name"),
                LastName = reader.GetString("last_name"),
                SecetCode = reader.GetString("secret_code"),
                Type = reader.GetString("type"),
                NumReports = reader.GetInt32("num_reports"),
                NumMentions = reader.GetInt32("num_mentions")
            };
        }
    }
}
