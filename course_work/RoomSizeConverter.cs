using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace course_work
{
    public class RoomSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return DbToDt((string)value);
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

        private string DbToDt(string str)
        {
            switch (str.Trim())
            {
                case "single":
                    return "Одноместный";
                case "double":
                    return "Двухместный с одной кроватью";
                case "twin":
                   return "Двухместный с двумя кроватями";
                default:
                    return str;
            }
        }

        private string DtToDb(string str) 
        {
            switch (str.Trim().ToLower()) 
            {
                case "одноместный":
                    return "single";
                case "сингл":
                    return "single";
                case "sgl":
                    return "single";
                case "двухместный с одной кроватью":
                    return "double";
                case "дабл":
                    return "double";
                case "dbl":
                    return "double";
                case "твин":
                    return "twin";
                case "двухместный с двумя кроватями":
                    return "twin";
                default:
                    return str.Trim().ToLower();
            }
        }
    }
}
