using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scheduler
{
    public class FormFunctions
    {
        private string user_id;
        private string user_hash;

        public FormFunctions(string _user_id, string _user_hash)
        {
            // Class constructor
            user_id = _user_id;
            user_hash = _user_hash;
        }

        public DataSet getDataset(string _tblName)
        {
            DataSet ds = new DataSet("Scheduler");
            dbTableFunctions dbFunc = new dbTableFunctions(user_id, user_hash);

            DataTable table1 = new DataTable(_tblName);
            
            List<string> colNames = dbFunc.ColNames(_tblName);
            List<List<string>> colVals = dbFunc.ColValues(_tblName);

            table1.Clear();

            table1.Columns.Clear();
            for (int i = 0; i < colNames.Count; i++) // populate column names
            {
                table1.Columns.Add(colNames[i].ToString());
            }

            table1.Rows.Clear();
            //DataRow dRow = new DataRow();
            for (int i = 0; i < colVals.Count; i++) // populate rows values
            { 
                //dRow.ItemArray = colVals[i].ToArray();
                table1.Rows.Add(colVals[i].ToArray());
            }

            ds.Tables.Add(table1);

            return ds;
        }

        public List<List<string>> tabPage15_GetData_lv()
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id, user_hash);
            dbTableFunctions dbFunc = new dbTableFunctions(user_id, user_hash);

            List<List<string>> gd = new List<List<string>>();
            gd = dbFunc.ColValues("po_details");

            return gd;
        }

        //-> Populate listview using given table as is
        public void popListViewT(ListView _lv, string _tblName)
        {
            dbTableFunctions dbFunc = new dbTableFunctions(user_id, user_hash);

            List<string> colNames = dbFunc.ColNames(_tblName);
            List<List<string>> colVals = dbFunc.ColValues(_tblName);

            _lv.Columns.Clear();
            for (int i = 0; i < colNames.Count; i++) // populate column names
            {
                _lv.Columns.Add(colNames[i].ToString());
            }

            _lv.Items.Clear();
            for (int i = 0; i < colVals.Count; i++) // populate row values.
            {
                ListViewItem lvi = new ListViewItem(colVals[i].ToArray());
                _lv.Items.Add(lvi);
            }
        }

        //-> Populate listview using given dataset as is
        public void popListViewDS(ListView _lv, DataSet _ds, bool assignColNames = true, bool assignTag = false, List<string> assignTagBy = null)
        {

            List<string> colNames = getColNamesDS(_ds);
            List<List<string>> colVals = getColValsDS(_ds);

            if (assignColNames)
            {
                _lv.Columns.Clear();
                for (int i = 0; i < colNames.Count; i++) // populate column names
                {
                    _lv.Columns.Add(colNames[i].ToString());
                }
            }

            _lv.Items.Clear();
            for (int i = 0; i < colVals.Count; i++) // populate row values.
            {
                ListViewItem lvi = new ListViewItem(colVals[i].ToArray());
                _lv.Items.Add(lvi);

                if (assignTag)
                {
                    _lv.Items[i].Tag = assignTagBy[i].ToString();
                }
            }
        }

        private List<string> getColNamesDS(DataSet _ds)
        {
            List<string> colNames = new List<string>();

            for (int i = 0; i < _ds.Tables[0].Columns.Count; i++)
            {
                colNames.Add(_ds.Tables[0].Columns[i].ToString());
            }
            return colNames;
        }

        private List<List<string>> getColValsDS(DataSet _ds)
        {
            List<List<string>> colValues = new List<List<string>>();
            //List<string> temp = new List<string>();

            for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
            {
                //temp.Clear();
                List<string> temp = new List<string>();
                for (int j = 0; j < _ds.Tables[0].Rows[i].ItemArray.Length; j++)
                {
                    temp.Add(_ds.Tables[0].Rows[i].ItemArray[j].ToString());
                }
                colValues.Add(temp);
            }
            return colValues;
        }

        // to check session expiry
        public bool checkSession(string _user_hash, double _timeDiff = 60.00)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id, user_hash);
            
            int session_id_count = dbc.Count("SELECT COUNT(*) FROM user_sessions WHERE session_id = '" + _user_hash + "';");
            string dbDate = dbc.SelectSingle("datetime", "user_sessions", "session_id", _user_hash);
            
            DateTime currTime = new DateTime();
            DateTime logTime = new DateTime();
            double elapsedTime = 0;

            currTime = DateTime.Now;
            logTime = DateTime.Parse(dbDate);
            
            elapsedTime = currTime.Subtract(logTime).TotalMinutes;

            if (session_id_count != 0 && elapsedTime > _timeDiff)
                return false;
            else
                return true;
        }

        public string[] getComboItems(string _group_id_Column, string _group_id, string _itemTextColumn, string _table_name, string _order_by = "member_id", string _whrClause = "")
        {
            DataSet ds = new DataSet();
            List<string> comboItems = new List<string>();
            ConnectToMySQL dbc = new ConnectToMySQL(user_id, user_hash);
            string query = "";

            query += "SELECT " + _itemTextColumn + " FROM " + _table_name + " ";

            if (_group_id_Column != "")
                query += " WHERE " + _group_id_Column + " = '" + _group_id + "' ";
            if (_whrClause != "")
                query += _whrClause;
            query += " ORDER BY " + _order_by + "; ";

            ds = dbc.SelectMyDA(query);

            for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
                comboItems.Add(ds.Tables[0].Rows[i][0].ToString());
            
            return comboItems.ToArray();
        }

        public void createXML_job(string _doc_name, List<string> _labels, DataSet _texts)
        {
            string xmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xmlStr += "<?xml-stylesheet type=\"text/xsl\" href=\"PersonalizationSheet.xsl\"?>";
            xmlStr += "<Document>";
            xmlStr += "<DocumentName>";
            xmlStr += _doc_name;
            xmlStr += "</DocumentName>";

            for (int i = 0; i < _labels.Count; i++)
            {
                xmlStr += "<Detail>";
                xmlStr += "<Label>" + _labels[i].ToString() + "</Label>";
                xmlStr += "<Text>" + _texts.Tables[0].Rows[0][i].ToString() + "</Text>";
                xmlStr += "</Detail>";
            }
            xmlStr += "</Document>";

            System.IO.File.WriteAllText(@"C:\Users\Chinmay\Documents\Visual Studio 2012\Projects\HarvardCardSystems-Scheduler\scheduler_files2print\PersonalizationSheet.xml", xmlStr);
        }

        public void createXML_gang(string _doc_name, List<string> _labels, DataSet _texts)
        {
            string xmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xmlStr += "<?xml-stylesheet type=\"text/xsl\" href=\"PersonalizationSheet.xsl\"?>";
            xmlStr += "<Document>";
            xmlStr += "<DocumentName>";
            xmlStr += _doc_name;
            xmlStr += "</DocumentName>";

            for (int i = 0; i < _labels.Count; i++)
            {
                xmlStr += "<Detail>";
                xmlStr += "<Label>" + _labels[i].ToString() + "</Label>";
                xmlStr += "<Text>" + _texts.Tables[0].Rows[0][i].ToString() + "</Text>";
                xmlStr += "</Detail>";
            }
            xmlStr += "</Document>";

            System.IO.File.WriteAllText(@"C:\Users\Chinmay\Documents\Visual Studio 2012\Projects\HarvardCardSystems-Scheduler\scheduler_files2print\PressInformationSheet.xml", xmlStr);
        }

        public void createXML_gang_mid(string _doc_name, List<string> _labels, DataSet _texts)
        {
            string xmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xmlStr += "<?xml-stylesheet type=\"text/xsl\" href=\"PersonalizationSheet.xsl\"?>";
            xmlStr += "<Document>";
            xmlStr += "<DocumentName>";
            xmlStr += _doc_name;
            xmlStr += "</DocumentName>";

            for (int i = 0; i < _labels.Count; i++)
            {
                xmlStr += "<Detail>";
                xmlStr += "<Label>" + _labels[i].ToString() + "</Label>";
                xmlStr += "<Text>" + _texts.Tables[0].Rows[0][i].ToString() + "</Text>";
                xmlStr += "</Detail>";
            }
            xmlStr += "</Document>";

            System.IO.File.WriteAllText(@"C:\Users\Chinmay\Documents\Visual Studio 2012\Projects\HarvardCardSystems-Scheduler\scheduler_files2print\MiDProductionSheet.xml", xmlStr);
        }

        public Double getJobRuntime(string _mcName, string _issueQty)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id, user_hash);

            Double capPerHr = (Double.Parse(dbc.SelectSingle("capacity", "machines", "machine_name", _mcName))/8);
            Double isq = Double.Parse(_issueQty);
            Double timeReqd = (isq / capPerHr);

            return timeReqd;
        }
    }
}
