using ExamClient.Models;
using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.ViewModel
{
    public class QuestionViewModel : MVVMBase
    {
        #region Property Question
        private Question _question;
        public Question Question
        {
            get { return _question; }
            set
            {
                if (_question != value)
                {
                    _question = value;
                    OnPropertyChanged(() => Question);
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

        public string TextMD
        {
            get
            {
                if (this.Question != null)
                {
                    return Helpers.BrowserBehavior.GetMarkdownTemplate()
.Replace("{contents}", new MarkdownSharp.Markdown().Transform(this.Question.Detail));
                }
                else
                {
                    return Helpers.BrowserBehavior.GetMarkdownTemplate()
    .Replace("{contents}", new MarkdownSharp.Markdown().Transform(""));

                }

            }
        }


        #region Property Callback
        private Action _callback;
        public Action Callback
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


        public QuestionViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                //IsSelected = true;
            }
        }

        public void Selected()
        {
            if (Callback != null) Callback();
        }
    }
}
