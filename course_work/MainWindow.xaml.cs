using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using System.Globalization;
using System.Drawing;
using System.Windows.Controls.Primitives;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace course_work
{

    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable roomsTable;

        public MainWindow()
        {
            InitializeComponent();  
            connectionString = ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            roomsTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                adapter = new SqlDataAdapter();

                //Добавление SELECT команды
                adapter.SelectCommand = new SqlCommand("sp_SelectRooms", connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Добавление INSERT команды
                adapter.InsertCommand = new SqlCommand("sp_InsertRooms", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NChar, 10, "room_class"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NChar, 10, "room_size"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@roomId", SqlDbType.Int, 0, "room_id");
                parameter.Direction = ParameterDirection.Output;

                //Добавление DELETE команды
                adapter.DeleteCommand = new SqlCommand("sp_DeleteRooms", connection);
                adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                adapter.DeleteCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));

                //Добавление UPDATE команды
                adapter.UpdateCommand = new SqlCommand("sp_UpdateRooms", connection);
                adapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
                adapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));
                adapter.UpdateCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                adapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NChar, 10, "room_class"));
                adapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NChar, 10, "room_size"));

                connection.Open();
                adapter.Fill(roomsTable);
                testGrid.ItemsSource = roomsTable.DefaultView;
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

        private void UpdateDB()
        {
            adapter.Update(roomsTable);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (testGrid.SelectedItems != null)
            {
                for (int i = testGrid.SelectedItems.Count-1; i >= 0; i--)
                {
                    DataRowView datarowView = testGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB();
        }


    }
}

