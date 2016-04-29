using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Threading;
namespace SQLConnection
{
    public class DataHelper
    {
        private string connectionString = "";
        public DataHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // this will execute a query and return true or false if succeeded or failed (wont return db contents)
        public bool ExecuteQuery(string command)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    if (connectionString.Contains("Data Source"))
                    {
                        SQLiteConnection connection = new SQLiteConnection(connectionString);
                        connection.Open();

                        SQLiteCommand cmd = new SQLiteCommand(command, connection);
                        cmd.ExecuteNonQuery(); //executes query and returns number of rows effected
                        connection.Close();
           
                        return true;
                    }
                    else
                    {
                        MySqlConnection connection = new MySqlConnection(connectionString);
                        connection.Open();

                        MySqlCommand cmd = new MySqlCommand(command, connection);
                        cmd.ExecuteNonQuery(); //executes query and returns number of rows effected
                        connection.Close();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error executing query" + Environment.NewLine + ex.ToString());
                    return false;
                }
                finally
                {

                }
            }
            else
            {
                MessageBox.Show("Connection string is null or empty.");
                return false;
            }
        }

        // this will execute and return information based on the query inputted
        public DataTable ExecuteReturnQuery(string query)
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    if (connectionString.Contains("Data Source"))
                    {
                        DataTable dt = new DataTable();
  
                        SQLiteConnection connection = new SQLiteConnection(connectionString);
                        connection.Open();

                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                        adapter.SelectCommand.CommandType = CommandType.Text;
                        adapter.Fill(dt);
                        connection.Close();
                        return dt;
                    }
                    else
                    {
                        DataTable dt = new DataTable();

                        MySqlConnection connection = new MySqlConnection(connectionString);
                        connection.Open();

                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                        adapter.SelectCommand.CommandType = CommandType.Text;
                        adapter.Fill(dt);

                        connection.Close();

                        return dt;
                    }
                }
                else
                {
                    MessageBox.Show("Connection string is null or empty");
                    return null;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error executing query" + Environment.NewLine + ex.ToString());
                return null;
            }
        }

        public bool OpenConnectionToSQLiteDB()
        {
            try
            {   
                SQLiteConnection sqlConnection = new SQLiteConnection(connectionString);
                sqlConnection.Open();
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database " + Environment.NewLine + ex.ToString());
                return false;
            }
        }

        public bool OpenConnectionToMySQLDB()
        {
            try
            {
                MySqlConnection sqlConnection = new MySqlConnection(connectionString);
                sqlConnection.Open();
                sqlConnection.Close();
                return true;   
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error connecting to database " + Environment.NewLine + ex.ToString());
                return false;
            }
        }
    }
}
