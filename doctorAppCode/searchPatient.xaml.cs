using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DoctorSoftwareApp
{
    /// <summary>
    /// Interaction logic for searchPatient.xaml
    /// </summary>
    public partial class searchPatient : Window
    {
        public searchPatient()
        {
            InitializeComponent();
        }

        //Connections

        public static NpgsqlConnection con;
        public static NpgsqlCommand cmd;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Step 1 - Establish connection
                establishConnect();

                //Step 2 - Open connection
                con.Open();

                //Step 3 - Create query
                string Query = "select * from patients where patient_id=@Id";

                //Step 4 - Create command
                cmd = new NpgsqlCommand(Query, con);

                //Step 4.2 - Initialize the query variable
                cmd.Parameters.AddWithValue("@Id", int.Parse(searchBar.Text));

                //Step 4.3 - Add a checker/boolean to see if the data is present or not
                bool noData = true;

                //Step 5 - Data Reader adapter
                NpgsqlDataReader dr = cmd.ExecuteReader(); //this line is going to read all the data matches with the query and return them

                //Step 6 - Checking all the info that was grabbed from the database, one by one
                while (dr.Read())
                {
                    noData = false;
                    search_first_name.Text = dr["patient_first_name"].ToString();
                    search_last_name.Text = dr["patient_last_name"].ToString();
                    search_patient_birth.Text = dr["patient_year"].ToString();
                    search_patient_contact.Text = dr["patient_phone_number"].ToString();
                    search_patient_notes.Text = dr["patient_notes"].ToString();
                }
                if (noData)
                {
                    MessageBox.Show("The id you entered is not valid");
                }


                //Step 7 - Close the connection
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            searchBar.Text = "";
            search_first_name.Text = "";
            search_last_name.Text = "";
            search_patient_birth.Text = "";
            search_patient_contact.Text = "";
            search_patient_notes.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }
    }
}
