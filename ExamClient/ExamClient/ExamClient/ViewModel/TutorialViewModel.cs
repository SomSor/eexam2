using ExamClient.Models;
using ExamClient.Resources;
using ExamClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ExamClient.ViewModel
{
    public class TutorialViewModel : MVVMBase
    {
        #region Property DisplayDuration
        private string _displayDuration;
        public string DisplayDuration
        {
            get { return _displayDuration; }
            set
            {
                if (_displayDuration != value)
                {
                    _displayDuration = value;
                    OnPropertyChanged(() => DisplayDuration);
                }
            }
        }
        #endregion

        public DispatcherTimer Timer { get; set; }

        public TimeSpan Duration { get; set; }

        public TutorialViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                Duration = new TimeSpan(0, 0, TestingData.TestDuration);
                DisplayDuration = Duration.ToString();

                Timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };

                Timer.Tick += (sndr, se) =>
                {
                    if (Duration <= new TimeSpan(0, 0, 0))
                    {
                        Timer.Stop();
                    }

                    Duration = Duration.Subtract(TimeSpan.FromSeconds(1));
                    TestingData.TestDuration = (int)Duration.TotalSeconds;
                    DisplayDuration = Duration.ToString();
                };


                if (TestingData.TestDuration != 0)
                {
                    Timer.Start();
                }

            }
        }

        public void startQuiz(Action navigate)
        {
            //if (TestingData.sheet.ExamDuration != 0)
            //{
            //    Timer.Stop();
            //}

            OnsiteServices svc = new OnsiteServices();
            svc.StartExam(new ClientSheetRequest { ClientId = TestingData.Config.ClientId, SheetId = TestingData.sheet._id });

            if (TestingData.State == "READY")
            {
                TestingData.SetTimer();
            }

            TestingData.sheet.LastedStatus = "TESTING";
            TestingData.State = "TESTING";

            TestingData.Activity = StudentActivity.Testing;
            //if (TestingData.TestDuration == 0)
            //{
            //    //TestingData.sheet.ExamDuration = TestingData.sheet.ExamDuration;
            //    TestingData.TestDuration = TestingData.sheet.ExamDuration;
            //}

            navigate();
        }
    }
}
