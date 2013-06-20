using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;

namespace Scheduler
{
    public partial class Form1 : Form
    {
        private List<string> fldNames = new List<string>();
        private ListViewHitTestInfo hitInfo; // To get location of Listview double-clicked item
        private TextBox editBox = new TextBox(); // To overlay textbox in selected subitem
        private int columnCount = 0; // To store column count of listviews/tablelayoutpanel

        public Form1()
        {
            InitializeComponent();
            
            // To enable 'in-line' editing for SA Edit Search listview -->
            editBox.Parent = listView1_SA_edit_search;
            editBox.Hide();
            editBox.LostFocus += new EventHandler(editBox_LostFocus);
            listView1_SA_edit_search.MouseDoubleClick += new MouseEventHandler(listView1_SA_edit_search_MouseDoubleClick);
            // <--

            for (int i = 1; i < tabControl1.TabCount; i++)
            {
                ((Control)this.tabControl1.TabPages[i]).Enabled = false;
            }
        }
        #region Initial Form Functions
        public void EnableTabs(string[] _deptID)
        {
            foreach (string s in _deptID)
            {
                for (int i = 1; i < tabControl1.TabCount; i++)
                {
                    if (((Control)this.tabControl1.TabPages[i]).Name == "tabPage" + s.Replace(" ", ""))
                    {
                        ((Control)this.tabControl1.TabPages[i]).Enabled = true;
                        LoadTabValues(((Control)this.tabControl1.TabPages[i]).Name);
                        break;
                    }
                }
            }
        }

        public void LoadTabValues(string _tabPageName)
        {
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> gdData = new List<string>();
           
            /*
            if (_tabPageName == "tabPage15")
            {
                gdData = fFunc.tabPage15_GetData();
                job_number_txtBox1.Text = gdData[0];
                gang_number_txtBox1.Text = gdData[1];
                po_number_txtBox1.Text = gdData[2];
            }
            */
        }
        #endregion
        #region Login Tab
        private void loginEnable()
        {
            login_group.Visible = true;
            logout_btn.Visible = false;
            password.Text = "";

            for (int i = 1; i < tabControl1.TabCount; i++)
            {
                ((Control)this.tabControl1.TabPages[i]).Enabled = false;
                //tabControl1.TabPages[i].Refresh();
            }
        }

        private void loginDisable()
        {
            login_group.Visible = false;
            logout_btn.Visible = true;
        }

        private void connect_btn_Click(object sender, EventArgs e)
        {
            TabControlGlobal TCG = new TabControlGlobal(user_id_lbl.Text, user_hash_lbl.Text);
            TCG.StoreLogin(username.Text, password.Text);

            //label3.Text = TCG.loginPass.ToString();
            label3.Text = " " + TCG.loginError; // Display login error

            if (TCG.loginPass != 0)
            {
                TCG.RecordUserSession(); // ENABLE it.
                EnableTabs(TCG.deptID_str_arr);
                user_id_lbl.Text = TCG.user_id.ToString();
                user_hash_lbl.Text = TCG.user_hash.ToString();
                loginDisable();
                //tabControl1.TabPages.Remove(tabPage1); // to hide particular TabPage   
            }
        }
        private void logout_btn_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            string query = "DELETE FROM user_sessions WHERE user_id = '" + user_id_lbl.Text.ToString() + "' ;";
            dbc.Delete(query);

            loginEnable();
        }
        #endregion
        #region Sales Tab
        #region Sales Tab - Insert TabPage
        private void PO_Entry_IBtn1_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> fldVals = new List<string>();

            // checking input : cust_po_num, cust_name, cust_req_date -->
            if (textBox_Cust_PO_Num.Text != "")
            {
                if (dbc.SelectSingle("id", "sales_vars", "cust_po_number", textBox_Cust_PO_Num.Text) != "")
                {
                    MessageBox.Show("Duplicate Customer PO Number. Please recheck the PO number and enter again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                    MessageBox.Show("PO Number verified successfully !", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please enter PO number.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (textBox_Cust_Name.Text == "")
            {
                MessageBox.Show("Please enter customer name.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedDate = dateTimePicker_Cust_Req_Date.Value.Year.ToString() + "-" + dateTimePicker_Cust_Req_Date.Value.Month.ToString() + "-" + dateTimePicker_Cust_Req_Date.Value.Day.ToString();
            string todaysDate = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString();
            if (selectedDate == todaysDate)
            {
                MessageBox.Show("Please select future date.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // <--

            // adding field names & field values of po_entry table -->
            fldNames.Clear();
            fldNames.Add("order_qty");
            fldNames.Add("card_name");
            fldNames.Add("card_denom");
            fldNames.Add("cvv");
            fldNames.Add("card_size");
            fldNames.Add("card_thickness");
            fldNames.Add("card_material");
            fldNames.Add("num_colors");
            fldNames.Add("lamination");
            fldNames.Add("magnetic");
            fldNames.Add("pin");
            fldNames.Add("barcode");
            fldNames.Add("sol");
            fldNames.Add("hot_stamp");
            fldNames.Add("signature_panel");
            fldNames.Add("silk_screen");
            fldNames.Add("hole_punch");
            fldNames.Add("single_pack");
            fldNames.Add("bundle_pack");
            fldNames.Add("inner_box");
            fldNames.Add("outer_box"); //21 == i 20
            //<--

            /*
            fldNames.Add("user_id");
            fldVals.Add(user_id_lbl.Text);
            
            fldNames.Add("cust_po_number");
            fldVals.Add(textBox_Cust_PO_Num.Text);

            fldNames.Add("cust_name");
            fldVals.Add(textBox_Cust_Name.Text);

            if (textBox_Cust_Notes.Text != "")
            {
                fldNames.Add("notes");
                fldVals.Add(textBox_Cust_Notes.Text);
            }

            fldNames.Add("req_date");
            fldVals.Add(dateTimePicker_Cust_Req_Date.Value.Year.ToString() + "-" + dateTimePicker_Cust_Req_Date.Value.Month.ToString() + "-" + dateTimePicker_Cust_Req_Date.Value.Day.ToString());
            */
            //<--

            textBox_Cust_PO_Num_copy.Text = textBox_Cust_PO_Num.Text;
            //dbc.Insert(fldNames, "po_entry", fldVals, user_id_lbl.Text); // TODO: uncomment
            PoD_IBtn3.Enabled = false;

            tableLayoutPanel1_SA_insert_addDetails.Controls.Clear();
            tableLayoutPanel1_SA_insert_addDetails.ColumnCount = 0;

            Label t_lbl1 = new Label(); t_lbl1.Text = "PO Detail Fields";
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl1, 0, 0);

            for (int i = 0; i < 21; i++)
            {
                Label t_lable2 = new Label();
                t_lable2.Text = fldNames[i];
                tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lable2, 0, i + 1);
            }

            Label t_lbl3 = new Label();
            Label t_lbl4 = new Label();
            t_lbl3.Text = "Gang Number:";
            t_lbl4.Text = "Job Number:";
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl3, 0, 22);
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl4, 0, 23);

            PoD_IBtn2_Click(this.PO_Entry_IBtn1, new EventArgs()); // To add empty column with default values.

            label_SA_insertPoDSuccess.Visible = false;
        }

        private void PoD_IBtn1_Click(object sender, EventArgs e) // Add PO Details to database 
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> fldVals = new List<string>();

            button_SA_edit_PoD_Click(tableLayoutPanel1_SA_insert_addDetails, new EventArgs());
        }

        private void PoD_IBtn2_Click(object sender, EventArgs e) // Add more columns to table page layout 1 (PO Details table form)
        {
            // adding additional text boxes to table layout panel 1 -->
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            
            //-1-> common code
            int colCount = this.tableLayoutPanel1_SA_insert_addDetails.ColumnCount;
            if (colCount == 0)
                colCount++;
            colCount++;

            this.tableLayoutPanel1_SA_insert_addDetails.ColumnCount = colCount;
            this.tableLayoutPanel1_SA_insert_addDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());

            Label t_lbl = new Label();
            t_lbl.Text = "PO Detail Values " + (colCount - 1).ToString();
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl, colCount - 1, 0);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            for (int i = 0; i < fldNames.Count; i++)
            {
                Panel t_panel = new Panel();
                ComboBox t_combo = new ComboBox();
                TextBox t_textBox = new TextBox();

                t_textBox.Location = new Point(3, 30);

                t_combo.Items.Add("Other");
                t_combo.SelectedItem = "Other";

                t_combo.Location = new System.Drawing.Point(4, 6);
                t_combo.Size = new System.Drawing.Size(121, 13);
                //t_combo.Tag = new TableLayoutPanelCellPosition(i, j);
                t_combo.DropDownStyle = ComboBoxStyle.DropDownList;
                t_combo.FlatStyle = FlatStyle.Popup;

                t_panel.AutoSize = true;
                t_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                t_panel.BorderStyle = BorderStyle.FixedSingle;

                t_panel.SuspendLayout();
                if (i >= 4 & i < 21)
                {
                    t_combo.Items.AddRange(fFunc.getComboItems("group_id", (i - 2).ToString(), "member_description", "po_specs"));
                    t_panel.Controls.Add(t_combo);
                    t_combo.SelectedIndex = 1;
                    t_combo.Tag = "";

                    if ((string)t_combo.SelectedItem == "Other")
                    {
                        t_textBox.Text = "";
                        t_panel.Controls.Add(t_textBox);
                    }
                }
                else if (i < 4)
                {
                    t_textBox.Text = "";
                    if (i == 2)
                        t_textBox.Text = "$";
                    t_textBox.Location = new System.Drawing.Point(4, 6);
                    t_panel.Controls.Add(t_textBox);
                }
                t_panel.ResumeLayout();


                if (i <= 20)
                {
                    tableLayoutPanel1_SA_insert_addDetails.SuspendLayout();
                    tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_panel, colCount - 1, i + 1);
                    //t_panel.Tag = ds.Tables[0].Rows[i][j].ToString(); // TODO: Remove
                    tableLayoutPanel1_SA_insert_addDetails.ResumeLayout();
                }

                t_combo.SelectedIndexChanged += t_combo_SelectedIndexChanged;
            }
            tableLayoutPanel1_SA_insert_addDetails.SuspendLayout();
            tableLayoutPanel1_SA_insert_addDetails.GetControlFromPosition((colCount - 1), 20).Tag = textBox_Cust_PO_Num_copy.Text; // Adding tag to store po num.
            tableLayoutPanel1_SA_insert_addDetails.GetControlFromPosition((colCount - 1), 21).Tag = "0"; // Adding tag to indicate new row.
            
            tableLayoutPanel1_SA_insert_addDetails.ResumeLayout();
            //<--

            // adding gang number and job number, fetched with po_details.id -->
            string str1 = "Initial";
            string str2 = "-no val-";

            Label t_lbl6 = new Label();
            Label t_lbl7 = new Label();
            t_lbl6.Text = str1;
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl6, colCount - 1, 22);
            t_lbl7.Text = str2;
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl7, colCount - 1, 23);
            // <--

            // To display total order quantity -->
            int totalOrderQty = 0;
            for (int j = 1; j < colCount; j++)
            {
                totalOrderQty += Int32.Parse((tableLayoutPanel1_SA_insert_addDetails.GetControlFromPosition(j, 1).GetChildAtPoint(new Point(4, 6)).Text == "" ? "0" : tableLayoutPanel1_SA_insert_addDetails.GetControlFromPosition(j, 1).GetChildAtPoint(new Point(4, 6)).Text));
            }
            Label_SA_insert_OrderQty2.Text = totalOrderQty.ToString();
            // <--
            //<-1- end of common code
        }

        private void PoD_IBtn3_Click(object sender, EventArgs e) // Get po details data based upon customer po number entered into textbox
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            // --> Get po details data based on customer po number
            string po_entry_id = dbc.SelectSingle("id", "sales_vars", "cust_po_number", textBox_Cust_PO_Num_copy.Text);
            if (po_entry_id == "")
            {
                MessageBox.Show("Couldn't find Customer PO Number. Please recheck the PO number and search again.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PoD_IBtn2.Enabled = false;
                PoD_IBtn1.Enabled = false;
                return;
            }
            PoD_IBtn2.Enabled = true;
            PoD_IBtn1.Enabled = true;
            label_SA_insertPoDSuccess.Visible = false;
            PO_Entry_IBtn1.Enabled = false;

            tableLayoutPanel1_SA_insert_addDetails.Controls.Clear();
            // adding field names of po_details table -->
            fldNames.Clear();
            fldNames.Add("order_qty");
            fldNames.Add("card_name");
            fldNames.Add("card_denom");
            fldNames.Add("cvv");
            fldNames.Add("card_size");
            fldNames.Add("card_thickness");
            fldNames.Add("card_material");
            fldNames.Add("num_colors");
            fldNames.Add("lamination");
            fldNames.Add("magnetic");
            fldNames.Add("pin");
            fldNames.Add("barcode");
            fldNames.Add("sol");
            fldNames.Add("hot_stamp");
            fldNames.Add("signature_panel");
            fldNames.Add("silk_screen");
            fldNames.Add("hole_punch");
            fldNames.Add("single_pack");
            fldNames.Add("bundle_pack");
            fldNames.Add("inner_box");
            fldNames.Add("outer_box"); // 21 == i20
            fldNames.Add("id");
            //<--

            DataSet ds = new DataSet();
            string query = "SELECT " + string.Join(", ", fldNames.ToArray());
            query += " FROM sales_vars WHERE cust_po_number = '" + textBox_Cust_PO_Num_copy.Text + "'; ";
            ds = dbc.SelectMyDA(query);
            // <--

            // Adding Column Headers for table layout panel 1 -->
            Label t_lbl2 = new Label(); 
            t_lbl2.Text = "PO Detail Fields";
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl2, 0, 0);
            //<--

            // adding field names to table layout panel 1 -->
            for (int i = 0; i < 21; i++)
            {
                Label t_lbl = new Label();
                t_lbl.Text = fldNames[i];

                tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl, 0, i + 1);
            }
            //<--

            // adding gang number and job number to table layout panel 1 -->
            Label t_lbl4 = new Label();
            Label t_lbl5 = new Label();

            t_lbl4.Text = "Gang Number:";
            t_lbl5.Text = "Job Number:";

            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl4, 0, 22);
            tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl5, 0, 23);
            // <--

            // adding data columns with field values from database for selected PO number
            int colCount = 1;
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            tableLayoutPanel1_SA_insert_addDetails.ColumnCount = colCount;
            this.tableLayoutPanel1_SA_insert_addDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1_SA_insert_addDetails.GetControlFromPosition(0, 0).Tag = ds.Tables[0].Rows.Count; // add col count to 0,0 position tag.

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                colCount++;
                this.tableLayoutPanel1_SA_insert_addDetails.ColumnCount = colCount;
                this.tableLayoutPanel1_SA_insert_addDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());

                Label t_lbl3 = new Label(); 
                t_lbl3.Text = "PO Detail Values " + (i + 1).ToString();
                tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl3, i + 1, 0);

                for (int j = 0; j < 21; j++)
                {
                    Panel t_panel = new Panel();
                    ComboBox t_combo = new ComboBox();
                    TextBox t_textBox = new TextBox();

                    t_textBox.Location = new Point(3, 30);

                    t_combo.Items.Add("Other");
                    t_combo.SelectedItem = "Other";

                    t_combo.Location = new System.Drawing.Point(4, 6);
                    t_combo.Size = new System.Drawing.Size(121, 13);
                    //t_combo.Tag = new TableLayoutPanelCellPosition(i, j);
                    t_combo.DropDownStyle = ComboBoxStyle.DropDownList;
                    t_combo.FlatStyle = FlatStyle.Popup;

                    t_panel.AutoSize = true;
                    t_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                    t_panel.BorderStyle = BorderStyle.FixedSingle;

                    t_panel.SuspendLayout();
                    if (j >= 4 & j < 21)
                    {
                        t_combo.Items.AddRange(fFunc.getComboItems("group_id", (j - 2).ToString(), "member_description", "po_specs"));
                        t_panel.Controls.Add(t_combo);
                        t_combo.SelectedItem = ds.Tables[0].Rows[i][j].ToString();
                        t_combo.Tag = ds.Tables[0].Rows[i][j].ToString();

                        if ((string)t_combo.SelectedItem == "Other")
                        {
                            t_textBox.Text = ds.Tables[0].Rows[i][j].ToString();
                            t_panel.Controls.Add(t_textBox);
                        }
                    }
                    else
                    {
                        t_textBox.Text = ds.Tables[0].Rows[i][j].ToString();
                        t_textBox.Location = new System.Drawing.Point(4, 6);
                        t_panel.Controls.Add(t_textBox);
                    }
                    t_panel.ResumeLayout();


                    tableLayoutPanel1_SA_insert_addDetails.SuspendLayout();
                    tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_panel, i + 1, j + 1);
                    tableLayoutPanel1_SA_insert_addDetails.ResumeLayout();

                    t_panel.Enabled = false;
                }

                // adding gang number and job number, fetched with po_details.id -->
                string str1 = dbc.SelectSingle("gang_number", "graphics_vars_gang", "id", dbc.SelectSingle("graphics_vars_gang_id", "graphics_vars_job", "sales_vars_id", ds.Tables[0].Rows[i][21].ToString()));
                string str2 = dbc.SelectSingle("job_number", "graphics_vars_job", "sales_vars_id", ds.Tables[0].Rows[i][21].ToString());

                Label t_lbl6 = new Label();
                Label t_lbl7 = new Label();
                t_lbl6.Text = str1;
                tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl6, i+1, 22);
                t_lbl7.Text = str2;
                tableLayoutPanel1_SA_insert_addDetails.Controls.Add(t_lbl7, i+1, 23);
                // <--
            }

            // To display total order quantity -->
            int totalOrderQty = 0;
            for (int j = 1; j < colCount; j++)
            {
                if (tableLayoutPanel1_SA_insert_addDetails.GetControlFromPosition(j, 1).GetChildAtPoint(new Point(4, 6)).Text != "")
                    totalOrderQty += Int32.Parse(tableLayoutPanel1_SA_insert_addDetails.GetControlFromPosition(j, 1).GetChildAtPoint(new Point(4, 6)).Text);
            }
            Label_SA_insert_OrderQty2.Text = totalOrderQty.ToString();
            // <--
        }
        #endregion
        #region Sales Tab - View TabPage
        #endregion
        #region Sales Tab - Edit TabPage
        private void button_SA_edit_search_Click(object sender, EventArgs e)
        {
            string s1 = (textBox_SA_edit_CustPoNum.Text == "") ? "" : "%" + textBox_SA_edit_CustPoNum.Text + "%";
            string s2 = (textBox_SA_edit_CustName.Text == "") ? "" : "%" + textBox_SA_edit_CustName.Text + "%";
            string s3 = (textBox_SA_edit_Notes.Text == "") ? "" : "%" + textBox_SA_edit_Notes.Text + "%";

            string query = "SELECT cust_name, cust_po_number, DATE_FORMAT(req_date, '%Y-%m-%d'), notes ";
            string query2 = " FROM sales_vars ";
            query2 += " WHERE cust_po_number LIKE '" + s1 + "' OR ";
            query2 += " cust_name LIKE '" + s2 + "' OR ";
            query2 += " notes LIKE '" + s3 + "' ";
            string query3 = " GROUP BY cust_po_number ";

            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            ds = dbc.SelectMyDA(query + query2 + query3 + ";");
            ds2 = dbc.SelectMyDA("SELECT cust_po_number " + query2 + query3 + ";");

            List<string> IDs = new List<string>();
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                IDs.Add(ds2.Tables[0].Rows[i][0].ToString());
            }

            fFunc.popListViewDS(listView1_SA_edit_search, ds, assignColNames: false, assignTag: true, assignTagBy: IDs);
        }

        void listView1_SA_edit_search_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            hitInfo = listView1_SA_edit_search.HitTest(e.X, e.Y);

            if (hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem) != 1 & hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem) != 2)
            {
                editBox.Bounds = hitInfo.SubItem.Bounds;
                editBox.Text = hitInfo.SubItem.Text;
                editBox.Focus();
                editBox.Show();
            }
        }

        void editBox_LostFocus(object sender, EventArgs e)
        {
            if (hitInfo.SubItem.Text != editBox.Text)
            {
                ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

                string query = "UPDATE po_entry SET ";
                if (hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem) == 0)
                    query += " cust_name = '" + editBox.Text + "' ";
                if (hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem) == 3)
                    query += " notes = '" + editBox.Text + "' ";
                query += " WHERE id = '" + hitInfo.Item.Tag.ToString() + "';";

                dbc.Update(query); // TODO: uncomment
            }
            hitInfo.SubItem.Text = editBox.Text;
            editBox.Hide();
        }

        private void button_SA_edit_getData_Click(object sender, EventArgs e)
        {
            // --> Get po details data based on customer po number
            if (listView1_SA_edit_search.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select the PO entry first.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            button_SA_edit_PoD.Visible = true;
            button_SA_edit_PoD.Enabled = true;
            label_SA_updateSuccess.Visible = false;

            string po_entry_id = "";
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            tableLayoutPanel1_SA_edit_addDetails.Controls.Clear();
            // adding field names of po_details table -->
            fldNames.Clear();
            fldNames.Add("order_qty");
            fldNames.Add("card_name");
            fldNames.Add("card_denom");
            fldNames.Add("cvv");
            fldNames.Add("card_size");
            fldNames.Add("card_thickness");
            fldNames.Add("card_material");
            fldNames.Add("num_colors");
            fldNames.Add("lamination");
            fldNames.Add("magnetic");
            fldNames.Add("pin");
            fldNames.Add("barcode");
            fldNames.Add("sol");
            fldNames.Add("hot_stamp");
            fldNames.Add("signature_panel");
            fldNames.Add("silk_screen");
            fldNames.Add("hole_punch");
            fldNames.Add("single_pack");
            fldNames.Add("bundle_pack");
            fldNames.Add("inner_box");
            fldNames.Add("outer_box"); //21 == i20
            fldNames.Add("id");
            //<--

            po_entry_id = listView1_SA_edit_search.SelectedItems[0].Tag.ToString();

            DataSet ds = new DataSet();
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            string query = "SELECT " + string.Join(", ", fldNames.ToArray());
            query += " FROM sales_vars WHERE cust_po_number = '" + listView1_SA_edit_search.SelectedItems[0].SubItems[1].Text + "'; ";
            ds = dbc.SelectMyDA(query);
            // <--

            label11.Text = "Customer Name: "+ dbc.SelectSingle("cust_name", "sales_vars", "cust_po_number", po_entry_id);
            label12.Text = "Customer PO#:  "+ po_entry_id;
            label11.Visible = true;
            label12.Visible = true;

            // Adding Column Headers for table layout panel 1 -->
            Label t_lbl2 = new Label(); t_lbl2.Text = "PO Detail Fields";
            tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl2, 0, 0);
            //<--

            // adding field names to table layout panel 1 -->
            for (int i = 0; i < 21; i++)
            {
                Label t_lbl = new Label();
                t_lbl.Text = fldNames[i];

                tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl, 0, i + 1);
            }
            //<--

            // adding gang number and job number to table layout panel 1 -->
            Label t_lbl4 = new Label();
            Label t_lbl5 = new Label();
            Label t_lbl8 = new Label();
            Label t_lbl9 = new Label();

            t_lbl4.Text = "Gang Number:";
            t_lbl5.Text = "Job Number:";
            t_lbl8.Text = "Data Approval:";
            t_lbl9.Text = "Art Approval:";

            tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl4, 0, 22);
            tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl5, 0, 23);
            tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl8, 0, 24);
            tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl9, 0, 25);
            // <--

            // adding data columns with field values from database for selected PO number -->
            int colCount = 1;
            tableLayoutPanel1_SA_edit_addDetails.ColumnCount = colCount;
            this.tableLayoutPanel1_SA_edit_addDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                colCount++;
                this.tableLayoutPanel1_SA_edit_addDetails.ColumnCount = colCount;
                this.tableLayoutPanel1_SA_edit_addDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());

                Label t_lbl3 = new Label(); t_lbl3.Text = "PO Detail Values " + (i + 1).ToString();
                tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl3, i + 1, 0);

                for (int j = 0; j < 21; j++)
                {
                    Panel t_panel = new Panel(); 
                    ComboBox t_combo = new ComboBox();
                    TextBox t_textBox = new TextBox();

                    t_textBox.Location = new Point(3, 30);

                    t_combo.Items.Add("Other");
                    t_combo.SelectedItem = "Other";

                    t_combo.Location = new System.Drawing.Point(4, 6); 
                    t_combo.Size = new System.Drawing.Size(121, 13); 
                    //t_combo.Tag = new TableLayoutPanelCellPosition(i, j);
                    t_combo.DropDownStyle = ComboBoxStyle.DropDownList;
                    t_combo.FlatStyle = FlatStyle.Popup;

                    t_panel.AutoSize = true; 
                    t_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink; 
                    t_panel.BorderStyle = BorderStyle.FixedSingle; 

                    t_panel.SuspendLayout();
                    if (j >= 4 & j < 21)
                    {
                        t_combo.Items.AddRange(fFunc.getComboItems("group_id", (j-2).ToString(), "member_description", "po_specs"));
                        t_panel.Controls.Add(t_combo);
                        t_combo.SelectedItem = ds.Tables[0].Rows[i][j].ToString();
                        t_combo.Tag = ds.Tables[0].Rows[i][j].ToString();

                        if ((string)t_combo.SelectedItem == "Other")
                        {
                            t_textBox.Text = ds.Tables[0].Rows[i][j].ToString();
                            t_panel.Controls.Add(t_textBox);
                        }
                    }
                    else
                    {
                        t_textBox.Text = ds.Tables[0].Rows[i][j].ToString();
                        t_textBox.Location = new System.Drawing.Point(4, 6);
                        t_panel.Controls.Add(t_textBox);
                    }
                    t_panel.ResumeLayout();

                    tableLayoutPanel1_SA_edit_addDetails.SuspendLayout(); 
                    tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_panel, i + 1, j + 1);
                    tableLayoutPanel1_SA_edit_addDetails.ResumeLayout();

                    t_combo.SelectedIndexChanged += t_combo_SelectedIndexChanged;
                }
                
                tableLayoutPanel1_SA_edit_addDetails.GetControlFromPosition((colCount - 1), 21).Tag = ds.Tables[0].Rows[i][21]; // Adding tag to indicate row id

                // adding gang number and job number, fetched with po_details.id -->
                string str1 = dbc.SelectSingle("graphics_vars_gang_id", "graphics_vars_job", "sales_vars_id", ds.Tables[0].Rows[i][21].ToString());
                string str2 = dbc.SelectSingle("job_number", "graphics_vars_job", "sales_vars_id", ds.Tables[0].Rows[i][21].ToString());

                string str3 = dbc.SelectSingle("DataApproved", "sales_vars", "id", ds.Tables[0].Rows[i][21].ToString());
                string str4 = dbc.SelectSingle("ArtApproved", "sales_vars", "id", ds.Tables[0].Rows[i][21].ToString());

                Label t_lbl6 = new Label();
                Label t_lbl7 = new Label();
                CheckBox t_chkBox1 = new CheckBox();
                CheckBox t_chkBox2 = new CheckBox();

                t_lbl6.Text = str1;
                tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl6, i + 1, 22);
                t_lbl7.Text = str2;
                tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_lbl7, i + 1, 23);
                // <--

                // adding checkboxes for data and art appoval -->
                if (str1 == "" || str2 == "")
                {
                    t_chkBox1.Enabled = false;
                    t_chkBox2.Enabled = false;
                }

                if (str3 == "Y")
                    t_chkBox1.Checked = true;
                if (str4 == "Y")
                    t_chkBox2.Checked = true;
                tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_chkBox1, i + 1, 24);
                tableLayoutPanel1_SA_edit_addDetails.Controls.Add(t_chkBox2, i + 1, 25);
                // <--
            }
            // <--

            // To display total order quantity -->
            int totalOrderQty = 0;
            for (int j = 1; j < colCount; j++)
            {
                totalOrderQty += Int32.Parse((tableLayoutPanel1_SA_edit_addDetails.GetControlFromPosition(j, 1).GetChildAtPoint(new Point(4, 6)).Text == "" ? "0" : tableLayoutPanel1_SA_edit_addDetails.GetControlFromPosition(j, 1).GetChildAtPoint(new Point(4, 6)).Text));
            }
            Label_SA_edit_OrderQty2.Text = totalOrderQty.ToString();
            // <--
        }

        void t_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableLayoutPanelCellPosition pos = new TableLayoutPanelCellPosition();
            Control ctr = new Control();
            Control ctrParent = new Control();
            TableLayoutPanel ctrGrandParent = new TableLayoutPanel();
            TextBox t_txtBox2 = new TextBox();

            ctr = (Control)sender; // cast combobox control
            ctrParent = ctr.Parent; // get panel control
            ctrGrandParent = (TableLayoutPanel)ctrParent.Parent; // get tablelayoutpanel control
            pos = ctrGrandParent.GetPositionFromControl(ctrParent); // get panel position

            if (ctrParent.Controls.Count < 2 & ctr.Text == "Other")
            {
                t_txtBox2.Location = new Point(3, 30);
                t_txtBox2.Text = ctr.Tag.ToString();
                ctrParent.Controls.Add(t_txtBox2);
            }
            else if (ctrParent.Controls.Count == 2 & ctr.Text != "Other")
            {
                ctrGrandParent.GetControlFromPosition(pos.Column, pos.Row).Controls.RemoveAt(1);
            }
        }

        private void button_SA_edit_PoD_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> fldVals = new List<string>();
            List<string> addtFldVals = new List<string>();
            TableLayoutPanel TLP = new TableLayoutPanel();
            string[] str = new string[] { };
            DataSet ds = new DataSet();

            if (((Control)sender).Name == "button_SA_edit_PoD")
                TLP = tableLayoutPanel1_SA_edit_addDetails;
            else
                TLP = (TableLayoutPanel)sender;

            // setting up start column and adding remaining values like user_id, po number, cust name, req date, notes -->
            int startColCount = 1;
            if (TLP.GetControlFromPosition(0, 0).Tag != null)
            {
                startColCount = Int32.Parse(TLP.GetControlFromPosition(0, 0).Tag.ToString()) + 1; // get col count from 0,0 position tag.
                string query = "SELECT cust_po_number, notes, cust_name, DATE_FORMAT(req_date, '%Y-%m-%d')";
                query += " FROM sales_vars WHERE cust_po_number = '" + TLP.GetControlFromPosition(startColCount, 20).Tag.ToString() + "'; ";
                ds = dbc.SelectMyDA(query);
                addtFldVals.Add(user_id_lbl.Text);
                for (int i = 0; i < ds.Tables[0].Rows[0].ItemArray.Length; i++)
                    addtFldVals.Add(ds.Tables[0].Rows[0].ItemArray[i].ToString());
            }
            else if(TLP.Name != "tableLayoutPanel1_SA_edit_addDetails")
            {
                addtFldVals.Add(user_id_lbl.Text);
                addtFldVals.Add(textBox_Cust_PO_Num.Text);
                addtFldVals.Add(textBox_Cust_Notes.Text);
                addtFldVals.Add(textBox_Cust_Name.Text);
                addtFldVals.Add(dateTimePicker_Cust_Req_Date.Value.Year.ToString() + "-" + dateTimePicker_Cust_Req_Date.Value.Month.ToString() + "-" + dateTimePicker_Cust_Req_Date.Value.Day.ToString());
            }
            // <--

            // adding field values and inserting for po_details table -->
            List<string> fldNames2 = new List<string>();
            fldNames2.Add("user_id");
            fldNames2.Add("cust_po_number");
            fldNames2.Add("notes");
            fldNames2.Add("cust_name");
            fldNames2.Add("req_date");
            for (int i = startColCount; i < TLP.ColumnCount; i++)
            {
                fldVals.Clear();
                for (int j = 1; j <= 21; j++)
                {
                    if (TLP.GetControlFromPosition(i, j).GetChildAtPoint(new Point(4, 6)).Text == "Other")
                        fldVals.Add(TLP.GetControlFromPosition(i, j).GetChildAtPoint(new Point(3, 30)).Text);
                    else
                        fldVals.Add(TLP.GetControlFromPosition(i, j).GetChildAtPoint(new Point(4, 6)).Text);
                }

                /*
                string s = TLP.GetControlFromPosition(i, 21).Tag.ToString();
                if (((Control)sender).Name == "button_SA_edit_PoD")
                    str = tableLayoutPanel1_SA_edit_addDetails.GetControlFromPosition(i, 21).Tag.ToString().Remove(TLP.GetControlFromPosition(i, 21).Tag.ToString().Length - 1).Split('&');
                else
                    str = TLP.GetControlFromPosition(i, 21).Tag.ToString().Remove(TLP.GetControlFromPosition(i, 21).Tag.ToString().Length - 1).Split('&');
                */

                fldVals.AddRange(addtFldVals.ToArray());
                //if (fldNames.Count == 21)
                //    fldNames.RemoveAt(21);
                if (fldNames.Count == 22)
                    fldNames.RemoveAt(21);
                
                if (Int32.Parse(TLP.GetControlFromPosition(startColCount, 21).Tag.ToString()) == 0)
                {
                    fldNames.AddRange(fldNames2);
                    dbc.Insert(fldNames, "sales_vars", fldVals, user_id_lbl.Text); // TODO: uncomment
                    fldNames2.Clear();
                }
                else
                {
                    fldNames.AddRange(new string[] {"DataApproved", "ArtApproved" });

                    if (((CheckBox)TLP.GetControlFromPosition(i, 24)).CheckState == CheckState.Checked)
                        fldVals.Add("Y");
                    else
                        fldVals.Add("N");
                    
                    if (((CheckBox)TLP.GetControlFromPosition(i, 25)).CheckState == CheckState.Checked)
                        fldVals.Add("Y");
                    else
                        fldVals.Add("N");

                    string query = "UPDATE sales_vars SET ";
                    for (int j = 0; j < fldNames.Count; j++)
                    {
                        query += fldNames[j] + " = '" + fldVals[j] + "'";
                        if (j != fldNames.Count - 1)
                            query += ", ";
                    }
                    query += " WHERE id ='" + TLP.GetControlFromPosition(i, 21).Tag.ToString() + "';";
                    dbc.Update(query); // TODO: uncomment
                    fldNames.RemoveAt(22);
                }
            }
            // <--
            if (((Control)sender).Name == "button_SA_edit_PoD")
                label_SA_updateSuccess.Visible = true;
            else
                label_SA_insertPoDSuccess.Visible = true;
        }
        #endregion
        #endregion
        #region Graphics Tab
        #region Graphics Tab - Insert TabPage
        private void btn1_GD_insert_InsertGang_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            label_GD_insert_newGangSuccess.Visible = false;

            if (dbc.Count("SELECT COUNT(id) FROM graphics_vars_gang WHERE gang_number = '"+txtBox_gang_num.Text+"';") > 0)
            {
                MessageBox.Show("Please enter new gang number", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> g_fldNames = new List<string>();
            List<string> g_fldVals = new List<string>();

            g_fldNames.Add("gang_number");
            g_fldVals.Add(txtBox_gang_num.Text);

            dbc.Insert(g_fldNames, "graphics_vars_gang", g_fldVals, user_id_lbl.Text); //TODO: uncomment
            label_GD_insert_newGangSuccess.Visible = true;
        }

        private void tabControl_GD_Enter(object sender, EventArgs e)
        {
            // NOTHING HERE YET
        }

        private void tabPage_GD_insert_Enter(object sender, EventArgs e)
        {
            txtBox_gang_num.Text = "";

            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            string query = "SELECT cust_po_number, cust_name, SUM(order_qty), COUNT(id) ";
            string query2 = "SELECT cust_po_number ";
            string query3 = "FROM sales_vars ";
            query3 += "WHERE cust_po_number NOT IN (SELECT sales_vars_id FROM graphics_vars_gang) ";
            query3 += "GROUP BY cust_po_number;";

            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            List<string> IDs = new List<string>();

            ds = dbc.SelectMyDA(query + query3);
            ds2 = dbc.SelectMyDA(query2 + query3);
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                IDs.Add(ds2.Tables[0].Rows[i][0].ToString());
            }
            fFunc.popListViewDS(listView_GD_insert_newPOs, ds, assignColNames: false, assignTag: true, assignTagBy: IDs);
        }

        private void btn_GD_insert_loadPO_Click(object sender, EventArgs e)
        {
            // --> Get po details data based on customer po number
            if (listView_GD_insert_newPOs.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select the PO entry first.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //label_GD_insert_update_status.Visible = false;
            //tableLayoutPanel_GD_insert_job.Controls.Clear();
            string po_entry_id = "";
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> fldNames2 = new List<string>();

            // --> Job Tabpage(s)
            // adding field names of po_details table -->
            fldNames.Clear();
            fldNames.Add("cust_name"); //i 0
            fldNames.Add("req_date");
            fldNames.Add("card_denom");
            fldNames.Add("card_material");
            fldNames.Add("order_qty");
            fldNames.Add("lamination");
            fldNames.Add("card_name");
            fldNames.Add("id");
            fldNames.Add("id"); //i 8
            fldNames.Add("user_id");
            fldNames.Add("card_size"); //i 10
            fldNames.Add("timestamp");
            //<--

            po_entry_id = listView_GD_insert_newPOs.SelectedItems[0].Tag.ToString();

            DataSet ds = new DataSet();
            //DataSet ds2 = new DataSet();
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            
            string[] cbox_items = fFunc.getComboItems("", "", "DISTINCT(gang_number)", "graphics_vars_gang", _order_by: "id");

            string query = "SELECT " + string.Join(", ", fldNames.ToArray());
            query += " FROM sales_vars ";
            query += " WHERE cust_po_number = '" + po_entry_id + "'";
            string query2 = " GROUP BY cust_po_number; ";
            //ds2 = dbc.SelectMyDA(query+query2);
            ds = dbc.SelectMyDA(query);
            
            this.tabControl_GD_insert.TabPages.Clear();
            for (int j = 0; j < dbc.Count("SELECT count(id) from sales_vars where cust_po_number = '" + po_entry_id + "';"); j++)
            {
                TabPage tabPage_job = new TabPage("Job " + (j + 1).ToString());
                TableLayoutPanel tlp_GD_insert_job = new TableLayoutPanel();
                //tlp_GD_insert_job = tableLayoutPanel_GD_insert_job;
                //tlp_GD_insert_job.Controls.Clear();
                tlp_GD_insert_job.Name = "tlp_GD_insert_job" + (j + 1).ToString();

                tlp_GD_insert_job.ColumnCount = 3;
                tlp_GD_insert_job.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
                tlp_GD_insert_job.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
                tlp_GD_insert_job.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
                tlp_GD_insert_job.Dock = System.Windows.Forms.DockStyle.Fill;
                tlp_GD_insert_job.Location = new System.Drawing.Point(3, 3);
                tlp_GD_insert_job.RowCount = 16;
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
                tlp_GD_insert_job.Size = new System.Drawing.Size(809, 539);
                tlp_GD_insert_job.TabIndex = 0;

                tabPage_job.Controls.Add(tlp_GD_insert_job);
                //tabPage_job.Location = new System.Drawing.Point(4, 22);
                //tabPage_job.Name = "tabPage3";
                //tabPage_job.Padding = new System.Windows.Forms.Padding(3);
                //tabPage_job.Size = new System.Drawing.Size(815, 545);
                tabPage_job.TabIndex = j+1;
                //tabPage_job.Text = "Jobs";
                tabPage_job.UseVisualStyleBackColor = true;
                this.tabControl_GD_insert.TabPages.Add(tabPage_job);

                //--> adding label names to table
                List<string> lblNames = new List<string>();
                lblNames.AddRange(new string[] { "JobID", "CustomerName", "PODate", "CardName", "Retailer", "Denomination", "Order Qty", "CoreMaterial", "CustomerDueDate", "PO", "TypeOfCard", "ProjectedDueDate", "Sales", "Lamination", "ActualDueDate", "GangID", "PinCover", "Personalization Job", "CardRatio", "Other", "Minimum Print Qty:", "Mag Stripe", "Encode" });

                for (int i = 0; i < lblNames.Count; i++)
                {
                    Label t_gd_insert_label1 = new Label();
                    Label t_gd_insert_label2 = new Label();
                    Label t_gd_insert_label3 = new Label();
                    Button t_gd_insert_button1 = new Button();
                    
                    int temp1;
                    if (i < 21)
                    {
                        t_gd_insert_label1.Text = lblNames[i].ToString();
                        t_gd_insert_label2.Text = lblNames[i + 1].ToString();
                        t_gd_insert_label3.Text = lblNames[i + 2].ToString();
                    }
                    else
                    {
                        t_gd_insert_label1.Text = lblNames[i].ToString();
                        t_gd_insert_label2.Text = lblNames[i + 1].ToString();
                    }

                    temp1 = Int32.Parse(Math.Floor(double.Parse(i.ToString()) / 3).ToString());
                    i++; i++;
                    temp1 = temp1 * 2;
                    tlp_GD_insert_job.Controls.Add(t_gd_insert_label1, 0, temp1);
                    tlp_GD_insert_job.Controls.Add(t_gd_insert_label2, 1, temp1);
                    if (i < 23)
                        tlp_GD_insert_job.Controls.Add(t_gd_insert_label3, 2, temp1);
                    else
                    {
                        t_gd_insert_button1.Text = "Save Info";
                        t_gd_insert_button1.Name = ds.Tables[0].Rows[j][8].ToString();
                        t_gd_insert_button1.Tag = tlp_GD_insert_job;
                        tlp_GD_insert_job.Controls.Add(t_gd_insert_button1, 2, temp1);
                    }
                    
                    t_gd_insert_button1.Click += new EventHandler(button_GD_insert_InsertJob_Click);
                }

                for (int i = 0; i < lblNames.Count; i++)
                {
                    Label t_gd_insert_label = new Label(); t_gd_insert_label.Dock = DockStyle.Fill;
                    TextBox t_gd_insert_textbox = new TextBox();
                    CheckBox t_gd_insert_chkbox = new CheckBox();
                    Button t_gd_insert_button = new Button();
                    ComboBox t_gd_insert_combobox = new ComboBox();

                    switch (i)
                    {
                        case 0:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 0, 1);
                            break;
                        case 1:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[0].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 1, 1);
                            break;
                        case 2:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[11].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 2, 1);
                            break;
                        case 3:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[6].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 0, 3);
                            break;
                        case 4:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 1, 3);
                            break;
                        case 5:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[2].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 2, 3);
                            break;
                        case 6:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[4].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 0, 5);
                            break;
                        case 7:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[3].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 1, 5);
                            break;
                        case 8:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[1].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 2, 5);
                            break;
                        case 9:
                            t_gd_insert_label.Text = po_entry_id;
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 0, 7);
                            break;
                        case 10:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[10].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 1, 7);
                            break;
                        case 11:
                            t_gd_insert_textbox.Enabled = false;
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 2, 7);
                            break;
                        case 12:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[9].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 0, 9);
                            break;
                        case 13:
                            t_gd_insert_label.Text = ds.Tables[0].Rows[j].ItemArray[5].ToString();
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_label, 1, 9);
                            break;
                        case 14:
                            t_gd_insert_textbox.Enabled = false;
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 2, 9);
                            break;
                        case 15:
                            t_gd_insert_combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                            t_gd_insert_combobox.FlatStyle = FlatStyle.Popup;
                            t_gd_insert_combobox.Items.Clear();
                            t_gd_insert_combobox.Items.Add("Initial");
                            t_gd_insert_combobox.Items.AddRange(cbox_items);
                            t_gd_insert_combobox.SelectedItem = "Initial";
                            //t_gd_insert_label.Text = "0000";
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_combobox, 0, 11);
                            break;
                        case 16:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 1, 11);
                            break;
                        case 17:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_chkbox, 2, 11);
                            break;
                        case 18:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 0, 13);
                            break;
                        case 19:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 1, 13);
                            break;
                        case 20:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 2, 13);
                            break;
                        case 21:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_chkbox, 0, 15);
                            break;
                        case 22:
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_textbox, 1, 15);
                            t_gd_insert_button.Text = "Print Sheet";
                            t_gd_insert_button.Enabled = false;
                            tlp_GD_insert_job.Controls.Add(t_gd_insert_button, 2, 15);
                            break;
                    }
                }
            }
            // <--

            //--> Gang tabpage
            TabPage tabPage_gang = new TabPage("Gang");
            TableLayoutPanel tlp_GD_insert_gang = new TableLayoutPanel();
            
            // adding field names of po_details table -->
            fldNames.Clear();
            fldNames.Add("cust_name"); //i 0
            fldNames.Add("cust_po_number");
            fldNames.Add("SUM(order_qty)");
            fldNames.Add("card_material");
            fldNames.Add("num_colors");
            fldNames.Add("lamination");
            fldNames.Add("card_name");
            fldNames.Add("COUNT(id)");
            fldNames.Add("id"); //i 8
            fldNames.Add("user_id");
            fldNames.Add("id"); //i 10

            query = "SELECT " + string.Join(", ", fldNames.ToArray());
            query += " FROM sales_vars ";
            query += " WHERE cust_po_number = '" + po_entry_id + "'";
            query += " GROUP BY cust_po_number; ";
            ds.Clear();
            ds = dbc.SelectMyDA(query);

            label_GD_insert_1x1.Text = ds.Tables[0].Rows[0][0].ToString();
            label_GD_insert_0x3.Text = ds.Tables[0].Rows[0][1].ToString();
            label_GD_insert_1x3.Text = ds.Tables[0].Rows[0][2].ToString();
            label_GD_insert_0x5.Text = ds.Tables[0].Rows[0][3].ToString();
            label_GD_insert_0x9.Text = ds.Tables[0].Rows[0][4].ToString();
            label_GD_insert_0x13.Text = ds.Tables[0].Rows[0][5].ToString();

            comboBox_GD_insert_0x0.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_GD_insert_0x0.FlatStyle = FlatStyle.Popup;
            comboBox_GD_insert_0x0.Items.Clear();
            comboBox_GD_insert_0x0.Items.Add("New");
            comboBox_GD_insert_0x0.Items.AddRange(cbox_items);

            button_gd_insert_gangsave.Tag = ds.Tables[0].Rows[0][8].ToString();

            int up_size = 0;

            //TableLayoutPanel tlp_GD_insert_gang_job = new TableLayoutPanel(); //TODO: make generic table layout panel by copying properties
            //tlp_GD_insert_gang_job = tableLayoutPanel_GD_insert_JobD; //TODO: make generic table layout panel by copying properties
            
            DataSet ds2 = new DataSet();
            string query3 = "SELECT card_name, up_size, graphics_vars_job.id, job_number ";
            //query3 += ", gang_number.gang_number_id ";
            query3 += " FROM sales_vars ";
            query3 += " JOIN graphics_vars_job on sales_vars_id = sales_vars.id ";
            query3 += " WHERE cust_po_number = '" + po_entry_id + "' AND graphics_vars_gang_id = '0'";
            query3 += " ORDER BY job_number; ";
            ds2 = dbc.SelectMyDA(query3);

            Label t_label0x1 = new Label();
            Button t_button0x1 = new Button();
            Label t_label1 = new Label();
            Label t_label2 = new Label();
            Label t_label3 = new Label();
            Label t_label4 = new Label();

            tableLayoutPanel_GD_insert_JobD.Controls.Clear();
            t_label0x1.Text = "Job Information";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label0x1, 0, 0);

            t_button0x1.Text = "Update";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_button0x1, 1, 0);
            t_button0x1.Click += new EventHandler(t_button0x1Click);

            t_label1.Text = "Job Number";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label1, 0, 1);

            t_label2.Text = "Job Name";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label2, 1, 1);

            t_label3.Text = "Job Up Size";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label3, 2, 1);

            t_label4.Text = "Action";
            t_label4.Tag = ds2.Tables[0].Rows.Count;
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label4, 3, 1);

            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                TextBox t_gangjob_textBox = new TextBox();
                Label t_gangjob_label1 = new Label();
                Label t_gangjob_label2 = new Label();
                Button t_gangjob_button = new Button();
                
                t_gangjob_label1.AutoSize = true;
                t_gangjob_label1.Text = ds2.Tables[0].Rows[i][3].ToString();

                t_gangjob_label2.AutoSize = true;
                t_gangjob_label2.Text = ds2.Tables[0].Rows[i][0].ToString();

                t_gangjob_textBox.Text = ds2.Tables[0].Rows[i][1].ToString();
                t_gangjob_textBox.Tag = ds2.Tables[0].Rows[i][1].ToString();

                t_gangjob_button.Text = "Insert Up-size";
                t_gangjob_button.Tag = ds2.Tables[0].Rows[i][2].ToString();

                tableLayoutPanel_GD_insert_JobD.SuspendLayout();
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_label1, 0, i + 2);
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_label2, 1, i + 2);
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_textBox, 2, i + 2);
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_button, 3, i + 2);
                tableLayoutPanel_GD_insert_JobD.ResumeLayout();

                up_size += Int32.Parse(ds2.Tables[0].Rows[i][1].ToString());

                t_gangjob_textBox.TextChanged += t_textBoxInsert_TextChanged;
                t_gangjob_button.Click += new EventHandler(t_gangjob_button_Click);
            }
            label_GD_insert_2x3.Text = up_size.ToString();
            int temp_up_size = 1;
            if (up_size != 0)
                temp_up_size = up_size;
            label_GD_insert_2x5.Text = (Int32.Parse(label_GD_insert_1x3.Text) / temp_up_size).ToString();

            tlp_GD_insert_gang = tableLayoutPanel_GD_insert_PoD;
            tabPage_gang.Controls.Add(tlp_GD_insert_gang);
            tabPage_gang.TabIndex = 0;
            tabPage_gang.UseVisualStyleBackColor = true;
            this.tabControl_GD_insert.TabPages.Add(tabPage_gang);
            // <--

            //--> Mid tabpage
            TabPage tabPage_mid = new TabPage("Mid");
            TableLayoutPanel tlp_gd_insert_mid = new TableLayoutPanel();
            Label t_lbl3 = new Label();
            ComboBox t_combo4 = new ComboBox();
            Button t_btn1 = new Button();
            Button t_btn2 = new Button();

            tlp_gd_insert_mid.ColumnCount = 2;
            tlp_gd_insert_mid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlp_gd_insert_mid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlp_gd_insert_mid.Dock = System.Windows.Forms.DockStyle.Fill;
            tlp_gd_insert_mid.AutoScroll = true;

            t_lbl3.Text = "GangID";
            t_combo4.Items.AddRange(cbox_items);
            t_btn1.Text = "Save";
            t_btn1.Click += new EventHandler(t_btn1_Click);

            t_btn2.Text = "Print";
            t_btn2.Click += new EventHandler(t_btn2_Click);
            t_btn2.Enabled = false;

            tlp_gd_insert_mid.Controls.Add(t_lbl3, 0, 0);
            tlp_gd_insert_mid.Controls.Add(t_btn1, 1, 0);
            tlp_gd_insert_mid.Controls.Add(t_combo4, 0, 1);
            tlp_gd_insert_mid.Controls.Add(t_btn2, 0, 1);

            DataSet ds3 = new DataSet();
            string query4 = "SHOW COLUMNS FROM graphics_vars_gang;";
            ds3 = dbc.SelectMyDA(query4);

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 1; col++)
                {
                    Label t_lbl1 = new Label(); t_lbl1.Dock = DockStyle.Fill;
                    Label t_lbl2 = new Label(); t_lbl2.Dock = DockStyle.Fill;
                    CheckBox t_chkBox = new CheckBox(); t_chkBox.Dock = DockStyle.Fill;
                    ComboBox t_comboBox = new ComboBox(); t_comboBox.Dock = DockStyle.Fill; t_comboBox.Enabled = false;

                    t_lbl1.Text = ds3.Tables[0].Rows[(row * 2) + 17][0].ToString();
                    t_lbl2.Text = ds3.Tables[0].Rows[(row * 2) + 18][0].ToString();

                    t_chkBox.Tag = t_comboBox;
                    t_comboBox.Tag = t_lbl1.Text;

                    tlp_gd_insert_mid.Controls.Add(t_lbl1, col, ((row * 2) + 2));
                    tlp_gd_insert_mid.Controls.Add(t_lbl2, col + 1, ((row * 2) + 2));

                    tlp_gd_insert_mid.Controls.Add(t_chkBox, col, ((row * 2) + 3));
                    tlp_gd_insert_mid.Controls.Add(t_comboBox, col + 1, ((row * 2) + 3));

                    t_chkBox.CheckStateChanged += new EventHandler(t_chkBox_CheckStateChanged);
                }
            }
            tabPage_mid.Controls.Add(tlp_gd_insert_mid);
            tabPage_mid.TabIndex = this.tabControl_GD_insert.TabPages.Count;
            tabPage_mid.UseVisualStyleBackColor = true;
            this.tabControl_GD_insert.TabPages.Add(tabPage_mid);
            //<--
        }

        void t_button0x1Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            string graphics_vars_gang_id = "1";

            if (comboBox_GD_insert_0x0.SelectedItem == "New")
                graphics_vars_gang_id = textBox_GD_insert_0x1.Text;
            else if(comboBox_GD_insert_0x0.SelectedItem != null)
                graphics_vars_gang_id = comboBox_GD_insert_0x0.SelectedItem.ToString();

            DataSet ds2 = new DataSet();
            
            string query3 = "SELECT card_name, up_size, graphics_vars_job.id, job_number ";
            query3 += " FROM sales_vars ";
            query3 += " JOIN graphics_vars_job on sales_vars_id = sales_vars.id ";
            query3 += " WHERE cust_po_number = '" + label_GD_insert_0x3.Text + "' AND graphics_vars_gang_id = '" + graphics_vars_gang_id + "'";
            query3 += " ORDER BY job_number; ";
            
            ds2 = dbc.SelectMyDA(query3);

            Label t_label0x1 = new Label();
            Button t_button0x1 = new Button();
            Label t_label1 = new Label();
            Label t_label2 = new Label();
            Label t_label3 = new Label();
            Label t_label4 = new Label();

            tableLayoutPanel_GD_insert_JobD.Controls.Clear();
            tableLayoutPanel_GD_insert_JobD.Controls.Clear();
            t_label0x1.Text = "Job Information";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label0x1, 0, 0);

            t_button0x1.Text = "Update";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_button0x1, 1, 0);
            t_button0x1.Click += new EventHandler(t_button0x1Click);

            t_label1.Text = "Job Number";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label1, 0, 1);

            t_label2.Text = "Job Name";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label2, 1, 1);

            t_label3.Text = "Job Up Size";
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label3, 2, 1);

            t_label4.Text = "Action";
            t_label4.Tag = ds2.Tables[0].Rows.Count;
            tableLayoutPanel_GD_insert_JobD.Controls.Add(t_label4, 3, 1);

            int up_size = 0;

            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                TextBox t_gangjob_textBox = new TextBox();
                Label t_gangjob_label1 = new Label();
                Label t_gangjob_label2 = new Label();
                Button t_gangjob_button = new Button();

                t_gangjob_label1.AutoSize = true;
                t_gangjob_label1.Text = ds2.Tables[0].Rows[i][3].ToString();

                t_gangjob_label2.AutoSize = true;
                t_gangjob_label2.Text = ds2.Tables[0].Rows[i][0].ToString();

                t_gangjob_textBox.Text = ds2.Tables[0].Rows[i][1].ToString();
                t_gangjob_textBox.Tag = ds2.Tables[0].Rows[i][1].ToString();

                t_gangjob_button.Text = "Insert Up-size";
                t_gangjob_button.Tag = ds2.Tables[0].Rows[i][2].ToString();

                tableLayoutPanel_GD_insert_JobD.SuspendLayout();
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_label1, 0, i + 2);
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_label2, 1, i + 2);
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_textBox, 2, i + 2);
                tableLayoutPanel_GD_insert_JobD.Controls.Add(t_gangjob_button, 3, i + 2);
                tableLayoutPanel_GD_insert_JobD.ResumeLayout();

                up_size += Int32.Parse(ds2.Tables[0].Rows[i][1].ToString());

                t_gangjob_textBox.TextChanged += t_textBoxInsert_TextChanged;
                t_gangjob_button.Click += new EventHandler(t_gangjob_button_Click);
            }

            label_GD_insert_2x3.Text = up_size.ToString();
            int temp_up_size = 1;
            if (up_size != 0)
                temp_up_size = up_size;
            label_GD_insert_2x5.Text = (Int32.Parse(label_GD_insert_1x3.Text) / temp_up_size).ToString();
        }

        void t_btn2_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> fldLabels = new List<string>();
            DataSet ds = new DataSet();
            fldLabels.AddRange(new string[] { "Printing", "PrintingType", "MagneticTape", "MagneticTapeType", "ColLay", "ColLayType", "HydraulicPress", "HydraulicPressType", "SheetCut", "SheetCutType", "DieCut", "DieCutType", "Personalization", "PersonalizationType" });

            string query = " SELECT " + string.Join(", ", fldLabels);
            query += " FROM graphics_vars_gang ";
            query += " WHERE gang_number = " + ((Button)sender).Tag.ToString();

            ds = dbc.SelectMyDA(query);
            fFunc.createXML_gang_mid("Mid Production Instruction Sheet", fldLabels, ds);
        }

        void t_btn1_Click(object sender, EventArgs e)
        {
            TableLayoutPanel tlp_gd_insert_mid = (TableLayoutPanel)tabControl_GD_insert.TabPages[tabControl_GD_insert.TabPages.Count - 1].Controls[0];
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            string gang_number = tlp_gd_insert_mid.GetControlFromPosition(0, 1).Text;
            List<string> fldVals = new List<string>();
            fldNames.Clear();

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 1; col++)
                {
                    if (((CheckBox)tlp_gd_insert_mid.GetControlFromPosition(col, ((row * 2) + 3))).CheckState == CheckState.Checked)
                    {
                        fldNames.Add(tlp_gd_insert_mid.GetControlFromPosition(col, ((row * 2) + 2)).Text);
                        fldVals.Add("Y");

                        fldNames.Add(tlp_gd_insert_mid.GetControlFromPosition(col + 1, ((row * 2) + 2)).Text);
                        fldVals.Add(tlp_gd_insert_mid.GetControlFromPosition(col + 1, ((row * 2) + 3)).Text);
                    }
                }
            }

            string query = "UPDATE graphics_vars_gang SET ";
            for (int i = 0; i < fldVals.Count; i++)
                query += fldNames[i] + "=" + "\"" + fldVals[i] + "\" ,";
            query = query.Remove((query.Length - 1), 1);
            query += " WHERE gang_number = \"" + gang_number + "\"";

            dbc.Update(query);

            tlp_gd_insert_mid.GetControlFromPosition(1, 1).Enabled = true;
            tlp_gd_insert_mid.GetControlFromPosition(1, 1).Tag = gang_number;

        }

        void t_chkBox_CheckStateChanged(object sender, EventArgs e)
        {
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            string[] mc_combo = new string[] { };

            if (((CheckBox)sender).CheckState == CheckState.Checked)
            {
                ((ComboBox)((CheckBox)sender).Tag).Enabled = true;
                ((ComboBox)((CheckBox)sender).Tag).Items.Clear();

                switch (((ComboBox)((CheckBox)sender).Tag).Tag.ToString())
                {
                    case "Printing":
                        mc_combo = fFunc.getComboItems("deptID", "1", "machine_name", "machines", _order_by: "machine_name");
                        ((ComboBox)((CheckBox)sender).Tag).Items.AddRange(mc_combo);
                        break;
                    case "MagneticTape":
                        mc_combo = fFunc.getComboItems("deptID", "3", "machine_name", "machines", _order_by: "machine_name");
                        ((ComboBox)((CheckBox)sender).Tag).Items.AddRange(mc_combo);
                        break;
                    case "DieCut":
                        mc_combo = fFunc.getComboItems("deptID", "5", "machine_name", "machines", _order_by: "machine_name");
                        ((ComboBox)((CheckBox)sender).Tag).Items.AddRange(mc_combo);
                        break;
                }
            }
            else
            {
                ((ComboBox)((CheckBox)sender).Tag).Enabled = false;
                ((ComboBox)((CheckBox)sender).Tag).Items.Clear();
            }
        }

        void t_gangjob_button_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            string str_buttonTag = ((Button)sender).Tag.ToString();

            string gang_number = "Initial";
            if (comboBox_GD_insert_0x0.SelectedItem.ToString() == "New")
                gang_number = textBox_GD_insert_0x1.Text;
            else if(comboBox_GD_insert_0x0.SelectedItem != null)
                gang_number = comboBox_GD_insert_0x0.SelectedItem.ToString();

            string query = " UPDATE graphics_vars_job ";
            query += " SET graphics_vars_gang_id = " + gang_number + ", ";
            query += " up_size = " + label_GD_insert_2x3.Tag.ToString();
            query += " WHERE id = " + str_buttonTag;

            dbc.Update(query);

            // Updating total up size in graphics_vars_gang table -->
            query = " SELECT up_size, id, sales_vars_id ";
            query += " FROM graphics_vars_gang ";
            query += " WHERE gang_number = '" + dbc.SelectSingle("graphics_vars_gang_id", "graphics_vars_job", "id", str_buttonTag) + "'; ";

            DataSet ds = new DataSet();
            int up_size = 0;
            ds = dbc.SelectMyDA(query);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (label_GD_insert_0x3.Text != ds.Tables[0].Rows[i][2].ToString())
                    up_size += Int32.Parse(ds.Tables[0].Rows[i][0].ToString());
            }
            up_size += Int32.Parse(label_GD_insert_2x3.Tag.ToString());

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                query = " UPDATE graphics_vars_gang ";
                query += " SET up_size = " + up_size.ToString() + " ";
                query += " WHERE id = " + ds.Tables[0].Rows[i][1].ToString();

                dbc.Update(query);
            }
            //<--
        }

        private void button_GD_insert_InsertJob_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            TableLayoutPanel tlp_gd_insert_job = (TableLayoutPanel)((Control)sender).Tag;
            List<string> j_fldNames = new List<string>();
            List<string> j_fldVals = new List<string>();
            string s_id = tlp_gd_insert_job.GetControlFromPosition(2, 14).Name;

            // checking duplicate entry of job -->
            string query = " SELECT cust_po_number, sales_vars.id ";
            query += " FROM sales_vars ";
            query += " WHERE cust_po_number = '" + dbc.SelectSingle("cust_po_number", "sales_vars", "id", s_id) + "'; ";

            DataSet ds = new DataSet();
            int count = 0;

            ds = dbc.SelectMyDA(query);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (dbc.Count("SELECT COUNT(id) FROM graphics_vars_job WHERE sales_vars_id = '" + ds.Tables[0].Rows[i][1].ToString() + "';") > 0)
                    count++;
                if (count == ds.Tables[0].Rows.Count)
                    return;
            }

            // <--

            j_fldNames.AddRange(new string[] { "sales_vars_id", "graphics_vars_gang_id", "job_number", "Retailer", "ProjectedDueDate", "ActualDueDate", "PinCover", "Personalization_job", "CardRatio", "Other_Barcode", "MinPrintQty", "MagStripe", "Encode", "user_id" });

            j_fldVals.Add(s_id);
            j_fldVals.Add(((ComboBox)tlp_gd_insert_job.GetControlFromPosition(0, 11)).SelectedItem.ToString());
            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(0, 1).Text);
            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(1, 3).Text);
            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(2, 7).Text);
            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(2, 9).Text);
            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(1, 11).Text);

            if (((CheckBox)tlp_gd_insert_job.GetControlFromPosition(2, 11)).CheckState == CheckState.Checked)
                j_fldVals.Add("Y");
            else
                j_fldVals.Add("N");

            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(0, 13).Text);
            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(1, 13).Text);
            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(2, 13).Text);

            if (((CheckBox)tlp_gd_insert_job.GetControlFromPosition(0, 15)).CheckState == CheckState.Checked)
                j_fldVals.Add("Y");
            else
                j_fldVals.Add("N");

            j_fldVals.Add(tlp_gd_insert_job.GetControlFromPosition(1, 15).Text);
            j_fldVals.Add(user_id_lbl.Text);

            dbc.Insert(j_fldNames, "graphics_vars_job", j_fldVals, user_id_lbl.Text); //TODO: uncomment

            tlp_gd_insert_job.GetControlFromPosition(2, 15).Tag = tlp_gd_insert_job.GetControlFromPosition(0, 1).Text;
            tlp_gd_insert_job.GetControlFromPosition(2, 15).Enabled = true;
            tlp_gd_insert_job.GetControlFromPosition(2, 15).Click += new EventHandler(t_gd_insert_buttonClick);
        }

        private void t_gd_insert_buttonClick(object sender, EventArgs e)
        {
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            
            // adding fieldnames -->
            fldNames.Clear();
            fldNames.AddRange(new string[] { "job_number", "cust_name", "DATE_FORMAT(timestamp, '%Y-%m-%d')", "card_name", "Retailer", "card_denom", "order_qty", "concat(card_thickness, ' ', card_material)", "req_date", "cust_po_number", "card_size", "ProjectedDueDate", "sales_vars.user_id", "lamination", "ActualDueDate", "graphics_vars_gang_id", "PinCover", "Personalization_job", "CardRatio", "Other_Barcode", "MinPrintQty", "MagStripe", "Encode" });
            // -->

            // fetching data -->
            string query = "SELECT " + string.Join(", ", fldNames.ToArray());
            query += " FROM sales_vars ";
            query += " JOIN graphics_vars_job ON sales_vars_id = sales_vars.id ";
            query += " WHERE graphics_vars_job.job_number = " + ((Button)sender).Tag.ToString();
            DataSet ds = new DataSet();
            ds = dbc.SelectMyDA(query);
            //-->

            // adding labels -->
            fldNames.Clear();
            fldNames.AddRange(new string[] {"JobID", "Customer Name", "PODate", "Card Name", "Retailer", "Denomination", "Order Qty", "Core Material", "Customer Due Date", "PO", "Type Of Card", "Projected Due Date", "Sales", "Lamination", "Actual Due Date", "GangID", "Pin Cover", "Personalization Job", "Card Ratio", "Other", "Min Print Qty", "Mag Stripe", "Encode" });
            // -->

            fFunc.createXML_job("Personalization Detail Sheet", fldNames, ds);
        }

        private void btn2_GD_insert_InsertD_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> fldVals = new List<string>();
            string query = ((Button)sender).Tag.ToString(); // TODO: can replace with "". Plugged in value for testing purposes
            
            string gang_number = "Initial";
            if (comboBox_GD_insert_0x0.SelectedItem.ToString() == "New")
            {
                gang_number = textBox_GD_insert_0x1.Text;
                if (dbc.Count("SELECT COUNT(id) FROM graphics_vars_gang WHERE gang_number = '" + textBox_GD_insert_0x1.Text + "';") > 0)
                {
                    MessageBox.Show("Please enter new gang number", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
                gang_number = comboBox_GD_insert_0x0.SelectedItem.ToString();

            if (dbc.Count("SELECT COUNT(id) FROM graphics_vars_gang WHERE sales_vars_id = '" + label_GD_insert_0x3.Text + "' AND gang_number = '" + gang_number + "';") > 0)
                return;
            // adding field names -->
            fldNames.Clear();
            fldNames.AddRange(new string[] {"user_id", "sales_vars_id", "gang_number", "sheet_size", "print_qty", "issue_qty", "PrintOnFaceFront", "PrintOnFaceBack", "SideGuideFront", "SideGuideBack", "UVCoatingBack", "up_size" });
            // <--

            // adding field values -->
            fldVals.Add(user_id_lbl.Text);
            fldVals.Add(label_GD_insert_0x3.Text);
            fldVals.Add(gang_number);
            fldVals.Add(textBox_GD_insert_1x5.Text);
            fldVals.Add(textBox_GD_insert_2x7.Text);
            fldVals.Add(textBox_GD_insert_2x9.Text);
            fldVals.Add(textBox_GD_insert_0x7.Text);
            fldVals.Add(textBox_GD_insert_1x7.Text);
            fldVals.Add(textBox_GD_insert_0x11.Text);
            fldVals.Add(textBox_GD_insert_1x11.Text);
            fldVals.Add(textBox_GD_insert_1x13.Text);
            fldVals.Add(label_GD_insert_2x3.Text);
            // -->

            dbc.Insert(fldNames, "graphics_vars_gang", fldVals, user_id_lbl.Text); // TODO: uncomment

            button_GD_insert_gang_PS.Enabled = true;
            button_GD_insert_gang_PS.Tag = gang_number;
        }

        void t_textBoxInsert_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox = new TextBox();
            txtBox = (TextBox)sender;
            int up_size_new = 0;
            string s = txtBox.Tag.ToString();

            int diff = (Int32.Parse((txtBox.Text == "" ? "0" : txtBox.Text)) - Int32.Parse((txtBox.Tag.ToString() == "" ? "0" : txtBox.Tag.ToString())));
            up_size_new = diff + (Int32.Parse(label_GD_insert_2x3.Text));
            
            label_GD_insert_2x3.Text = up_size_new.ToString();
            label_GD_insert_2x5.Text = (Int32.Parse(label_GD_insert_1x3.Text) / Int32.Parse(label_GD_insert_2x3.Text)).ToString();
            txtBox.Tag = txtBox.Text;

            label_GD_insert_2x3.Tag = txtBox.Text;
        }

        private void comboBox_GD_insert_0x0_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableLayoutPanel tlp_temp = (TableLayoutPanel)tabControl_GD_insert.TabPages[tabControl_GD_insert.TabPages.Count-2].Controls[0];
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            if (comboBox_GD_insert_0x0.SelectedItem == "New")
            {
                textBox_GD_insert_0x1.Visible = true;
                button_GD_insert_gang_PS.Enabled = false;
            }
            else
            {
                textBox_GD_insert_0x1.Visible = false;
                button_GD_insert_gang_PS.Tag = comboBox_GD_insert_0x0.SelectedItem;
                button_GD_insert_gang_PS.Enabled = true;
                //tlp_temp.GetControlFromPosition(0, 1).Text = comboBox_GD_insert_0x0.SelectedItem.ToString();

                // refreshing gang tab -->
                fldNames.Clear();
                fldNames.Add("cust_name"); //i 0
                fldNames.Add("cust_po_number");
                fldNames.Add("SUM(order_qty)");
                fldNames.Add("card_material");
                fldNames.Add("num_colors");
                fldNames.Add("lamination");
                fldNames.Add("card_name");
                fldNames.Add("COUNT(id)");
                fldNames.Add("id"); //i 8
                fldNames.Add("user_id");
                fldNames.Add("id"); //i 10

                string query = "SELECT " + string.Join(", ", fldNames.ToArray());
                query += " FROM sales_vars ";
                query += " WHERE cust_po_number = '" + label_GD_insert_0x3.Text + "'";
                query += " GROUP BY cust_po_number; ";
                DataSet ds = new DataSet();
                ds.Clear();
                ds = dbc.SelectMyDA(query);

                label_GD_insert_1x1.Text = ds.Tables[0].Rows[0][0].ToString();
                label_GD_insert_0x3.Text = ds.Tables[0].Rows[0][1].ToString();
                label_GD_insert_1x3.Text = ds.Tables[0].Rows[0][2].ToString();
                label_GD_insert_0x5.Text = ds.Tables[0].Rows[0][3].ToString();
                label_GD_insert_0x9.Text = ds.Tables[0].Rows[0][4].ToString();
                label_GD_insert_0x13.Text = ds.Tables[0].Rows[0][5].ToString();

                button_gd_insert_gangsave.Tag = ds.Tables[0].Rows[0][8].ToString();

                // adding field names -->
                fldNames.Clear();
                fldNames.AddRange(new string[] { "sheet_size", "print_qty", "issue_qty", "PrintOnFaceFront", "PrintOnFaceBack", "SideGuideFront", "SideGuideBack", "UVCoatingBack", "up_size" });
                // <--

                query = "SELECT " + string.Join(", ", fldNames.ToArray());
                query += " FROM graphics_vars_gang ";
                query += " WHERE gang_number = '" + comboBox_GD_insert_0x0.SelectedItem.ToString() + "'";
                ds.Clear();
                ds = dbc.SelectMyDA(query);
                
                // updating field values -->
                textBox_GD_insert_1x5.Text = ds.Tables[0].Rows[0][0].ToString();
                textBox_GD_insert_2x7.Text = ds.Tables[0].Rows[0][1].ToString();
                textBox_GD_insert_2x9.Text = ds.Tables[0].Rows[0][2].ToString();
                textBox_GD_insert_0x7.Text = ds.Tables[0].Rows[0][3].ToString();
                textBox_GD_insert_1x7.Text = ds.Tables[0].Rows[0][4].ToString();
                textBox_GD_insert_0x11.Text = ds.Tables[0].Rows[0][5].ToString();
                textBox_GD_insert_1x11.Text = ds.Tables[0].Rows[0][6].ToString();
                textBox_GD_insert_1x13.Text = ds.Tables[0].Rows[0][7].ToString();
                label_GD_insert_2x3.Text = ds.Tables[0].Rows[0][8].ToString();
                // -->

            }
        }

        private void button_GD_insert_gang_PS_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> fldVals = new List<string>();
            DataSet ds = new DataSet();

            // adding labels -->
            fldNames.Clear();
            fldNames.AddRange(new string[] {"GangID", "CustomerName", "PO", "Qty", "UP", "Material", "Size", "RequireQty", "PrintQty", "IssueQty", "PrintOnFaceFront", "PrintOnFaceBack", "NumberOfColorFront", "NumberOfColorBack", "SideGuideFront", "SideGuideBack", "UVCoatingFront", "UVCoatingBack" });
            // <--

            // adding table fields -->
            fldVals.AddRange(new string[] { "gang_number", "cust_name", "cust_po_number", "(SELECT SUM(order_qty) FROM sales_vars WHERE sales_vars.cust_po_number = '"+dbc.SelectSingle("cust_po_number", "sales_vars", "id", dbc.SelectSingle("sales_vars_id", "graphics_vars_gang", "gang_number", ((Button)sender).Tag.ToString()))+"')", "up_size", "card_material", "sheet_size", "require_qty", "print_qty", "issue_qty", "PrintOnFaceFront", "PrintOnFaceBack", "NumberOfColorFront", "NumberOfColorBack", "SideGuideFront", "SideGuideBack", "UVCoatingFront", "UVCoatingBack" });
            // <--

            string query = " SELECT " + string.Join(", ", fldVals);
            query += " FROM graphics_vars_gang ";
            query += " JOIN sales_vars ON sales_vars.id = sales_vars_id ";
            query += " WHERE gang_number = " + ((Button)sender).Tag.ToString();

            ds = dbc.SelectMyDA(query);

            fFunc.createXML_gang("Press Information Sheet", fldNames, ds);
        }

        #endregion
        #region Graphics Tab - View TabPage
        private void btn1_GD_view_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            if (fFunc.checkSession(user_hash_lbl.Text.ToString(), double.Parse(ConfigurationManager.AppSettings["session_expiry"])) == false)
            {
                MessageBox.Show("Login expired ! Please login again.");
                loginEnable();
                tabControl1.SelectedTab = tabPage0;
            }
            else
            {
                TabControlGlobal TCG = new TabControlGlobal(user_id_lbl.Text, user_hash_lbl.Text);
                dbTableFunctions dbFunc = new dbTableFunctions(user_id_lbl.Text, user_hash_lbl.Text);

                GD_view_listView.Clear();
                GD_view_listView.View = View.Details;
                //fFunc.popListViewT(listView1, "po_details");


                DataSet ds = new DataSet();
                ds = dbc.SelectMyDA("SELECT * FROM po_details;");
                fFunc.popListViewDS(GD_view_listView, ds);

                //string query = "UPDATE `po_details` SET `job_number`= \"" + txtBox_gang_num.Text + "\", po_number = \"" + po_number_txtBox1.Text + "\", gang_number = \"" + gang_number_txtBox1.Text + "\" WHERE  `id`=1 LIMIT 1; ";
                //dbc.Update(query);
            }
        }
        private void btn2_GD_view_Click(object sender, EventArgs e)
        {
            ConnectToPostgreSQL pgdb = new ConnectToPostgreSQL();

            List<string> fldNames = new List<string>();
            fldNames.Add("jccostcenterid");
            fldNames.Add("jcdescription");

            DataSet ds2 = pgdb.Select(fldNames, "costcenter");
            DataTable dt = new DataTable();
            dt = ds2.Tables[0];
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            fFunc.popListViewDS(GD_view_listView, ds2);
        }
        #endregion
        #region Graphics Tab - Edit TabPage
        private void button_GD_edit_search_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            string s1 = (textBox_GD_edit_GNumSearch.Text == "") ? "" : "%" + textBox_GD_edit_GNumSearch.Text + "%";
            string s2 = (textBox_GD_edit_PONumSearch.Text == "") ? "" : "%" + textBox_GD_edit_PONumSearch.Text + "%";

            string query = "SELECT cust_po_number, cust_name, SUM(order_qty), COUNT(po_details.id) ";
            string query2 = "SELECT po_entry.id ";
            string query3 = "FROM po_entry ";
            query3 += " JOIN po_details ON po_entry.id = po_details.po_entry_id ";
            query3 += " JOIN gang_number ON po_details.gang_number_id = gang_number.id ";
            query3 += " WHERE po_details.po_entry_id IN ";
            query3 += " (SELECT po_entry.id ";
            query3 += " FROM po_entry ";
            query3 += " JOIN po_details ON po_details.po_entry_id = po_entry.id ";
            query3 += " JOIN gang_number ON po_details.gang_number_id = gang_number.id ";
            query3 += " WHERE gang_number.gang_number_id LIKE '" + s1 + "' OR ";
            query3 += " cust_po_number LIKE '" + s2 + "' ";
            query3 += " GROUP BY po_entry.id)";
            query3 += " GROUP BY cust_po_number;";

            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            List<string> IDs = new List<string>();

            ds = dbc.SelectMyDA(query + query3);
            ds2 = dbc.SelectMyDA(query2 + query3);
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                IDs.Add(ds2.Tables[0].Rows[i][0].ToString());
            }
            fFunc.popListViewDS(listView_GD_edit_searchResult, ds, assignColNames: false, assignTag: true, assignTagBy: IDs);
        }

        private void button_GD_edit_loadPoD_Click(object sender, EventArgs e)
        {
            // --> Get po details data based on customer po number
            if (listView_GD_edit_searchResult.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select the PO entry first.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            label_GD_edit_update_status.Visible = false;
            string po_entry_id = "";
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            // adding field names of po_details table -->
            fldNames.Clear();
            fldNames.Add("cust_name"); //i 0
            fldNames.Add("cust_po_number"); //i 1
            fldNames.Add("order_qty"); //i 2
            fldNames.Add("card_material"); //i 3
            fldNames.Add("num_colors"); //i 4
            fldNames.Add("lamination"); //i 5
            fldNames.Add("card_name"); //i 6
            fldNames.Add("gang_number.gang_number_id"); //i 7
            fldNames.Add("po_details.id"); //i 8
            fldNames.Add("po_details.user_id"); //i 9
            fldNames.Add("po_entry_id"); //i 10
            fldNames.Add("po_details.gang_number_id"); //i 11
            fldNames.Add("sheet_size"); //i 12
            fldNames.Add("PrintOnFaceFront"); //i 13
            fldNames.Add("PrintOnFaceBack"); //i 14
            fldNames.Add("SideGuideFront"); //i 15
            fldNames.Add("SideGuideBack"); //i 16
            fldNames.Add("UVCoatingBack"); //i 17
            fldNames.Add("print_qty"); //i 18
            fldNames.Add("issue_qty"); //i 19
            //<--

            po_entry_id = listView_GD_edit_searchResult.SelectedItems[0].Tag.ToString();

            DataSet ds = new DataSet();
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            string[] cbox_items = fFunc.getComboItems("", "", "gang_number_id", "gang_number", _order_by: "id", _whrClause: " WHERE id != 1 AND id != 2 ");

            string query = "SELECT " + string.Join(", ", fldNames.ToArray());
            query += " FROM po_details ";
            query += " JOIN po_entry ON po_entry.id = po_details.po_entry_id ";
            query += " JOIN gang_number ON po_details.gang_number_id = gang_number.id ";
            query += " WHERE po_entry_id = '" + po_entry_id + "';";
            //query += " GROUP BY cust_po_number; ";
            ds = dbc.SelectMyDA(query);

            int order_qty = 0;
            int job_lines = ds.Tables[0].Rows.Count;

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                order_qty += Int32.Parse(ds.Tables[0].Rows[j][2].ToString());

            label_GD_edit_PoD1x1.Text = ds.Tables[0].Rows[0][0].ToString();
            label_GD_edit_PoD0x3.Text = ds.Tables[0].Rows[0][1].ToString();
            label_GD_edit_PoD1x3.Text = order_qty.ToString();
            label_GD_edit_PoD0x5.Text = ds.Tables[0].Rows[0][3].ToString();
            label_GD_edit_PoD0x9.Text = ds.Tables[0].Rows[0][4].ToString();
            label_GD_edit_PoD0x13.Text = ds.Tables[0].Rows[0][5].ToString();

            comboBox_GD_edit_PoD0x1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_GD_edit_PoD0x1.FlatStyle = FlatStyle.Popup;
            comboBox_GD_edit_PoD0x1.Items.Clear();
            comboBox_GD_edit_PoD0x1.Items.AddRange(cbox_items);
            comboBox_GD_edit_PoD0x1.SelectedItem = dbc.SelectSingle("gang_number_id", "gang_number", "id", ds.Tables[0].Rows[0][11].ToString());

            textBox_GD_edit_PoD1x5.Text = ds.Tables[0].Rows[0][12].ToString();
            textBox_GD_edit_PoD0x7.Text = ds.Tables[0].Rows[0][13].ToString();
            textBox_GD_edit_PoD1x7.Text = ds.Tables[0].Rows[0][14].ToString();
            textBox_GD_edit_PoD0x11.Text = ds.Tables[0].Rows[0][15].ToString();
            textBox_GD_edit_PoD1x11.Text = ds.Tables[0].Rows[0][16].ToString();
            textBox_GD_edit_PoD1x13.Text = ds.Tables[0].Rows[0][17].ToString();
            textBox_GD_edit_PoD2x7.Text = ds.Tables[0].Rows[0][18].ToString();
            textBox_GD_edit_PoD2x9.Text = ds.Tables[0].Rows[0][19].ToString();

            checkedListBox_GD_edit_PoD2x0.Items.Clear();
            checkedListBox_GD_edit_PoD2x0.Items.Add("Initial");
            checkedListBox_GD_edit_PoD2x0.Items.AddRange(fFunc.getComboItems("", "", "gang_number_id", "gang_number", _order_by: "id", _whrClause: " WHERE id != 1 AND id != 2 "));
            
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                checkedListBox_GD_edit_PoD2x0.SetItemChecked(checkedListBox_GD_edit_PoD2x0.Items.IndexOf(ds.Tables[0].Rows[j][7].ToString()), true);

            DataSet ds2 = new DataSet();
            string query2 = "SELECT card_name, up_size, po_details.id, job_number, gang_number.gang_number_id ";
            query2 += " FROM po_details ";
            query2 += " JOIN gang_number on gang_number.id = po_details.gang_number_id ";
            query2 += " WHERE po_entry_id = '" + po_entry_id + "' ORDER BY job_number; ";
            ds2 = dbc.SelectMyDA(query2);
            int up_size = 0;

            Label t_label1 = new Label();
            Label t_label2 = new Label();
            Label t_label3 = new Label();
            Label t_label4 = new Label();

            tableLayoutPanel_GD_edit_JobD.Controls.Clear();
            t_label1.Text = "Job Number";
            tableLayoutPanel_GD_edit_JobD.Controls.Add(t_label1, 0, 0);

            t_label2.Text = "Gang Number";
            tableLayoutPanel_GD_edit_JobD.Controls.Add(t_label2, 1, 0);

            t_label3.Text = "Job Name";
            tableLayoutPanel_GD_edit_JobD.Controls.Add(t_label3, 2, 0);

            t_label4.Text = "Job UP Size";
            t_label4.Tag = ds2.Tables[0].Rows.Count;
            tableLayoutPanel_GD_edit_JobD.Controls.Add(t_label4, 3, 0);

            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                TextBox t_textBox = new TextBox();
                TextBox t_textBox2 = new TextBox();
                Label t_label = new Label();
                ComboBox t_cbox = new ComboBox();

                t_textBox.Text = ds2.Tables[0].Rows[i][3].ToString();
                t_textBox.Tag = ds2.Tables[0].Rows[i][2].ToString();

                t_cbox.Items.Add("Initial");
                t_cbox.Items.AddRange(cbox_items);
                t_cbox.SelectedItem = "Initial";
                t_cbox.SelectedItem = txtBox_gang_num.Text;
                t_cbox.SelectedItem = ds2.Tables[0].Rows[i][4].ToString();

                t_label.AutoSize = true;
                t_label.Text = ds2.Tables[0].Rows[i][0].ToString();

                t_textBox2.Text = ds2.Tables[0].Rows[i][1].ToString();
                t_textBox2.Tag = ds2.Tables[0].Rows[i][1].ToString();

                tableLayoutPanel_GD_edit_JobD.SuspendLayout();
                tableLayoutPanel_GD_edit_JobD.Controls.Add(t_textBox, 0, i + 1);
                tableLayoutPanel_GD_edit_JobD.Controls.Add(t_cbox, 1, i + 1);
                tableLayoutPanel_GD_edit_JobD.Controls.Add(t_label, 2, i + 1);
                tableLayoutPanel_GD_edit_JobD.Controls.Add(t_textBox2, 3, i + 1);
                tableLayoutPanel_GD_edit_JobD.ResumeLayout();

                up_size += Int32.Parse(ds2.Tables[0].Rows[i][1].ToString());

                t_textBox2.TextChanged += t_textBoxEdit_TextChanged;
            }
            label_GD_edit_PoD2x3.Text = up_size.ToString();
            label_GD_edit_PoD2x5.Text = (Int32.Parse(label_GD_edit_PoD1x3.Text) / up_size).ToString();
        }

        void t_textBoxEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBox = new TextBox();
            txtBox = (TextBox)sender;
            int up_size_new = 0;
            string s = txtBox.Tag.ToString();

            int diff = (Int32.Parse((txtBox.Text == "" ? "0" : txtBox.Text)) - Int32.Parse((txtBox.Tag.ToString() == "" ? "0" : txtBox.Tag.ToString())));
            up_size_new = diff + (Int32.Parse(label_GD_edit_PoD2x3.Text));

            label_GD_edit_PoD2x3.Text = up_size_new.ToString();
            label_GD_edit_PoD2x5.Text = (Int32.Parse(label_GD_edit_PoD1x3.Text) / Int32.Parse(label_GD_edit_PoD2x3.Text)).ToString();
            txtBox.Tag = txtBox.Text;
        }

        private void button_GD_edit_JobD_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);

            string query = "";
            //string gang_id = dbc.SelectSingle("id", "gang_number", "gang_number_id", comboBox_GD_edit_PoD0x1.SelectedItem.ToString());

            for (int i = 0; i < Int32.Parse(tableLayoutPanel_GD_edit_JobD.GetControlFromPosition(3, 0).Tag.ToString()); i++)
            {
                string gang_id = dbc.SelectSingle("id", "gang_number", "gang_number_id", tableLayoutPanel_GD_edit_JobD.GetControlFromPosition(1, i + 1).Text);
                query = "UPDATE po_details SET ";
                query += " gang_number_id = '" + gang_id + "', ";
                query += " sheet_size = '" + textBox_GD_edit_PoD1x5.Text + "', ";
                query += " print_qty = '" + textBox_GD_edit_PoD2x7.Text + "', ";
                query += " issue_qty = '" + textBox_GD_edit_PoD2x9.Text + "', ";
                query += " PrintOnFaceFront = '" + textBox_GD_edit_PoD0x7.Text + "', ";
                query += " PrintOnFaceBack = '" + textBox_GD_edit_PoD1x7.Text + "', ";
                query += " SideGuideFront = '" + textBox_GD_edit_PoD0x11.Text + "', ";
                query += " SideGuideBack = '" + textBox_GD_edit_PoD1x11.Text + "', ";
                query += " UVCoatingBack = '" + textBox_GD_edit_PoD1x13.Text + "',";
                query += " job_number = '" + tableLayoutPanel_GD_edit_JobD.GetControlFromPosition(0, i + 1).Text + "', ";
                query += " up_size = '" + tableLayoutPanel_GD_edit_JobD.GetControlFromPosition(3, i + 1).Text + "' ";
                query += " WHERE id = '" + tableLayoutPanel_GD_edit_JobD.GetControlFromPosition(0, i + 1).Tag.ToString() + "' ;";

                dbc.Update(query); // TODO: uncomment
            }
            label_GD_edit_update_status.Visible = true;
            label_GD_edit_update_status.Text = "Update Successful !";
        }

        #endregion   
        #endregion
        #region Printing Tab
        #region Printing Tab - View Tabpage
        private void Button_PT_view_updateLS_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);

            string[] machines = fFunc.getComboItems("deptID", "1", "machine_name", "machines", _order_by: "machine_name");

            tLP_PT_view.Controls.Clear();
            tLP_PT_view.ColumnCount = 2*(machines.Length);
            tLP_PT_view.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            for(int i = 0; i < tLP_PT_view.ColumnCount; i++)
                tLP_PT_view.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.AutoSize));

            for (int i = 0; i < machines.Length; i++ )
            {

                Label t_label = new Label();
                DateTimePicker tp = new DateTimePicker();
                DateTimePicker dp = new DateTimePicker();

                tp.Format = DateTimePickerFormat.Time;
                tp.ShowUpDown = true;
                tp.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);

                t_label.Text = machines[i];
                tLP_PT_view.Controls.Add(t_label, i, 0);
                tLP_PT_view.SetColumnSpan(t_label, 2);
                tLP_PT_view.Controls.Add(dp, (i*2), 1);
                tLP_PT_view.Controls.Add(tp, ((i*2)+1), 1);
            }

            string query = " SELECT gang_number, PrintingType ";
            query += " FROM graphics_vars_gang ";
            query += " JOIN sales_vars ON sales_vars.cust_po_number = sales_vars_id ";
            query += " WHERE Printing = 'Y'";
            query += " AND ArtApproved = 'Y'";
            query += " GROUP BY gang_number ";

            DataSet ds = new DataSet();
            ds = dbc.SelectMyDA(query);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                List<Label> row = new List<Label>();

                for (int j = 0; j < tLP_PT_view.ColumnCount; j++)
                {
                    Label t_label2 = new Label(); t_label2.AutoSize = true;
                    Label t_label3 = new Label(); t_label2.AutoSize = true;
                    if (ds.Tables[0].Rows[i][1].ToString() == tLP_PT_view.GetControlFromPosition(j, 0).Text)
                    {

                        t_label2.Text = ds.Tables[0].Rows[i][0].ToString();
                        t_label3.Text = "";
                        
                        row.Add(t_label2);
                        row.Add(t_label3);
                    }
                    else
                    {
                        t_label2.Text = "";
                        t_label3.Text = "";

                        row.Add(t_label2);
                        row.Add(t_label3);
                    }
                    j++;
                }

                tLP_PT_view.Controls.AddRange(row.ToArray());

                
                for (int j = 0; j < tLP_PT_view.ColumnCount; j++)
                {
                    Control ctrl = tLP_PT_view.GetControlFromPosition(j, i + 1);
                    string issueQty = "1";
                    Double timeReqd = 1;                    

                    if (ctrl.GetType() == typeof(Label))
                    {
                        if (row[j].Text != "")
                        {
                            string str = tLP_PT_view.GetControlFromPosition(j+1, i + 1).Text;
                            DateTime dt = (DateTime)tLP_PT_view.GetControlFromPosition(j+1, i + 1).Tag;
                            row[j + 1].Text = "S: " + dt.ToString();
                            issueQty = dbc.SelectSingle("issue_qty", "graphics_vars_gang", "gang_number", row[j].Text);
                            timeReqd = fFunc.getJobRuntime(tLP_PT_view.GetControlFromPosition(j, 0).Text, issueQty);
                            //row[j + 1].Text += " F: " + tLP_PT_view.GetControlFromPosition(j, i + 1).Tag.ToString() + " " + ((DateTimePicker)tLP_PT_view.GetControlFromPosition(j + 1, i + 1)).Value.AddHours(timeReqd).ToShortTimeString();
                            dt = dt.AddHours(timeReqd);
                            //row[j + 1].Tag = ((DateTimePicker)tLP_PT_view.GetControlFromPosition(j + 1, i + 1)).Value.AddHours(timeReqd).ToShortTimeString();
                            row[j + 1].Text += " F: "+ dt.ToString();
                            row[j + 1].Tag = dt;
                        }
                    }
                    else
                    {
                        if (row[j].Text != "")
                        {
                            DateTime dt = new DateTime(((DateTimePicker)ctrl).Value.Year, ((DateTimePicker)ctrl).Value.Month, ((DateTimePicker)ctrl).Value.Day, ((DateTimePicker)tLP_PT_view.GetControlFromPosition(j + 1, i + 1)).Value.Hour, ((DateTimePicker)tLP_PT_view.GetControlFromPosition(j + 1, i + 1)).Value.Minute, ((DateTimePicker)tLP_PT_view.GetControlFromPosition(j + 1, i + 1)).Value.Second);

                            string str = "S: " + dt.ToString() + "+" ;
                            issueQty = dbc.SelectSingle("issue_qty", "graphics_vars_gang", "gang_number", row[j].Text);
                            timeReqd = fFunc.getJobRuntime(tLP_PT_view.GetControlFromPosition(j,0).Text, issueQty);
                            //row[j + 1].Text += " F: " + ((DateTimePicker)ctrl).Value.ToShortDateString() + " " + ((DateTimePicker)tLP_PT_view.GetControlFromPosition(j + 1, i + 1)).Value.AddHours(timeReqd).ToShortTimeString();
                            dt = dt.AddHours(timeReqd);
                            str += " F: " + dt.ToString();
                            row[j + 1].Text = str;
                            row[j + 1].Tag = dt;
                        }
                    }
                    j++;
                }
            }

        }
        #endregion
        #endregion
        #region Mid Tab
        #region Mid Tab - View Tabpage
        private void button_MID_view_updateLS_Click(object sender, EventArgs e)
        {
            ConnectToMySQL dbc = new ConnectToMySQL(user_id_lbl.Text, user_hash_lbl.Text);
            FormFunctions fFunc = new FormFunctions(user_id_lbl.Text, user_hash_lbl.Text);
            List<string> machines = new List<string>();

            machines.AddRange(fFunc.getComboItems("deptID", "3", "machine_name", "machines", _order_by: "machine_name"));
            //machines.AddRange(fFunc.getComboItems("deptID", "4", "machine_name", "machines", _order_by: "machine_name"));
            machines.AddRange(fFunc.getComboItems("deptID", "5", "machine_name", "machines", _order_by: "machine_name"));

            tLP_MID_view.Controls.Clear();
            tLP_MID_view.ColumnCount = 2 * (machines.Count);
            tLP_MID_view.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            for (int i = 0; i < tLP_MID_view.ColumnCount; i++)
                tLP_MID_view.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());

            for (int i = 0; i < machines.Count; i++)
            {

                Label t_label = new Label();
                DateTimePicker tp = new DateTimePicker();
                DateTimePicker dp = new DateTimePicker();

                tp.Format = DateTimePickerFormat.Time;
                tp.ShowUpDown = true;
                tp.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);

                t_label.Text = machines[i];
                tLP_MID_view.Controls.Add(t_label, i, 0);
                tLP_MID_view.SetColumnSpan(t_label, 2);
                tLP_MID_view.Controls.Add(dp, (i * 2), 1);
                tLP_MID_view.Controls.Add(tp, ((i * 2) + 1), 1);
            }

            string query = " SELECT gang_number, MagneticTapeType, DieCutType ";
            query += " FROM graphics_vars_gang ";
            query += " JOIN sales_vars ON sales_vars.cust_po_number = sales_vars_id ";
            query += " WHERE (MagneticTape = 'Y' OR DieCut = 'Y') ";
            query += " AND ArtApproved = 'Y'";
            query += " GROUP BY gang_number ";

            DataSet ds = new DataSet();
            ds = dbc.SelectMyDA(query);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                List<Label> row = new List<Label>();

                for (int j = 0; j < tLP_MID_view.ColumnCount; j++)
                {
                    Label t_label2 = new Label();
                    Label t_label3 = new Label();
                    if (ds.Tables[0].Rows[i][1].ToString() == tLP_MID_view.GetControlFromPosition(j, 0).Text)
                    {

                        t_label2.Text = ds.Tables[0].Rows[i][0].ToString();
                        t_label3.Text = "";

                        row.Add(t_label2);
                        row.Add(t_label3);
                    }
                    else if (ds.Tables[0].Rows[i][2].ToString() == tLP_MID_view.GetControlFromPosition(j, 0).Text)
                    {

                        t_label2.Text = ds.Tables[0].Rows[i][0].ToString();
                        t_label3.Text = "";

                        row.Add(t_label2);
                        row.Add(t_label3);
                    }
                    else
                    {
                        t_label2.Text = "";
                        t_label3.Text = "";

                        row.Add(t_label2);
                        row.Add(t_label3);
                    }
                    j++;
                }

                tLP_MID_view.Controls.AddRange(row.ToArray());
            }
        }
        #endregion
        #endregion
    }
}
