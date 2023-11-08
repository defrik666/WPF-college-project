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

namespace course_work
{
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection = null;

        public MainWindow()
        {
            InitializeComponent();

            ConnectDB();
            testGrid.ItemsSource = LoadData();

        }

        public void ConnectDB()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["HotelDB"].ConnectionString);
            sqlConnection.Open();
        }

        public DataTable Select(string selectSQL) // функция подключения к базе данных и обработка запросов
        {
            DataTable dataTable = new DataTable("DB");
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); 
            sqlDataAdapter.Fill(dataTable);    
            
            return dataTable;
        }

        public DataView LoadData()
        {
            return Select("Select * FROM Rooms").DefaultView;
        }
    }
}
