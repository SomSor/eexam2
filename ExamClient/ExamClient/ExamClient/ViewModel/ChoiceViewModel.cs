using ExamClient.Models;
using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExamClient.ViewModel
{
  
    public class ChoiceViewModel : MVVMBase
    {
        #region Property Choice
        private ChoiceSelector _choice;
        public ChoiceSelector Choice
        {
            get { return _choice; }
            set
            {
                if (_choice != value)
                {
                    _choice = value;
                    OnPropertyChanged(() => Choice);
                }
            }
        }
        #endregion
        #region Property FirstPic
        private Visibility _firstPic;
        public Visibility FirstPic
        {
            get { return _firstPic; }
            set
            {
                if (_firstPic != value)
                {
                    _firstPic = value;
                    OnPropertyChanged(() => FirstPic);
                }
            }
        }
        #endregion
        #region Property SecondPic
        private Visibility _secondPic;
        public Visibility SecondPic
        {
            get { return _secondPic; }
            set
            {
                if (_secondPic != value)
                {
                    _secondPic = value;
                    OnPropertyChanged(() => SecondPic);
                }
            }
        }
        #endregion      
        #region Property ThirdPic
        private Visibility _thirdPic;
        public Visibility ThirdPic
        {
            get { return _thirdPic; }
            set
            {
                if (_thirdPic != value)
                {
                    _thirdPic = value;
                    OnPropertyChanged(() => ThirdPic);
                }
            }
        }
        #endregion
        #region Property FourthPic
        private Visibility _fourthPic;
        public Visibility FourthPic
        {
            get { return _fourthPic; }
            set
            {
                if (_fourthPic != value)
                {
                    _fourthPic = value;
                    OnPropertyChanged(() => FourthPic);
                }
            }
        }
        #endregion
        #region Property FifthPic
        private Visibility _fifthPic;
        public Visibility FifthPic
        {
            get { return _fifthPic; }
            set
            {
                if (_fifthPic != value)
                {
                    _fifthPic = value;
                    OnPropertyChanged(() => FifthPic);
                }
            }
        }
        #endregion
        #region Property Callback
        private Action<ChoiceSelector> _callback;  
        public Action<ChoiceSelector> Callback
        {
            get { return _callback; }
            set
            {
                if (_callback != value)
                {
                    _callback = value;
                    OnPropertyChanged(() => Callback);
                }
            }
        }
        #endregion
        #region Property ChoiceNo
        private int _choiceNo;
        public int ChoiceNo
        {
            get { return _choiceNo; }
            set
            {
                if (_choiceNo != value)
                {
                    _choiceNo = value;
                    OnPropertyChanged(() => ChoiceNo);

                    switch (_choiceNo)
                    {
                        case 1:
                            FirstPic = Visibility.Visible;
                            SecondPic = Visibility.Collapsed;
                            ThirdPic = Visibility.Collapsed;
                            FourthPic = Visibility.Collapsed;
                            FifthPic = Visibility.Collapsed;
                            break;
                        case 2:
                            FirstPic = Visibility.Collapsed;
                            SecondPic = Visibility.Visible;
                            ThirdPic = Visibility.Collapsed;
                            FourthPic = Visibility.Collapsed;
                            FifthPic = Visibility.Collapsed;
                            break;
                        case 3:
                            FirstPic = Visibility.Collapsed;
                            SecondPic = Visibility.Collapsed;
                            ThirdPic = Visibility.Visible;
                            FourthPic = Visibility.Collapsed;
                            FifthPic = Visibility.Collapsed;
                            break;
                        case 4:
                            FirstPic = Visibility.Collapsed;
                            SecondPic = Visibility.Collapsed;
                            ThirdPic = Visibility.Collapsed;
                            FourthPic = Visibility.Visible;
                            FifthPic = Visibility.Collapsed;
                            break;
                        case 5:
                            FirstPic = Visibility.Collapsed;
                            SecondPic = Visibility.Collapsed;
                            ThirdPic = Visibility.Collapsed;
                            FourthPic = Visibility.Collapsed;
                            FifthPic = Visibility.Visible;
                            break;

                        default:
                            FirstPic = Visibility.Collapsed;
                            SecondPic = Visibility.Collapsed;
                            ThirdPic = Visibility.Collapsed;
                            FourthPic = Visibility.Collapsed;
                            break;
                    }
                }
            }
        }
        #endregion        
        #region Property IsSelected
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(() => IsSelected);
                }
            }
        }
        #endregion
        public string QuestionNo { get; set; }    
        public ChoiceViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
            }
        }
        public void Selected()
        {
            if (TestingData.State != "CheckAnswer")
            {
                //this.IsSelected = true;
            }

            if (Callback != null) Callback(Choice);
        }

    }
}
