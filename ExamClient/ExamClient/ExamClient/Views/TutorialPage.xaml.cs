using ExamClient.ViewModel;
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
    /// Interaction logic for TutorialPage.xaml
    /// </summary>
    public partial class TutorialPage : Page
    {
        public TutorialViewModel ViewModel
        {
            get
            {
                return this.DataContext as ViewModel.TutorialViewModel;
            }
        }

        public TutorialPage()
        {
            InitializeComponent();
            alertDlg.Visibility = Visibility.Collapsed;
        }

        private void start(object sender, RoutedEventArgs e)
        {
            Services.OnsiteServices svc = new Services.OnsiteServices();

            var activeData = svc.CheckActive();

            if (activeData.IsActive)
            {
                Action navigateMethod;
                navigateMethod = NavigateToExamPage;
                ViewModel.startQuiz(navigateMethod);
            }
            else
            {
                alertDlg.Visibility = Visibility.Visible;
            }

          
        }

        public void NavigateToExamPage()
        {
            NavigationService.Navigate(new Uri("/Views/ExamPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void closealert(object sender, RoutedEventArgs e)
        {
            alertDlg.Visibility = Visibility.Collapsed;
        }
    }
}
