using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DoctorSoftwareApp
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private string get_ConnectionString()
        {
            //For PGSQL connectionString we need five values: host, port, dbName, userName, password

            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=DoctorApp;";
            string userName = "Username=postgres;";
            string password = "Password=atuaprima;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Giving the values for username and password
            string inputUsername = username.Text;
            string inputPassword = password.Password;

            //Connect with database
            NpgsqlConnection con = new NpgsqlConnection(get_ConnectionString());
            con.Open();

            //Create a QUERY to verify the login info
            string query = "SELECT doctor_username, doctor_password FROM doctors WHERE doctor_username = @username";
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", inputUsername);

            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string storedPassword = reader["doctor_password"].ToString();

                if (inputPassword == storedPassword)
                {
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong password");
                }
            }
            else
            {
                MessageBox.Show("User not found");
            }

            con.Close();
        }


        //Open doctor registration 
        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            docReg doctorRegistration = new docReg();
            doctorRegistration.Show();
            this.Close();
        }
    }
}
