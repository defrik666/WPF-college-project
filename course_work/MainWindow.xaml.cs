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
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace course_work
{

    public partial class MainWindow : Window
    {
        public int user_id { get; set; }
        public string privilege {  get; set; }

        readonly string connectionString;
        SqlDataAdapter roomsAdapter;
        SqlDataAdapter reservationsAdapter;
        SqlDataAdapter datePickerAdapter;
        SqlDataAdapter reservationsAddAdapter;
        SqlDataAdapter currentAdappter;
        DataTable dataTable;
        DataGrid currentDataGrid;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString;
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

        private void CreateRoomsAdapter(SqlConnection connection)
        {
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
            roomsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Int, 0, "room_price"));
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
            roomsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Int, 0, "room_price"));
        }

        private void CreateReservationsAdapter(SqlConnection connection)
        {
            reservationsAdapter = new SqlDataAdapter();

            if (privilege == "admin")
            {
                //Добавление SELECT команды
                reservationsAdapter.SelectCommand = new SqlCommand("sp_SelectReservations", connection);
                reservationsAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                ////Добавление INSERT команды
                //reservationsAdapter.InsertCommand = new SqlCommand("sp_InsertRooms", connection);
                //reservationsAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                //reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                //reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NChar, 10, "room_class"));
                //reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NChar, 10, "room_size"));
                //SqlParameter parameter = reservationsAdapter.InsertCommand.Parameters.Add("@roomId", SqlDbType.Int, 0, "room_id");
                //parameter.Direction = ParameterDirection.Output;

                ////Добавление DELETE команды
                //reservationsAdapter.DeleteCommand = new SqlCommand("sp_DeleteRooms", connection);
                //reservationsAdapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                //reservationsAdapter.DeleteCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));

                ////Добавление UPDATE команды
                //reservationsAdapter.UpdateCommand = new SqlCommand("sp_UpdateRooms", connection);
                //reservationsAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
                //reservationsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));
                //reservationsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                //reservationsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NChar, 10, "room_class"));
                //reservationsAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NChar, 10, "room_size"));
            }
            else
            {
                //Добавление SELECT команды
                reservationsAdapter.SelectCommand = new SqlCommand("sp_SelectReservationsUser", connection);
                reservationsAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                reservationsAdapter.SelectCommand.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int, 0, "user_id"));
                reservationsAdapter.SelectCommand.Parameters["@userId"].Value = user_id;


                //Добавление DELETE команды
                reservationsAdapter.DeleteCommand = new SqlCommand("sp_DeleteReservationsUser", connection);
                reservationsAdapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                reservationsAdapter.DeleteCommand.Parameters.Add(new SqlParameter("@reservationId", SqlDbType.Int, 0, "reservation_id"));

                //Добавление INSERT команды
                reservationsAdapter.InsertCommand = new SqlCommand("sp_InsertReservationsUser", connection);
                reservationsAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int, 0, "user_id"));
                reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));
                reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@startDate", SqlDbType.Date, 0, "start_date"));
                reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@endDate", SqlDbType.Date, 0, "end_date"));
                reservationsAdapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Decimal, 0, "price"));
                SqlParameter par1 = reservationsAdapter.InsertCommand.Parameters.Add("@reservationId", SqlDbType.Int, 0, "reservation_id");
                par1.Direction = ParameterDirection.Output;
                SqlParameter par2 = reservationsAdapter.InsertCommand.Parameters.Add("@total", SqlDbType.Int, 0, "total");
                par2.Direction = ParameterDirection.Output;

            }
        }

        private void CreateReservationsAddAdapter(SqlConnection connection)
        {
            reservationsAddAdapter = new SqlDataAdapter();

            //Добавление SELECT команды
            reservationsAddAdapter.SelectCommand = new SqlCommand("sp_SelectReservations", connection);
            reservationsAddAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            //Добавление INSERT команды
            reservationsAddAdapter.InsertCommand = new SqlCommand("sp_InsertReservations", connection);
            reservationsAddAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            reservationsAddAdapter.InsertCommand.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int, 0, "user_id"));
            reservationsAddAdapter.InsertCommand.Parameters.Add(new SqlParameter("@roomId", SqlDbType.Int, 0, "room_id"));
            reservationsAddAdapter.InsertCommand.Parameters.Add(new SqlParameter("@startDate", SqlDbType.Date, 0, "start_date"));
            reservationsAddAdapter.InsertCommand.Parameters.Add(new SqlParameter("@endDate", SqlDbType.Date, 0, "end_date"));
            reservationsAddAdapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Decimal, 0, "price"));
            SqlParameter par1 = reservationsAddAdapter.InsertCommand.Parameters.Add("@reservationId", SqlDbType.Int, 0, "reservation_id");
            par1.Direction = ParameterDirection.Output;
            SqlParameter par2 = reservationsAddAdapter.InsertCommand.Parameters.Add("@total", SqlDbType.Int, 0, "total");
            par2.Direction = ParameterDirection.Output;
        }

        private void CreateDatePickerAdapter(SqlConnection connection)
        {
            datePickerAdapter = new SqlDataAdapter();

            //Добавление SELECT команды
            datePickerAdapter.SelectCommand = new SqlCommand("sp_SelectReservationsDateRange", connection);
            datePickerAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            datePickerAdapter.SelectCommand.Parameters.Add(new SqlParameter("@room_id", SqlDbType.Int, 0, "room_id"));
        }

        private void UpdateDB(SqlDataAdapter adapter)
        {
            adapter.Update(dataTable);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB(currentAdappter);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDataGrid.SelectedItems != null)
            {
                for (int i = currentDataGrid.SelectedItems.Count-1; i >= 0; i--)
                {
                    DataRowView datarowView = (DataRowView)currentDataGrid.SelectedItems[i];
                    if (datarowView != null)
                    {
                        DataRow dataRow = datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB(currentAdappter);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {

                TabItem ti = tabControl.SelectedItem as TabItem;
                SqlConnection connection = new SqlConnection(connectionString);
                switch (ti.Header.ToString()) 
                {
                    case "Комнаты":
                        if (roomsAdapter == null) CreateRoomsAdapter(connection);

                        ChangeTab(roomsAdapter, roomGrid, connection);
                        break;
                    case "Бронь":
                        if (reservationsAdapter == null) CreateReservationsAdapter(connection);

                        ChangeTab(reservationsAdapter, reservationsGrid, connection);
                        break;
                    default:
                        MessageBox.Show("Произошла непредвиденная ошибка");
                        break;
                }
                connection?.Close();
            }
        }

        private void ChangeTab(SqlDataAdapter adapter, DataGrid dataGrid, SqlConnection connection)
        {
            dataTable = new DataTable();
            try
            {
                connection.Open();
                adapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
            currentDataGrid = dataGrid;
            currentAdappter = adapter;

            StackPanel_Reservation.Visibility = Visibility.Collapsed;
            BtnCancel.Visibility = Visibility.Collapsed;
        }

        private void DatePicker_CalendarOpened(object sender, EventArgs e)
        {
            
            dataTable = new DataTable();

            if(datePickerAdapter == null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                CreateDatePickerAdapter(connection);
            }

            if(currentDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожайлуста, выберите комнату");
                return;
            }

            DataRowView dataRowView = currentDataGrid.SelectedItem as DataRowView;
            if(dataRowView == null)
            {
                return;
            }
            int room_id = (int)dataRowView.Row["room_id"];

            datePickerAdapter.SelectCommand.Parameters["@room_id"].Value = room_id;
            datePickerAdapter.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                DateTime start = (DateTime)row["start_date"];
                DateTime end = (DateTime)row["end_date"];
                DatePicker_startDate.BlackoutDates.Add(new CalendarDateRange(start, end));
                DatePicker_endDate.BlackoutDates.Add(new CalendarDateRange(start, end));
            }
        }

        private void RoomGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (roomGrid.SelectedItem == null)
            {
                StackPanel_Reservation.Visibility = Visibility.Collapsed;
            }
            else if(roomGrid.SelectedItem as DataRowView == null)
            {
                StackPanel_Reservation.Visibility = Visibility.Collapsed;
            }
            else
            {
                StackPanel_Reservation.Visibility = Visibility.Visible;
            }
 

            DatePicker_startDate.BlackoutDates.Clear();
            DatePicker_startDate.SelectedDate = null;
            DatePicker_endDate.BlackoutDates.Clear();
            DatePicker_endDate.SelectedDate = null;
        }

        private void ReservationsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reservationsGrid.SelectedItem == null)
            {
                BtnCancel.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnCancel.Visibility = Visibility.Visible;
            }


        }

        private void DatePicker_startDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker_endDate.DisplayDateStart = DatePicker_startDate.SelectedDate;

            if(DatePicker_endDate.SelectedDate == null)
            {
                BtnReservation.Visibility = Visibility.Hidden;
            }
            else
            {
                BtnReservation.Visibility = Visibility.Visible;
            }
        }

        private void DatePicker_endDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker_startDate.DisplayDateEnd = DatePicker_endDate.SelectedDate;

            if (DatePicker_startDate.SelectedDate == null)
            {
                BtnReservation.Visibility = Visibility.Hidden;
            }
            else
            {
                BtnReservation.Visibility = Visibility.Visible;
            }
        }

        private void BtnReservation_Click(object sender, RoutedEventArgs e)
        {
            if(reservationsAddAdapter == null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                CreateReservationsAddAdapter(connection);
            }

            DataTable table = new DataTable();
            reservationsAddAdapter.Fill(table);

            DataRowView row = roomGrid.SelectedItem as DataRowView;
            int room_id = (int)row.Row["room_id"];
            int price = (int)row.Row["room_price"];
            DateTime start_date = (DateTime)DatePicker_startDate.SelectedDate;
            DateTime end_date = (DateTime)DatePicker_endDate.SelectedDate;

            table.Rows.Add(0, user_id, room_id,start_date,end_date,price,0);
            reservationsAddAdapter.Update(table);
        }
    }
}

