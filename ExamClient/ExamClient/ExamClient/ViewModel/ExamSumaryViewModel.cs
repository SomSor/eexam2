using ExamClient.Models;
using ExamClient.Resources;
using ExamClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.ViewModel
{
    public class ExamSumaryViewModel : MVVMBase
    {
        #region Property Correct
        private string _correct;
        public string Correct
        {
            get { return _correct; }
            set
            {
                if (_correct != value)
                {
                    _correct = value;
                    OnPropertyChanged(() => Correct);
                }
            }
        }
        #endregion

        #region Property InCorrect
        private string _inCorrect;
        public string InCorrect
        {
            get { return _inCorrect; }
            set
            {
                if (_inCorrect != value)
                {
                    _inCorrect = value;
                    OnPropertyChanged(() => InCorrect);
                }
            }
        }
        #endregion

        #region Property Status
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(() => Status);
                }
            }
        }
        #endregion

        #region Property DisplayScore
        private string _disPlayScore;
        public string DisplayScore
        {
            get { return _disPlayScore; }
            set
            {
                if (_disPlayScore != value)
                {
                    _disPlayScore = value;
                    OnPropertyChanged(() => DisplayScore);
                }
            }
        }
        #endregion

        public ExamSumaryViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                Correct = TestingData.sheet.CorrectScore.ToString();
                InCorrect = TestingData.sheet.InCorrectScore.ToString();

                if (TestingData.sheet.CorrectScore >= TestingData.sheet.PassScore)
                {
                    Status = "คุณสอบผ่าน";
                }
                else
                {
                    Status = "คุณสอบตก";
                }


                DisplayScore = String.Format("{0} / {1 } ", Correct, InCorrect);

            }
        }

        public void EndExam()
        {
            OnsiteServices svc = new OnsiteServices();
            svc.EndExam(new ClientSheetRequest { ClientId = TestingData.Config.ClientId, SheetId = TestingData.sheet._id });

            TestingData.Reset();
        }
    }
}
