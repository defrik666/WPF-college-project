using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security;
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

namespace course_work
{
    public partial class RegestrationView : Window
    {
        readonly UserModel User;
        readonly string connectionString;

        public RegestrationView()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["HotelDB1"].ConnectionString;
            User = new UserModel();
            this.DataContext = User;
        }

        public bool CheckUser(SqlConnection connection)
        {
            bool status = false;

            SqlCommand cmd = new SqlCommand("sp_SelectUsersRegestration", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter username = new SqlParameter("@username", SqlDbType.VarChar, 20, "Username");
            username.Value = User.Username;
            cmd.Parameters.Add(username);
            try
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
            }
            return status;
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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        public void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.user_id = User.User_id;
            mainWindow.privilege = "user";
            mainWindow.Show();
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                SqlConnection connection = new SqlConnection(connectionString);
                if (!CheckUser(connection))
                {
                    User.Insert(connection);
                    MessageBox.Show("Регистрация прошла успешно!");
                    OpenMainWindow();
                    return;
                }
                MessageBox.Show("Имя пользователя уже используется");
            }
        }

        private bool Validate()
        {
            string txt = "";
            if (User.Username == null) txt += "Имя пользователя не может быть пустым \n";
            else if (User.Username.Length <= 3) txt += "Имя пользователя не может быть меньше 4 символов \n";
            if (User.Password == null) txt += "Пароль не может быть пустым \n";
            else if (User.Password.Length <= 3) txt += "Пароль не может быть меньше 4 символов \n";
            if (User.Name == null) txt += "Имя не может быть пустым \n";
            else if (!Regex.IsMatch(User.Name, @"[а-яА-Я]{1,20}")) txt = "Имя должно содержать только кириллические символы \n";
            if (User.Surname == null) txt += "Фамиля не может быть пустой \n";
            else if (!Regex.IsMatch(User.Surname, @"[а-яА-Я]{1,20}")) txt = "Фамилия должна содержать только кириллические символы \n";
            if (User.Email == null) txt += "Почта не может быть пустой \n";
            else if (!Regex.IsMatch(User.Email, @"[A-Za-z0-9]{1,20}((\@gmail\.com)|(\@yandex\.ru)|(\@mail\.ru))"))
            {
                if (!Regex.IsMatch(User.Email, @"[A-Za-z0-9]{1,20}\@.{1,}")) txt += "Имя пользователя должно состоять из латинских букв и цифр \n";
                if (!Regex.IsMatch(User.Email, @"((\@gmail\.com)|(\@yandex\.ru)|(\@mail\.ru))")) txt += "Принимаемы домены почты: gmail.com,mail.ru,yandex.ru \n";
            };
            if (User.Phone == null) txt += "Телефон не может быть пустой \n";
            else if (!Regex.IsMatch(User.Phone, @"7.{1,}")) txt += "Для регистрации необходимо ввести русский номер телефона начинающийся с 7 \n";
            else if (!Regex.IsMatch(User.Phone, @"7\d{10}")) txt += "Телефон должен быть написан в форме {7**********} \n";

            if (txt != "")
            {
                MessageBox.Show(txt);
                return false;
            } 

            return true;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null) ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
        }

        public class UserModel
        {
            public string Username { get; set; }
            public SecureString Password { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public int User_id {  get; set; }

            public void Insert(SqlConnection connection)
            {
                
                SqlCommand cmd = new SqlCommand("sp_InsertUsersRegestration", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NChar, 20));
                cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.NChar, 20));
                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NChar, 50));
                cmd.Parameters.Add(new SqlParameter("@surname", SqlDbType.NChar, 50));
                cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NChar, 50));
                cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 12));
                cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int, 0));
                cmd.Parameters["@userId"].Direction = ParameterDirection.Output;
                cmd.Parameters["@username"].Value = Username;
                cmd.Parameters["@password"].Value = new System.Net.NetworkCredential(string.Empty, Password).Password;
                cmd.Parameters["@name"].Value = Name;
                cmd.Parameters["@surname"].Value = Surname;
                cmd.Parameters["@email"].Value = Email;
                cmd.Parameters["@phone"].Value = Phone;
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
    }
}
