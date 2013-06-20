using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Configuration;

namespace Scheduler
{
    public class ConnectToMySQL
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string user_id;
        private string user_hash;

        //Constructor
        public ConnectToMySQL(string _user_id, string _user_hash)
        {
            Initialize();
            user_id = _user_id;
            user_hash = _user_hash;
        }

        #region MySQL connection
        //Initialize values
        private void Initialize()
        {
            server = ConfigurationManager.AppSettings["MySQL_server"];
            database = ConfigurationManager.AppSettings["MySQL_database"]; ;
            uid = ConfigurationManager.AppSettings["MySQL_userid"]; ;
            password = ConfigurationManager.AppSettings["MySQL_password"]; ;
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        // Execute given query and return the datareader.
        public MySqlDataReader executeQuery(string _query)
        {
            MySqlDataReader dataReader = null;
            
            try
            {
                //Open connection
                if (OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(_query, connection);
                    //Create a data reader and Execute the command
                    dataReader = cmd.ExecuteReader();
                    //rowCount = Count("SELECT COUNT(" + _fldName + ") FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'hsc-db1' AND table_name = '" + _tblName + "';");

                    //close Data Reader
                    //dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    
                    return dataReader;
                }
                else
                {
                    return dataReader;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return dataReader;
            }
        }

        //Execute Non Query
        public void executeNonQuery(string _query)
        {
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(_query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
        #endregion
        #region MySQL INSERT
        //Insert statement
        public void Insert(List<string> _fldNames, string _tblName, List<string> _fldVals, string _user_id = "2")
        {
            string query = "";

            if (_fldNames.Count != 0)
            {
                query = "INSERT INTO " + _tblName + " ( " + string.Join(" ,", _fldNames.ToArray()) + " ) VALUES ( ";
                for (int i = 0; i < _fldVals.Count; i++)
                {
                    query += "'" + _fldVals[i].ToString() + "', ";
                }
                string _q = query.Remove(query.Length - 2, 2);
                query = _q;
                query += ") ;";
            }
            else
            {
                query = "INSERT INTO " + _tblName + " VALUES ( ";
                for (int i = 0; i < _fldVals.Count; i++)
                {
                    query += "'" + _fldVals[i].ToString() + "', ";
                }
                string _q = query.Remove(query.Length - 2, 2);
                query = _q;
                query += ") ;"; 
            }

            //createTrigger(_fldNames, _tblName, "INSERT", _user_id: _user_id);

            //open connection
            if (this.OpenConnection() == true)
            {
                
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                // Update user table for the activity
                string query2 = "UPDATE user_sessions SET user_sessions.user_id = '" + user_id + "', user_sessions.activity_count = user_sessions.activity_count + 1 WHERE user_sessions.session_id = '" + user_hash + "';";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                cmd2.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
        #endregion
        #region MySQL UPDATE
        //Update statement
        public void Update(string _query)
        {
            string query = _query;

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                // Update user table for the activity
                string query2 = "UPDATE user_sessions SET user_sessions.user_id = '" + user_id + "', user_sessions.activity_count = user_sessions.activity_count + 1 WHERE user_sessions.session_id = '" + user_hash + "';";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                cmd2.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
        #endregion
        #region MySQL DELETE
        //Delete statement
        public void Delete(string _query)
        {
            string query = _query;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();

                // Update user table for the activity
                string query2 = "UPDATE user_sessions SET user_sessions.user_id = '" + user_id + "', user_sessions.activity_count = user_sessions.activity_count + 1 WHERE user_sessions.session_id = '" + user_hash + "';";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                cmd2.ExecuteNonQuery();

                this.CloseConnection();
            }
        }
        #endregion
        #region MySQL SELECT
        //Select statement with data adapter
        public DataSet SelectMyDA(string _query)
        {

            string query = _query;
            List<string> list = new List<string>();
            DataSet _ds = new DataSet();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                dataAdapter.SelectCommand = cmd;

                dataAdapter.Fill(_ds);

                // Update user table for the activity
                string query2 = "UPDATE user_sessions SET user_sessions.user_id = '" + user_id + "', user_sessions.activity_count = user_sessions.activity_count + 1 WHERE user_sessions.session_id = '" + user_hash + "';";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                cmd2.ExecuteNonQuery();

                //close Connection
                this.CloseConnection();

                //return dataset to be displayed
                return _ds;
            }
            else
            {
                return _ds;
            }
        }

        //Select statement
        public List<string> Select(string _query)
        {
            
            string query = _query;
            List<string> list = new List<string>();

            try
            {
                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            list.Add(dataReader.GetString(i));
                        }
                    }
                    //close Data Reader
                    dataReader.Close();

                    // Update user table for the activity
                    string query2 = "UPDATE user_sessions SET user_sessions.user_id = '" + user_id + "', user_sessions.activity_count = user_sessions.activity_count + 1 WHERE user_sessions.session_id = '" + user_hash + "';";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    return list;
                }
                else
                {
                    return list;
                }
            }
            catch (MySqlException ex)
            {
                list.Add(ex.Message);
                return list;
            }
        }

        //Select statement - single value return
        public string SelectSingle(string _fldName, string _tblName, string _whrField, string _whrValue)
        {

            string query = "SELECT " + _fldName + " FROM " + _tblName + " WHERE "+ _whrField +" = '" + _whrValue + "';";
            List<string> list = new List<string>();
            list.Add("");

            try
            {
                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        list.Clear();
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            list.Add(dataReader.GetString(i));
                        }
                    }
                    //close Data Reader
                    dataReader.Close();

                    // Update user table for the activity
                    string query2 = "UPDATE user_sessions SET user_sessions.user_id = '" + user_id + "', user_sessions.activity_count = user_sessions.activity_count + 1 WHERE user_sessions.session_id = '" + user_hash + "';";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    return list[0].ToString();
                }
                else
                {
                    return list[0].ToString();
                }
            }
            catch (MySqlException ex)
            {
                list.Add(ex.Message);
                return list[0].ToString();
            }
        }
        #endregion
        #region MySQL COUNT
        //Count statement
        public int Count(string _query)
        {
            string query = _query;
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                // Update user table for the activity
                string query2 = "UPDATE user_sessions SET user_sessions.user_id = '" + user_id + "', user_sessions.activity_count = user_sessions.activity_count + 1 WHERE user_sessions.session_id = '" + user_hash + "';";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                cmd2.ExecuteNonQuery();

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
        #endregion
        #region MySQL TRIGGER
        // Drop and Create Trigger
        public void createTrigger(List<string> _fldNames, string _tblName, string _trgrType, string _trgrEvent = " AFTER ", string _user_id = "2")
        {
            string query = " DELIMITER $$ \n" ;
            query += " DROP TRIGGER IF EXISTS " + _tblName + " $$ \n";
            query += " CREATE TRIGGER " + _tblName + "_t \n" + _trgrEvent + " " + _trgrType + " ON " + _tblName + "\n FOR EACH ROW BEGIN \n";

            if (_trgrType == "UPDATE" || _trgrType == "DELETE")
            {
                for (int i = 0; i < _fldNames.Count; i++)
                {
                    query += " IF OLD." + _fldNames[i].ToString() + " <> NEW." + _fldNames[i].ToString() + " THEN \n";
                    query += " INSERT INTO user_activity (user_id, table_name, field_name, row_id, old_value, new_value, operation) ";
                    query += " VALUES (NEW." + _user_id + ", \"" + _tblName + "\", \"" + _fldNames[i].ToString() + "\", NEW.id, OLD." + _fldNames[i].ToString() + ", NEW." + _fldNames[i].ToString() + ", \"" + _trgrType + "\"); \n";
                    query += " END IF; \n";
                }
            }
            else
            {
                for (int i = 0; i < _fldNames.Count; i++)
                {
                    query += " INSERT INTO user_activity (user_id, table_name, field_name, row_id, old_value, new_value, operation) ";
                    query += " VALUES (" + _user_id + ", \"" + _tblName + "\", \"" + _fldNames[i].ToString() + "\", NEW.id, NEW." + _fldNames[i].ToString() + ", NEW." + _fldNames[i].ToString() + ", \"" + _trgrType + "\"); \n";

                }
            }
            query += " END $$ \n";
            query += " DELIMITER ; \n";

            executeNonQuery(query);
        }
        #endregion
        #region MySQL BACKUP AND RESTORE
        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
        #endregion
    }
}
