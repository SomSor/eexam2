using ExamClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ExamClient.Converters
{
    public class AnswerColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                ChoiceSelector choice = (ChoiceSelector)value;

                if (TestingData.State != "CheckAnswer")
                {
                    if (choice.ChoiceNo == 0)
                    {
                        var onAns = Colors.Black;
                        onAns.R = (byte)68;
                        onAns.G = (byte)68;
                        onAns.B = (byte)68;
                        return new SolidColorBrush(onAns);
                    }
                    else
                    {
                        var Ans = Colors.Black;
                        Ans.R = (byte)44;
                        Ans.G = (byte)84;
                        Ans.B = (byte)234;
                        return new SolidColorBrush(Ans);
                    }
                }
                else
                {
                    if (choice.IsCorrect.HasValue && choice.IsCorrect.Value)
                    {
                        var Correct = Colors.Black;
                        Correct.R = (byte)51;
                        Correct.G = (byte)205;
                        Correct.B = (byte)95;
                        return new SolidColorBrush(Correct);
                    }
                    else
                    {
                        var InCorrect = Colors.Black;
                        InCorrect.R = (byte)239;
                        InCorrect.G = (byte)71;
                        InCorrect.B = (byte)58;
                        return new SolidColorBrush(InCorrect);
                    }                  
                }
            }
            else
            {
                var onAns = Colors.Black;
                onAns.R = (byte)68;
                onAns.G = (byte)68;
                onAns.B = (byte)68;
                return new SolidColorBrush(onAns);
            }




        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
