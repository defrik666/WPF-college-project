using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace course_work
{
    internal class RoomPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return DbToDt((int)value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return DtToDb((string)value);
            }
            return value;
        }

        private string DbToDt(int num)
        {
            return num.ToString() + " ₽";
        }

        private int DtToDb(string str)
        {
            int num = Int32.Parse(str.Trim('₽').Trim());
            return num;
        }
    }
}
