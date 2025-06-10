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
    }
}