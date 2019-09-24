using System;
using System.Collections.Generic;
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

namespace ExamClient.Views
{
    /// <summary>
    /// Interaction logic for ExamSumaryPage.xaml
    /// </summary>
    public partial class ExamSumaryPage : Page
    {
        public ExamSumaryPage()
        {
            InitializeComponent();
        }

        private void backToExam(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ExamPage.xaml", UriKind.RelativeOrAbsolute));
        }
  
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.RelativeOrAbsolute));

        }
    }
}
