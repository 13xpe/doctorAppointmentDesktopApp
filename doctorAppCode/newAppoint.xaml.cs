using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoctorSoftwareApp
{
    /// <summary>
    /// Interaction logic for newAppoint.xaml
    /// </summary>
    public partial class newAppoint : Window
    {
        public newAppoint()
        {
            InitializeComponent();
        }

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

        private string get_ConnectingString()
        {
            //To connect with postgreSQL we need values: host, port, dbName, userName, password
            string host = "Host=localhost;";
            String port = "Port=5432;";
            string dbName = "Database=DoctorApp;";
            string userName = "Username=postgres;";
            string password = "Password=atuaprima;";

            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;
        }

        private void establishConnect()
        {
            try
            {
                con = new NpgsqlConnection(get_ConnectingString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            appointment checkAppoint = new appointment();
            checkAppoint.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            appointment goBackAppoint = new appointment();
            goBackAppoint.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                // Step 1: Establish connection
                establishConnect();

                // Step 2: Open the connection
                con.Open();

                // Step 3: Query Generation
                string Query = "INSERT INTO appointments (appointment_patient_id, appointment_patient_name, appointment_date, appointment_notes) VALUES (@appointment_id, @appointment_name, @appointment_date, @appointment_notes)";

                // Step 4: Initialize the command adapter of the database
                cmd = new NpgsqlCommand(Query, con);

                // Step 4.1: Initialize the parameters in the variables of the command
                cmd.Parameters.AddWithValue("@appointment_id", appointment_id.Text);
                cmd.Parameters.AddWithValue("@appointment_name", appointment_name.Text);
                cmd.Parameters.AddWithValue("@appointment_date", appointment_date.Text);
                cmd.Parameters.AddWithValue("@appointment_notes", appointment_notes.Text);

                // Step 5: Execute the query    
                cmd.ExecuteNonQuery();

                // Step 6: Successful Message
                MessageBox.Show("Appointment created successfully");

                // Step 7: Close the connection
                con.Close();

                // Go back to main window
                Dashboard dashboardd = new Dashboard();
                dashboardd.Show();
                this.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
