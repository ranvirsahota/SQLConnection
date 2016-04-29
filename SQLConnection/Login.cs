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
                if (string.IsNullOrEmpty(txtBoxSQLSeverName.Text) && !string.IsNullOrEmpty(txtDBName.Text) && string.IsNullOrEmpty(txtBoxUserName.Text) && string.IsNullOrEmpty(txtBoxPassword.Text))
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
                else if (!string.IsNullOrEmpty(txtBoxSQLSeverName.Text) && !string.IsNullOrEmpty(txtDBName.Text) && !string.IsNullOrEmpty(txtBoxUserName.Text) && !string.IsNullOrEmpty(txtBoxPassword.Text))
                {
                    connectionString = "server=" + txtBoxSQLSeverName.Text + "; userid=" + txtBoxUserName.Text + "; password=" + txtBoxPassword.Text + "; database=" + txtDBName.Text + ";" + "Convert Zero Datetime = True";
                    dh = new DataHelper(connectionString);
                    if (dh.OpenConnectionToMySQLDB() == true)
                    {
                        MessageBox.Show("You've connected to MySQL database: "+txtDBName.Text);
                        this.Hide();
                        new MainMenu(connectionString).Show();
                    }
                }
                else
                {
                    MessageBox.Show("Error: with login");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to login");
                MessageBox.Show(ex.ToString());
                this.Show();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you want to connect to an SQLite database you just need to type in the database name into the database text box and make sure all other text boxes have nothing inputted." + Environment.NewLine + "If you want to connect to a MySQL database then textboxes need to have value that is equal to your login details.");
        }
    }
}