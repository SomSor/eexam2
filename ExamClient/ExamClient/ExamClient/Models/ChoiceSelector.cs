using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class ChoiceSelector : Choice
    {
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
                //if (this.Choice != null)
                //{
                return Helpers.BrowserBehavior.GetMarkdownTemplate()
               .Replace("{contents}", new MarkdownSharp.Markdown().Transform(this.Detail));
                //                }
                //                else
                //                {
                //                    return Helpers.BrowserBehavior.GetMarkdownTemplate()
                //.Replace("{contents}", new MarkdownSharp.Markdown().Transform(""));

                //                }

            }
        }

        public int QuestionNo { get; set; }
    }
}
