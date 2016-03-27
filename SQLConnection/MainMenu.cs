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
    public partial class MainMenu : Form
    {
        private string connectionString = "";
        private bool btnBackClicked = false;

        public MainMenu(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void radioBtnEditData_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    this.Hide();
                    radioBtnEditData.Checked = false;
                    new editData(connectionString).Show();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Close();
            btnBackClicked = true;
        }

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!btnBackClicked)
            {
                Application.Exit();
            }
        }
        /*
private void btnChange_Click(object sender, EventArgs e)
{
  string extractactedConnectionString = "";
  DataHelper dh;
  if (connectionString.Contains("Data Source="))
  {
      extractactedConnectionString = connectionString.Split('=')[0];
  }
  else
  {

      dh = new DataHelper(connectionString);
      if (dh.d() == true)
      {
          MessageBox.Show("You've connected to MySQL database: " + txtDBName.Text);
          this.Hide();
          new MainMenu(connectionString).Show();
      }
  }
}
*/
    }
}
