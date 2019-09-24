using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ExamClient.Converters
{
    public class ChoiceCorrectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value != null) bool.TryParse(value.ToString(), out result);

            Visibility correct = new Visibility();

            if (result)
            {
                correct = Visibility.Visible;
            }

            else
            {
                correct = Visibility.Collapsed;
            }

            return correct;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
