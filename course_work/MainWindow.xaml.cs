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

namespace course_work
{

    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter roomsAdapter;
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
                roomsAdapter = new SqlDataAdapter();

                //Добавление SELECT команды
                roomsAdapter.SelectCommand = new SqlCommand("sp_SelectRooms", connection);
                roomsAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Добавление INSERT команды
                roomsAdapter.InsertCommand = new SqlCommand("sp_InsertRooms", connection);
                roomsAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                roomsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                roomsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NChar, 10, "room_class"));
                roomsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NChar, 10, "room_size"));
                SqlParameter parameter = roomsAdapter.InsertCommand.Parameters.Add("@roomId", SqlDbType.Int, 0, "room_id");
                parameter.Direction = ParameterDirection.Output;

                //Добавление DELETE команды
                roomsAdapter.DeleteCommand = new SqlCommand("sp_DeleteRooms", connection);
                roomsAdapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                roomsAdapter.DeleteCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));

                //Добавление UPDATE команды
                roomsAdapter.UpdateCommand = new SqlCommand("sp_UpdateRooms", connection);
                roomsAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
                roomsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));
                roomsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                roomsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NChar, 10, "room_class"));
                roomsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NChar, 10, "room_size"));

                connection.Open();
                roomsAdapter.Fill(roomsTable);
                roomGrid.ItemsSource = roomsTable.DefaultView;
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

        private void UpdateDB(SqlDataAdapter adapter)
        {
            adapter.Update(roomsTable);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB(roomsAdapter);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomGrid.SelectedItems != null)
            {
                for (int i = roomGrid.SelectedItems.Count-1; i >= 0; i--)
                {
                    DataRowView datarowView = roomGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB(roomsAdapter);
        }


    }
}

