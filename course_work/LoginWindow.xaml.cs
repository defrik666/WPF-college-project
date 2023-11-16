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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Data;
using System.Data.SqlClient;

namespace course_work
{
    public partial class LoginWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        string privilege;

        public LoginWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString;
        }
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("sp_SelectUsers", connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@username", SqlDbType.NChar, 20));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        public bool Login(string username, string password)
        {
            DataSet dataSet = new DataSet();
            adapter.SelectCommand.Parameters["@username"].Value = username;
            adapter.Fill(dataSet);
            if (dataSet.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            if (dataSet.Tables[0].Rows[0]["password"].ToString().Trim() == password.Trim())
            {
                privilege = dataSet.Tables[0].Rows[0]["privilege"].ToString().Trim();
                return true;
            }

            return false;
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {


            if (Login(UsernameBox.Text, PasswordBox.Text))
            {
                OpenMainWindow();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        
        public void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            if(privilege == "user")
            {

            }
            else if(privilege == "admin")
            {

            }
            mainWindow.Show();
        }

    }
}
