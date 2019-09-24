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
    /// Interaction logic for ExamPage.xaml
    /// </summary>
    public partial class ExamPage : Page
    {
        public ExamPage()
        {
            InitializeComponent();

            lbMiniAns.MouseLeftButtonDown += (sndr, se) =>
            {
                var vm = this.DataContext as ViewModel.ExamViewModel;
                vm.OpenAnsDlg();
            };

        }

        private void sendResult(object sender, RoutedEventArgs e)
        {
            NavigateToResultPage();
        }

        private void NavigateToResultPage()
        {
            NavigationService.Navigate(new Uri("/Views/ExamSumaryPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Models.TestingData.State == "CheckAnswer")
            {
                NavigationService.Navigate(new Uri("/Views/ExamSumaryPage.xaml", UriKind.RelativeOrAbsolute));

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/TutorialPage.xaml", UriKind.RelativeOrAbsolute));

        }
    }
}
