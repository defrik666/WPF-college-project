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
        SqlConnection sqlConnection = null;

        public LoginWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ConnectDB();
            DataTable dt = Select("Select * FROM rooms JOIN class ON rooms.roomClassId = class.classId JOIN size ON rooms.roomSizeId = size.sizeId");


        }


        public bool Login(User user)
        {

            return false;
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User() { Username = UsernameBox.Text, Password = PasswordBox.Text};

            if (Login(user))
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

            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            mainWindow.Show();
        }

        public void ConnectDB()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString);
            sqlConnection.Open();
        }

        public DataTable Select() // функция подключения к базе данных и обработка запросов
        {
            DataTable dataTable = new DataTable("DB");
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "Select * FROM users";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);

            return dataTable;
        }


        public class User
        {
            public string Username {  get; set; }
            public string Password { get; set; }
        }
    }
}
