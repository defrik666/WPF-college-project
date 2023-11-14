using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.ObjectModel;

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

            //ConnectDB();
            //ObservableCollection<Rooms> roomsColl = new ObservableCollection<Rooms>();
            //LoadData(roomsColl);
            //testGrid.DataContext = roomsColl;

            //testGrid.ItemsSource = LoadData();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "Select * FROM rooms JOIN class ON rooms.roomClassId = class.classId JOIN size ON rooms.roomSizeId = size.sizeId";
            roomsTable = new DataTable();
            SqlConnection connection = null;    
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                //// установка команды на добавление для вызова хранимой процедуры
                //adapter.InsertCommand = new SqlCommand("sp_InsertRooms", connection);
                //adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                //adapter.InsertCommand.Parameters.Add(new SqlParameter("@roomClass", SqlDbType.NVarChar, 10, "Class"));
                //adapter.InsertCommand.Parameters.Add(new SqlParameter("@roomSize", SqlDbType.NVarChar, 10, "Size"));
                //adapter.InsertCommand.Parameters.Add(new SqlParameter("@photo", SqlDbType.Image, 0, "photo"));
                //SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@roomId", SqlDbType.Int, 0, "roomId");
                //parameter.Direction = ParameterDirection.Output;

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
                MessageBox.Show("aaaaaa");
                if (connection != null)
                    connection.Close();
            }
        }

        //public void ConnectDB()
        //{
        //    sqlConnection.Open();
        //}

        //public DataTable Select(string selectSQL) // функция подключения к базе данных и обработка запросов
        //{
        //    DataTable dataTable = new DataTable("DB");
        //    SqlCommand sqlCommand = sqlConnection.CreateCommand();
        //    sqlCommand.CommandText = selectSQL;
        //    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
        //    sqlDataAdapter.Fill(dataTable);

        //    return dataTable;
        //}

        //public DataView LoadData()
        //{
        //    return Select("Select * FROM rooms JOIN class ON rooms.roomClassId = class.classId JOIN size ON rooms.roomSizeId = size.sizeId").DefaultView;
        //}

        //private void LoadData(ObservableCollection<Rooms> roomsColl)
        //{
        //    DataTable dt = Select("Select * FROM rooms JOIN class ON rooms.roomClassId = class.classId JOIN size ON rooms.roomSizeId = size.sizeId");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        //MessageBox.Show(EngToRus((string)dr["roomClass"]));

        //        Rooms rooms = new Rooms() {roomId = (int)dr["roomId"], Class = EngToRus((string)dr["roomClass"]), Size = (string)dr["roomSize"], Photo = ConvertByteImage((byte[])dr["photo"]) };

        //        roomsColl.Add(rooms);
        //    }


        //}

        //public BitmapImage ConvertByteImage(byte[] imageByteArray)
        //{
        //    BitmapImage img = new BitmapImage();
        //    using (MemoryStream memStream = new MemoryStream(imageByteArray))
        //    {
        //        img.BeginInit();
        //        img.CacheOption = BitmapCacheOption.OnLoad;
        //        img.StreamSource = memStream;
        //        img.EndInit();
        //        img.Freeze();
        //    }
        //    return img;
        //}

        //public string EngToRus(string str)
        //{
        //    switch (str.Trim())
        //    {
        //        case "standart":
        //            return "Стандартный";
        //        case "superior":
        //            return "Улучшеный";
        //        //case '':
        //        //    return null;
        //        //case "standart":
        //        //    return "Стандартный";
        //        //case "superior":
        //        //    return "Улучшеный";
        //        //case apartment:
        //        //    return null;
        //        default:
        //            return str;
        //    }
        //}

        //public class Rooms
        //{
        //    public int roomId {  get; set; }
        //    public string Class { get; set; }
        //    public string Size { get; set; }
        //    public BitmapImage Photo { get; set; }
        //}
    }
}
