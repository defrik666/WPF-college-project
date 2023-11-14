using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace course_work
{
    public class RoomClassConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return EngToRus((string)value);
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string EngToRus(string str)
        {
            switch (str.Trim())
            {
                case "standart":
                    return "Стандартный";
                case "superior":
                    return "Улучшеный";
                case "apartment":
                   return "Апартаменты";
                case "studio":
                    return "Студия";
                case "suit":
                    return "Презедентский";
                default:
                    return str;
            }
        }
    }
}
