using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void medicalRecordButton_Click(object sender, RoutedEventArgs e)
        {
            patientRegistration newPatient = new patientRegistration();
            newPatient.Show();
            this.Close();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

                //In this button we will read all the database entries in this method. 

                //Step 1 - Establish connection
                establishConnect();

                //Step 2 - Open connection
                con.Open();

                //Step 3 - Create query
                string Query = "select * from patients";

                //Step 4 - Create command
                cmd = new NpgsqlCommand(Query, con);

                //Step 5 - We need to create a SQL dataAdapter and a SQL datatable, read the data, execute it, the info needs to
                //go to the datatable and then push it to the dataAdapter to set it back to DataGrid

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Now we send the datatable information to dataGrid itemsource

                dataGrid.ItemsSource = dt.AsDataView();//making sure dataGrid is getting the full table data

                //Step 7 - Reinitialize our wpf controls data, for dataGrid
                DataContext = da;

                //Step 8 - Close connection
                con.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            searchPatient searchPatient = new searchPatient();
            searchPatient.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainBack = new MainWindow();
            mainBack.Show();
            this.Close();
        }

        private void AppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            appointment appointmentPage = new appointment();
            appointmentPage.Show();
            this.Close();
        }
    }
}
