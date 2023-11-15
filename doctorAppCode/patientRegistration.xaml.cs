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
    /// Interaction logic for patientRegistration.xaml
    /// </summary>
    public partial class patientRegistration : Window
    {
        public patientRegistration()
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

        private void register_patient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Step 1: Establish connection
                establishConnect();

                // Step 2: Open the connection
                con.Open();

                // Step 3: Query Generation
                string Query = "INSERT INTO patients (patient_first_name, patient_last_name, patient_year, patient_phone_number, patient_notes) VALUES (@patient_first_name, @patient_last_name, @patient_year, @patient_phone_number, @patient_notes)";

                // Step 4: Initialize the command adapter of the database
                cmd = new NpgsqlCommand(Query, con);

                // Step 4.1: Initialize the parameters in the variables of the command
                cmd.Parameters.AddWithValue("@patient_first_name", patient_first_name.Text);
                cmd.Parameters.AddWithValue("@patient_last_name", patient_last_name.Text);
                cmd.Parameters.AddWithValue("@patient_year", long.Parse(patient_year.Text));
                cmd.Parameters.AddWithValue("@patient_phone_number", long.Parse(patient_phone_number.Text));
                cmd.Parameters.AddWithValue("@patient_notes", patient_notes.Text);

                // Step 5: Execute the query    
                cmd.ExecuteNonQuery();

                // Step 6: Successful Message
                MessageBox.Show("Patient profile created successfully");

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            patient_first_name.Text = "";
            patient_last_name.Text = "";
            patient_year.Text = "";
            patient_phone_number.Text = "";
            patient_notes.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Dashboard dashboardd = new Dashboard();
            dashboardd.Show();
            this.Close();
        }
    }
}
