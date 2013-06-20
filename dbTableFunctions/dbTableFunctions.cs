using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Scheduler
{
    public class dbTableFunctions
    {
        private string user_id;
        private string user_hash;

        public dbTableFunctions(string _user_id, string _user_hash)
        {
            // Class Constructor.
            user_id = _user_id;
            user_hash = _user_hash;
        }

        //Get Column Names from specified table
        public List<string> ColNames(string _tblName)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id, user_hash);

            string query = "SELECT COLUMN_NAME FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = '" + _tblName + "';";
            List<string> fldNames = dbc.Select(query);

            return fldNames;
        }

        public List<List<string>> ColValues(string _tblName)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id, user_hash);
            
            List<List<string>> gd = new List<List<string>>();
            string query = "SELECT * FROM " + _tblName + ";";
            string countq = "SELECT COUNT(*) FROM " + _tblName + ";";
            int count = dbc.Count(countq);
            List<string> IDs = dbc.Select("SELECT id FROM " + _tblName + ";");

            for (int i = 0; i < count; i++)
            {
                List<string> gd1 = dbc.Select("SELECT * FROM "+_tblName+" WHERE id = " + IDs[i].ToString() + ";");
                gd.Add(gd1);
            }
            return gd;
        }
    }
}
