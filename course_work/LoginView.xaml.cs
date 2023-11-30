using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace course_work
{
    public partial class LoginView : Window
    {
        readonly string connectionString;
        SqlDataAdapter adapter;
        string privilege;
        int user_id;

        public LoginView()
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
            DataTable dataTable = new DataTable();
            adapter.SelectCommand.Parameters["@username"].Value = username;
            adapter.Fill(dataTable);
            if (dataTable.Rows.Count == 0)
            {
                return false;
            }
            if (dataTable.Rows[0]["password"].ToString().Trim() == password.Trim())
            {
                privilege = dataTable.Rows[0]["privilege"].ToString().Trim();
                user_id = (int)dataTable.Rows[0]["user_id"];
                return true;
            }

            return false;
        }

        public void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.user_id = user_id;
            mainWindow.privilege = privilege;
            mainWindow.Show();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Login(txtUser.Text, txtPassword.Password))
            {
                OpenMainWindow();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
