using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ExamClient.Converters
{
    public class ChoiceSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false;
            if (value != null) bool.TryParse(value.ToString(), out result);

            Visibility seleted = new Visibility();

            //if (result && StudentQuiz.State == "CheckAnswer")
            if (result)
            {
                seleted = Visibility.Visible;
            }

            else
            {
                seleted = Visibility.Collapsed;
            }

            return seleted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
