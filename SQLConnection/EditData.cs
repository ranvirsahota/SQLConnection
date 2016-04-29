using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLConnection
{
    public partial class editData : Form
    {
        private string connectionString = "";
        private string comboBoxTableSelectedValue = "";
        private string firstColumn = "";
        private DataHelper dh;
        private DataTable dt;
        private bool btnBackClicked = false;
        private bool newRow = false;
        private string id = "";
        public editData(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            dh = new DataHelper(this.connectionString);

            //this is executing the query and returning all the results in a datatable.
            if (connectionString.Contains("Data Source"))
            {
                dt = dh.ExecuteReturnQuery("select name from sqlite_master where type = \"table\" and name like \"tbl%\" or type=\"view\" and name like \"view%\"");
            }
            else
            {
                dt = dh.ExecuteReturnQuery("SHOW TABLES;");
            }
            //this is iterating through all rows in the datatable
            foreach (DataRow row in dt.Rows)
            {
                //this is grabbing the first column in the row and adding to combobox
                comboBoxTable.Items.Add(row.ItemArray[0]);
            }
        }

        private void dataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string currentColumn = dataGrid.CurrentCell.OwningColumn.HeaderText;
            string value = dataGrid.CurrentCell.Value.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                if (newRow)
                {

                    dh.ExecuteQuery("INSERT INTO `" + comboBoxTableSelectedValue + "`(`" + currentColumn + "`) VALUES('" + value + "');");
                    dataGrid.CurrentRow.Cells[0].Value = dataGrid.CurrentRow.Index + 1;
                    newRow = false;
                }
                else
                {
                    dh.ExecuteQuery("UPDATE `" + comboBoxTableSelectedValue + "` SET `" + currentColumn + "`='" + value + "' WHERE `" + firstColumn + "`='" + id + "';");
                }
            }
            else
            {
                MessageBox.Show("Nothing was inputted");
            }
        }

        private void comboBoxTable_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBoxTableSelectedValue = comboBoxTable.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(comboBoxTableSelectedValue))
            {
                dataGrid.DataSource = dh.ExecuteReturnQuery("SELECT * FROM " + comboBoxTableSelectedValue).DefaultView;
                if (comboBoxTableSelectedValue.Contains("view") || string.Equals(comboBoxTableSelectedValue, "tblIUCR"))
                {
                    dataGrid.ReadOnly = true;
                    dataGrid.AllowUserToAddRows = false;
                }
                else
                {
                    dataGrid.ReadOnly = false;
                    dataGrid.Columns[0].ReadOnly = true;
                    dataGrid.AllowUserToAddRows = true;
                }
                firstColumn = dataGrid.Columns[0].HeaderText;
                dataGrid.Refresh();
            }
            else
            {
                MessageBox.Show("No table selected");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string id = txtBoxID.Text;
            if (!string.IsNullOrEmpty(comboBoxTableSelectedValue))
            {
                if (!string.IsNullOrEmpty(id))
                {
                    dataGrid.DataSource = dh.ExecuteReturnQuery("SELECT * FROM `" + comboBoxTableSelectedValue + "` WHERE `" + firstColumn + "`='" + txtBoxID.Text + "';").DefaultView;
                    dataGrid.AllowUserToAddRows = false;
                }
                else
                {
                    MessageBox.Show("No value was enterd into the text box");
                }
            }
            else
            {
                MessageBox.Show("No table is selected");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            new MainMenu(connectionString).Show();
            btnBackClicked = true;
            this.Close();

        }

        private void editData_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!btnBackClicked)
            {
                Application.Exit();
            }
        }

        private void dataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGrid.CurrentRow.IsNewRow)
            {
                newRow = true;
            }
            else
            {
                id = dataGrid.CurrentRow.Cells[0].Value.ToString();
            }
        }
    }
}