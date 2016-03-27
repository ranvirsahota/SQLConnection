using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.IO;

namespace SQLConnection
{
    public partial class Login : Form
    {
        private string connectionString = "";
        private DataHelper dh;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                connectionString = "";
                if (string.IsNullOrEmpty(txtBoxSQLSeverName.Text))
                {
                    string path = "Databases/";
                    DirectoryInfo directory = new DirectoryInfo(path);
                    FileInfo[] files = directory.GetFiles("*.db");

                    for (int i = 0; i < files.Length; i++)
                    {
                        if (txtDBName.Text.Equals(files[i].Name.Split('.')[0]))
                        {
                            connectionString = "Data Source=" + txtDBName.Text + ".db";                            
                            break;
                        }
                    }
                    dh = new DataHelper(connectionString);

                    if (dh.OpenConnectionToSQLiteDB() == true)
                    {
                        MessageBox.Show("You've connected to SQLite database: " + txtDBName.Text);
                        this.Hide();
                        new MainMenu(connectionString).Show();
                    }
                }
                else
                {

                    connectionString = "server=" + txtBoxSQLSeverName.Text + "; userid=" + txtBoxUserName.Text + "; password=" + txtBoxPassword.Text + "; database=" + txtDBName.Text + ";";
                    dh = new DataHelper(connectionString);
                    if (dh.OpenConnectionToMySQLDB() == true)
                    {
                        MessageBox.Show("You've connected to MySQL database: "+txtDBName.Text);
                        this.Hide();
                        new MainMenu(connectionString).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to login");
                MessageBox.Show(ex.ToString());
                this.Show();
            }
        }
    }
}