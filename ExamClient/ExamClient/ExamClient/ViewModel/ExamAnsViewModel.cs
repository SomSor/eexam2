using ExamClient.Models;
using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ExamClient.ViewModel
{
    public class ExamAnsViewModel : MVVMBase
    {
        public BitmapImage ProfilePhoto { get; set; }
        #region Property FullName
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(() => FullName);
                }
            }
        }
        #endregion

        #region Property SubjectName
        private string _subjectName;
        public string SubjectName
        {
            get { return _subjectName; }
            set
            {
                if (_subjectName != value)
                {
                    _subjectName = value;
                    OnPropertyChanged(() => SubjectName);
                }
            }
        }
        #endregion

        #region Property Answers
        private List<Answer> _answers;
        public List<Answer> Answers
        {
            get
            {
                return _answers;
            }
            set
            {
                if (_answers != value)
                {
                    _answers = value;
                    OnPropertyChanged(() => Answers);
                }
            }
        }
        #endregion
        #region Property CurrentAnswer
        private Answer _currentAnswer;
        public Answer CurrentAnswer
        {
            get { return _currentAnswer; }
            set
            {
                if (_currentAnswer != value)
                {
                    _currentAnswer = value;
                    OnPropertyChanged(() => CurrentAnswer);

                    if (CurrentAnswer.AnswerNo != 0)
                    {
                        // SetCurrentQuestion(CurrentAnswer.AnswerNo);
                    }

                }
            }
        }
        #endregion
        #region Property SelectIndex
        private int _selectIndex;
        public int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                if (_selectIndex != value)
                {
                    _selectIndex = value;
                    OnPropertyChanged(() => SelectIndex);
                }
            }
        }
        #endregion
        public ExamAnsViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                ProfilePhoto = TestingData.ProfilePhoto;

                FullName = TestingData.sheet.FirstName + "  " + TestingData.sheet.LastName;
                SubjectName = TestingData.sheet.SubjectName;
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
