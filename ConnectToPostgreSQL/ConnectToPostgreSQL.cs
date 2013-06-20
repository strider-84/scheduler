using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows.Forms;
using System.Data;
using System.Configuration;

namespace Scheduler
{
    public class ConnectToPostgreSQL
    {
        private NpgsqlConnection conn;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        //Constructor
        public ConnectToPostgreSQL()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = ConfigurationManager.AppSettings["PSQL_server"];
            port = ConfigurationManager.AppSettings["PSQL_port"];
            database = ConfigurationManager.AppSettings["PSQL_database"];
            uid = ConfigurationManager.AppSettings["PSQL_userid"];
            password = ConfigurationManager.AppSettings["PSQL_password"];
            string connectionString;
            connectionString = "Server="+server+";Port="+port+";User Id="+uid+";Password="+password+";Database="+database+";";

            conn = new NpgsqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Code);
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Code);
                return false;
            }
        }

        public DataSet Select(List<string> _fldNames, string _tblName, string _whrClause = "")
        {
            string _query = "SELECT " + string.Join(" ,", _fldNames.ToArray()) + " FROM " + _tblName + " " + _whrClause + " ;";
            DataSet ds = new DataSet();

            //Open connection
            if (this.OpenConnection() == true)
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(_query, conn);

                ds.Reset();
                da.Fill(ds);

                //close Connection
                this.CloseConnection();

                return ds;
            }
            else
                return ds;
        }
    }
}
