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


namespace course_work
{
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection = null;

        public MainWindow()
        {
            InitializeComponent();

            //ConnectDB();

            //List<Rooms> rooms = new List<Rooms>();
            //DataTable dt = Select("Select * FROM rooms JOIN class ON rooms.roomClassId = class.classId JOIN size ON rooms.roomSizeId = size.sizeId");
            //foreach (DataRow dr in dt.Rows)
            //{
            //    Rooms rooms1 = new Rooms() {Class = dr["roomClass"].ToString(), Size = dr["roomSize"].ToString(), };

            //    MessageBox.Show(dr["photo"].ToString());
            //}


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

        //private void Test(Rooms room)
        //{

        //}

        //public Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image image = new Image.
        //    return  
        //}

        //public Image ByteToImage(byte[] value)
        //{
        //    if (value != null)
        //    {
        //        using (MemoryStream mStream = new MemoryStream(value)) { return Image.FromStream(mStream); }
        //    }
        //    else { return null; }
        //}

        public class Rooms
        {
            public string Class { get; set; }
            public string Size { get; set; }
            public Image Photo { get; set; }
        }
    }
}
