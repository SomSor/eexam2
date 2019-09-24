using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.ViewModel
{
    public class LoginViewModel : MVVMBase
    {
        public LoginViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
            }
        }
        public void SetLanguageTH()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("th-TH");
        }
        public void SetLanguageEN()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

    }
}
