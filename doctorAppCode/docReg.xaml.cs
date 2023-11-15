using System;
using Npgsql;
using BCrypt;
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
    /// Interaction logic for docReg.xaml
    /// </summary>
    public partial class docReg : Window
    {
        public docReg()
        {
            InitializeComponent();
        }

        //Connections

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


        //After having the info we need we establish the connection
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


        //Add values to the database (doctor regist)

        private void submit_Doctor_Reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Step 1: Establish connection
                establishConnect();

                // Step 2: Open the connection
                con.Open();

                // Step 3: Query Generation
                string Query = "INSERT INTO doctors (doctor_first_name, doctor_last_name, doctor_email, doctor_address, doctor_phone_number, doctor_specialization, doctor_medical_id, doctor_notes, doctor_username, doctor_password) VALUES (@doctor_first_name, @doctor_last_name, @doctor_email, @doctor_address, @doctor_phone_number, @doctor_specialization, @doctor_medical_id, @doctor_notes, @doctor_username, @doctor_password)";

                // Step 4: Initialize the command adapter of the database
                cmd = new NpgsqlCommand(Query, con);

                string plainPassword = doctor_password.Password;

                // Step 4.1: Initialize the parameters in the variables of the command
                cmd.Parameters.AddWithValue("@doctor_first_name", doctor_first_name.Text);
                cmd.Parameters.AddWithValue("@doctor_last_name", doctor_last_name.Text);
                cmd.Parameters.AddWithValue("@doctor_email", doctor_email.Text);
                cmd.Parameters.AddWithValue("@doctor_address", doctor_address.Text);
                cmd.Parameters.AddWithValue("@doctor_phone_number", long.Parse(doctor_phone_number.Text));
                cmd.Parameters.AddWithValue("@doctor_specialization", doctor_specialization.Text);
                cmd.Parameters.AddWithValue("@doctor_medical_id", int.Parse(doctor_medical_id.Text));
                cmd.Parameters.AddWithValue("@doctor_notes", doctor_notes.Text);
                cmd.Parameters.AddWithValue("@doctor_username", doctor_username.Text);
                cmd.Parameters.AddWithValue("@doctor_password", plainPassword);

                // Step 5: Execute the query    
                cmd.ExecuteNonQuery();

                // Step 6: Successful Message
                MessageBox.Show("Doctor profile created successfully");

                // Step 7: Close the connection
                con.Close();

                // Go back to main window
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reset_Doctor_Click(object sender, RoutedEventArgs e)
        {

            doctor_first_name.Text = "";
            doctor_last_name.Text = "";
            doctor_email.Text = "";
            doctor_address.Text = "";
            doctor_phone_number.Text = "";
            doctor_specialization.Text = "";
            doctor_medical_id.Text = "";
            doctor_notes.Text = "";
            doctor_username.Text = "";
            doctor_password.Password = "";

            doctor_first_name.Focus();

        }

        private void cancel_doctor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainw = new MainWindow();
            mainw.Show();
            this.Close();
        }
    }
}
