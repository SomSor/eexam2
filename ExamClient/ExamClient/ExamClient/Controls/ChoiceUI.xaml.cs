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

namespace ExamClient.Controls
{
    /// <summary>
    /// Interaction logic for ChoiceUI.xaml
    /// </summary>
    public partial class ChoiceUI : UserControl
    {
        public ChoiceUI()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (sndr, se) =>
            {
                var vm = this.DataContext as ViewModel.ChoiceViewModel;
                //vm.OnClicked();
                vm.Callback(vm.Choice);
            };
        }
    }
}
