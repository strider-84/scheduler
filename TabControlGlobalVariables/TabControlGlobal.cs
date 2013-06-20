using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class TabControlGlobal
    {
        public int loginPass = 0;
        public string loginError = "";
        public string loginUsername = "";
        public string fName = "";
        public string lName = "";
        public int user_group = 5;
        public string user_dept = "";
        public bool user_active = false;
        public int user_id = 0;
        public string user_hash = "";
        public string[] deptID_str_arr = null;
        private string user_id2;
        private string user_hash2;
        private ConnectToMySQL dbc;

        public struct activityValues
        {
            public string row_id;
            public string table_name;
            public string field;
            public string old_value;
            public string new_value;

            public activityValues(string _row_id, string _table_name, string _field, string _old_value, string _new_value)
            {
                row_id = _row_id;
                table_name = _table_name;
                field = _field;
                old_value = _old_value;
                new_value = _new_value;
            }
        }

        // Constructor
        public TabControlGlobal(string _user_id, string _user_hash)
        {
            // class constructor.
            user_id2 = _user_id;
            user_hash2 = _user_hash;
            dbc = new ConnectToMySQL(user_id2, user_hash2);

        }

        // Create Hash String using given String
        private static string MD5Hash(string _text)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5password = md5.ComputeHash(Encoding.ASCII.GetBytes(_text));
            StringBuilder result = new StringBuilder();
            foreach (byte b in md5password)
                result.Append(b.ToString("x2"));
            return result.ToString();
        }

        public void StoreLogin(string _username, string _password)
        {
            loginPass = dbc.Count("SELECT Count(*) FROM users WHERE username = \"" + _username + "\" AND BINARY password = \"" + _password + "\";");
            if (loginPass != 0)
            {
                List<string> loginInfo = dbc.Select("SELECT * FROM users WHERE username = \"" + _username + "\" AND BINARY password = \"" + _password + "\";");
                loginUsername = loginInfo[3];
                fName = loginInfo[1];
                lName = loginInfo[2];
                user_group = Int32.Parse(loginInfo[6]);
                if (loginInfo[5] == "Y")
                    user_active = true;
                user_id = Int32.Parse(loginInfo[0]);
                deptID_str_arr = dbc.Select("SELECT Dept_ID FROM v_user_depts WHERE User_ID = " + user_id + ";")[0].Split(',');

                if (!user_active)
                {
                    loginError = "Your Username is not activated. Please contact Administrator";
                    loginPass = 0;
                }
            }
            else
                loginError = "Invalid Username/Password!";
        }

        public void RecordUserSession()
        {
            var dateT = DateTime.Now;
            string query = "INSERT INTO user_sessions (user_id, session_id) VALUES (" + user_id + ", \""+MD5Hash(dateT.ToString("yyyyMMddHHmmssffff"))+"\");";
            user_hash = MD5Hash(dateT.ToString("yyyyMMddHHmmssffff"));
            
            List<string> temp2 = new List<string>();
            List<string> temp = new List<string>();
            temp.Add("user_id"); temp.Add("session_id");
            temp2.Add(user_id.ToString()); temp2.Add(user_hash);
            dbc.Insert(temp, "user_sessions", temp2, user_id.ToString());
        }

        public void RecordUserActivity(activityValues actValues)
        {
            List<string> oldVal = dbc.Select("SELECT " + actValues.field + " FROM " + actValues.table_name + " WHERE id = " + actValues.row_id + ";"); 
        }
    }
}
