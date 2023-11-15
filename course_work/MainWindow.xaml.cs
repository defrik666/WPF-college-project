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
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            connectionString = ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string sql = "Select * FROM rooms JOIN class ON rooms.room_class_id = class.class_id JOIN size ON rooms.room_size_id = size.size_id";
            string sql = "SELECT photo, (SELECT room_class FROM class WHERE room_class_id = class_id) as room_class,(SELECT room_size FROM size WHERE room_size_id = size_id) as room_size FROM rooms";
        roomsTable = new DataTable();
        SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand(sql, connection);
        adapter = new SqlDataAdapter(command);

        // установка команды на добавление для вызова хранимой процедуры
                adapter.InsertCommand = new SqlCommand("sp_InsertRooms", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NChar, 10, "room_class"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NChar, 10, "room_size"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@roomId", SqlDbType.Int, 0, "roomId");
                parameter.Direction = ParameterDirection.Output;

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
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            MessageBox.Show(adapter.Update(roomsTable).ToString());
            //adapter.Update(roomsTable);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (testGrid.SelectedItems != null)
            {
                for (int i = 0; i < testGrid.SelectedItems.Count; i++)
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

