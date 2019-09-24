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

namespace ExamClient.ViewModel
{
    public class SmartCardViewModel : MVVMBase
    {
        #region Property ProfileVis
        private Visibility _profileVis;
        public Visibility ProfileVis
        {
            get { return _profileVis; }
            set
            {
                if (_profileVis != value)
                {
                    _profileVis = value;
                    OnPropertyChanged(() => ProfileVis);
                }
            }
        }
        #endregion

        #region Property SmartCardVis
        private Visibility _smartCardVis;
        public Visibility SmartCardVis
        {
            get { return _smartCardVis; }
            set
            {
                if (_smartCardVis != value)
                {
                    _smartCardVis = value;
                    OnPropertyChanged(() => SmartCardVis);
                }
            }
        }
        #endregion

        #region Property ErrorStatus
        private string _errorStatus;
        public string ErrorStatus
        {
            get { return _errorStatus; }
            set
            {
                if (_errorStatus != value)
                {
                    _errorStatus = value;
                    OnPropertyChanged(() => ErrorStatus);
                }
            }
        }
        #endregion

        #region Property ErrorVis
        private Visibility _errorVis;
        public Visibility ErrorVis
        {
            get { return _errorVis; }
            set
            {
                if (_errorVis != value)
                {
                    _errorVis = value;
                    OnPropertyChanged(() => ErrorVis);
                }
            }
        }
        #endregion

        #region Property ConfirmVis
        private Visibility _confirmVis;
        public Visibility ConfirmVis
        {
            get { return _confirmVis; }
            set
            {
                if (_confirmVis != value)
                {
                    _confirmVis = value;
                    OnPropertyChanged(() => ConfirmVis);
                }
            }
        }
        #endregion

        #region Property SubjectSelected
        private SubjectResponse _subjectSelected;
        public SubjectResponse SubjectSelected
        {
            get { return _subjectSelected; }
            set
            {
                if (_subjectSelected != value)
                {
                    _subjectSelected = value;
                    OnPropertyChanged(() => SubjectSelected);

                    if (Index != -5)
                    {
                        SmartCardVis = Visibility.Collapsed;
                        ErrorVis = Visibility.Collapsed;
                        ConfirmVis = Visibility.Visible;
                        ProfileVis = Visibility.Collapsed;
                    }
                }
            }
        }
        #endregion

        #region Property Index
        private int _intdex;
        public int Index
        {
            get { return _intdex; }
            set
            {
                if (_intdex != value)
                {
                    _intdex = value;
                    OnPropertyChanged(() => Index);
                }
            }
        }
        #endregion

        #region Property PreExamResponse
        private PreExamResponse _preExam;
        public PreExamResponse PreExam
        {
            get { return _preExam; }
            set
            {
                if (_preExam != value)
                {
                    _preExam = value;
                    OnPropertyChanged(() => PreExam);
                }
            }
        }
        #endregion

        public Action navigate { get; set; }

        public SmartCardViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                Index = -5;
                PreExam = new PreExamResponse();
                SubjectSelected = new SubjectResponse();


                SmartCardVis = Visibility.Visible;
                ErrorVis = Visibility.Collapsed;
                ConfirmVis = Visibility.Collapsed;
                ProfileVis = Visibility.Collapsed;
                ErrorStatus = "ข้อมูลผิดพลาดกรุณาติดต่อเจ้าหน้าที่";
            }
        }
        public void CheckExam(string pid, Action navigateMethod)
        {
            if (!String.IsNullOrWhiteSpace(pid))
            {
                try
                {
                    //pid = "1409901796428"; //HACK   

                    OnsiteServices svc = new OnsiteServices();
                    PreExam = svc.CheckExam(pid);
                    if (PreExam != null)
                    {
                        TestingData.preExam = new PreExamResponse();
                        TestingData.preExam = PreExam;

                        SmartCardVis = Visibility.Collapsed;
                        ErrorVis = Visibility.Collapsed;
                        ConfirmVis = Visibility.Collapsed;
                        ProfileVis = Visibility.Visible;

                        this.navigate = navigateMethod;
                    }
                    else
                    {
                        ErrorStatus = "ไม่พบข้อมูล";
                        SmartCardVis = Visibility.Collapsed;
                        ErrorVis = Visibility.Visible;
                        ConfirmVis = Visibility.Collapsed;
                        ProfileVis = Visibility.Collapsed;
                    }
                }
                catch (Exception e)
                {
                    SmartCardVis = Visibility.Collapsed;
                    ErrorVis = Visibility.Visible;
                    ConfirmVis = Visibility.Collapsed;
                    ProfileVis = Visibility.Collapsed;
                }
            }
            else
            {
                ErrorVis = Visibility.Visible;
                ErrorStatus = "PID NOT FOUND";
            }

        }
        public void SelectedSubjectCode()
        {
        }
        public void CloseError()
        {

            SmartCardVis = Visibility.Visible;
            ErrorVis = Visibility.Collapsed;
            ConfirmVis = Visibility.Collapsed;
            ProfileVis = Visibility.Collapsed;

        }
        public void CancelConfrim()
        {
            SmartCardVis = Visibility.Collapsed;
            ErrorVis = Visibility.Collapsed;
            ConfirmVis = Visibility.Collapsed;
            ProfileVis = Visibility.Visible;
        }
        public void SubmitConfrim()
        {
            //get sheet

            try
            {
                OnsiteServices svc = new OnsiteServices();

                var sheet = svc.GetSheet(PreExam.PID, SubjectSelected.SubjectCode, TestingData.Config.ClientId);
                TestingData.sheet = sheet;

                TestingData.State = "READY";

                this.navigate();

            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}
