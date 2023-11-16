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

        public string DbToDt(string str)
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
                case "suite":
                    return "Презедентский";
                default:
                    return str;
            }
        }

        public string DtToDb(string str) 
        {
            switch (str.Trim().ToLower()) 
            {
                case "стандартный":
                    return "standart";
                case "стандарт":
                    return "standart";
                case "std":
                    return "standart";
                case "стд":
                    return "standart";
                case "sup":
                    return "superior";
                case "улучшенный":
                    return "superior";
                case "ул":
                    return "superior";
                case "суп":
                    return "superior";
                case "апартаменты":
                    return "apartment";
                case "apt":
                    return "apartment";
                case "ап":
                    return "apartment";
                case "студия":
                    return "studio";
                case "студ":
                    return "studio";
                case "stu":
                    return "studio";
                case "люкс":
                    return "suite";
                case "su":
                    return "suite";
                default:
                    return str.Trim().ToLower();
            }
        }
    }
}
