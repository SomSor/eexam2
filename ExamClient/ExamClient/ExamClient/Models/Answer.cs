using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExamClient.Models
{
    public class Answer : MVVMBase
    {
        public int AnswerNo { get; set; }     

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

                    if (IsSelected)
                    {
                        SelectedVisibility = Visibility.Visible;
                    }
                    else
                    {
                        SelectedVisibility = Visibility.Collapsed;
                    }
                }
            }
        }
        #endregion

        #region Property SelectedVisibility
        private Visibility _selectedVisibility;
        public Visibility SelectedVisibility
        {
            get { return _selectedVisibility; }
            set
            {
                if (_selectedVisibility != value)
                {
                    _selectedVisibility = value;
                    OnPropertyChanged(() => SelectedVisibility);
                }
            }
        }
        #endregion       
    }
}
