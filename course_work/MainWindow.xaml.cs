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
<<<<<<< HEAD

=======
using System.Collections.ObjectModel;
>>>>>>> c073ca36bcfd677c567f551a81f3e870c9d0732c

namespace course_work
{
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection = null;

        public MainWindow()
        {
            InitializeComponent();

            //ConnectDB();

            ObservableCollection<Rooms> roomsColl = new ObservableCollection<Rooms>();

            LoadData(roomsColl);

            testGrid.DataContext = roomsColl;

            //testGrid.ItemsSource = LoadData();

        }

        //public void ConnectDB()
        //{
        //    sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString);
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

<<<<<<< HEAD
        //private void Test(Rooms room)
        //{
=======
        private void LoadData(ObservableCollection<Rooms> roomsColl)
        {
            DataTable dt = Select("Select * FROM rooms JOIN class ON rooms.roomClassId = class.classId JOIN size ON rooms.roomSizeId = size.sizeId");
            foreach (DataRow dr in dt.Rows)
            {
                MessageBox.Show(EngToRus((string)dr["roomClass"]));

                Rooms rooms = new Rooms() {roomId = (int)dr["roomId"], Class = EngToRus((string)dr["roomClass"]), Size = (string)dr["roomSize"], Photo = ConvertByteImage((byte[])dr["photo"]) };

                roomsColl.Add(rooms);
            }
>>>>>>> c073ca36bcfd677c567f551a81f3e870c9d0732c

        //}

        public BitmapImage ConvertByteImage(byte[] imageByteArray)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(imageByteArray))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = memStream;
                img.EndInit();
                img.Freeze();
            }
            return img;
        }

        public string EngToRus(string str)
        {
            switch (str.Trim())
            {
                case "standart":
                    return "Стандартный";
                case "superior":
                    return "Улучшеный";
                //case '':
                //    return null;
                //case "standart":
                //    return "Стандартный";
                //case "superior":
                //    return "Улучшеный";
                //case apartment:
                //    return null;
                default:
                    return str;
            }
        }

        public class Rooms
        {
            public int roomId {  get; set; }
            public string Class { get; set; }
            public string Size { get; set; }
            public BitmapImage Photo { get; set; }
        }
    }
}
