﻿using System;
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
    public partial class OldLoginWindow : Window
    {
        readonly string connectionString;
        SqlDataAdapter adapter;
        string privilege;
        int user_id;

        public OldLoginWindow()
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
                MessageBox.Show(privilege);
                MessageBox.Show(user_id.ToString());

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
            mainWindow.user_id = user_id;
            mainWindow.privilege = privilege;
            mainWindow.Show();
        }
    }
}
