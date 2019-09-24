using ExamClient.Models;
using ExamClient.Resources;
using ExamClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ExamClient.ViewModel
{
    public class ExamViewModel : MVVMBase
    {
        #region Exam
        public int currentQuestionNo;
        public bool IsFromBtn;

        #region Property Preview
        private string _preview;
        public string Preview
        {
            get
            { return _preview; }
            set
            {
                if (_preview != value)
                {
                    _preview = value;
                    OnPropertyChanged(() => Preview);
                }
            }
        }
        #endregion   
        #region Property CurrentSheet
        private ExamSheetResponse _currentSheet;
        public ExamSheetResponse CurrentSheet
        {
            get { return _currentSheet; }
            set
            {
                if (_currentSheet != value)
                {
                    _currentSheet = value;
                    OnPropertyChanged(() => CurrentSheet);
                }
            }
        }
        #endregion
        #region Property QuestionSelect
        private QuestionViewModel _questionSelect;
        public QuestionViewModel QuestionSelect
        {
            get { return _questionSelect; }
            set
            {
                if (_questionSelect != value)
                {
                    _questionSelect = value;
                    OnPropertyChanged(() => QuestionSelect);
                }
            }
        }
        #endregion
        #region Property ChoiceSelect
        private ChoiceSelector _choiceSelect;
        public ChoiceSelector ChoiceSelect
        {
            get { return _choiceSelect; }
            set
            {
                if (_choiceSelect != value)
                {
                    _choiceSelect = value;
                    OnPropertyChanged(() => ChoiceSelect);
                }
            }
        }
        #endregion
        #region Property First
        private ChoiceViewModel _first;
        public ChoiceViewModel First
        {
            get { return _first; }
            set
            {
                if (_first != value)
                {
                    _first = value;
                    OnPropertyChanged(() => First);
                }
            }
        }
        #endregion
        #region Property Second
        private ChoiceViewModel _second;
        public ChoiceViewModel Second
        {
            get { return _second; }
            set
            {
                if (_second != value)
                {
                    _second = value;
                    OnPropertyChanged(() => Second);
                }
            }
        }
        #endregion
        #region Property Third
        private ChoiceViewModel _third;
        public ChoiceViewModel Third
        {
            get { return _third; }
            set
            {
                if (_third != value)
                {
                    _third = value;
                    OnPropertyChanged(() => Third);
                }
            }
        }
        #endregion
        #region Property Fourth
        private ChoiceViewModel _fourth;
        public ChoiceViewModel Fourth
        {
            get { return _fourth; }
            set
            {
                if (_fourth != value)
                {
                    _fourth = value;
                    OnPropertyChanged(() => Fourth);
                }
            }
        }
        #endregion

        //#region Property ExamAns
        //private ExamAnsViewModel _examAns;
        //public ExamAnsViewModel ExamAns
        //{
        //    get { return _examAns; }
        //    set
        //    {
        //        if (_examAns != value)
        //        {
        //            _examAns = value;
        //            OnPropertyChanged(() => ExamAns);
        //        }
        //    }
        //}
        //#endregion

        #region Property CurrentQuestion
        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                if (_currentQuestion != value)
                {
                    _currentQuestion = value;
                    OnPropertyChanged(() => CurrentQuestion);
                }
            }
        }
        #endregion
        #region Property CurrentChoices
        private ChoiceSelector _currentChoices;
        public ChoiceSelector CurrentChoices
        {
            get { return _currentChoices; }
            set
            {
                if (_currentChoices != value)
                {
                    _currentChoices = value;
                    OnPropertyChanged(() => CurrentChoices);
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
                        MainExamVisibility = Visibility.Visible;
                        ExamAnsVisibility = Visibility.Collapsed;
                        SetCurrentQuestion(CurrentAnswer.AnswerNo);
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

                    if (!IsFromBtn)
                    {
                        OpenAnsDlg();

                    }
                    IsFromBtn = false;


                }
            }
        }
        #endregion

      

        #endregion Exam

        #region etc
        private OnsiteServices svc;
        public TimeSpan Duration { get; set; }
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

        #region Property BorderColor
        private System.Windows.Media.SolidColorBrush _borderColor;
        public System.Windows.Media.SolidColorBrush BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    OnPropertyChanged(() => BorderColor);
                }
            }
        }
        #endregion
        #region Property AnswerColor
        private System.Windows.Media.SolidColorBrush _answerColor;
        public System.Windows.Media.SolidColorBrush AnswerColor
        {
            get { return _answerColor; }
            set
            {
                if (_answerColor != value)
                {
                    _answerColor = value;
                    OnPropertyChanged(() => AnswerColor);
                }
            }
        }
        #endregion
        #region Property BtnChooseVisibility
        private Visibility _btnChooseVisibility;
        public Visibility BtnChooseVisibility
        {
            get { return _btnChooseVisibility; }
            set
            {
                if (_btnChooseVisibility != value)
                {
                    _btnChooseVisibility = value;
                    OnPropertyChanged(() => BtnChooseVisibility);
                }
            }
        }
        #endregion
        #region Property SendExamVisibility
        private Visibility _sendExamVisibility;
        public Visibility SendExamVisibility
        {
            get { return _sendExamVisibility; }
            set
            {
                if (_sendExamVisibility != value)
                {
                    _sendExamVisibility = value;
                    OnPropertyChanged(() => SendExamVisibility);
                }
            }
        }
        #endregion
        #region Property MainExamVisibility
        private Visibility _mainExamVisibility;
        public Visibility MainExamVisibility
        {
            get { return _mainExamVisibility; }
            set
            {
                if (_mainExamVisibility != value)
                {
                    _mainExamVisibility = value;
                    OnPropertyChanged(() => MainExamVisibility);
                }
            }
        }
        #endregion
        #region Property ExamAnsVisibility
        private Visibility _examAnsVisibility;
        public Visibility ExamAnsVisibility
        {
            get { return _examAnsVisibility; }
            set
            {
                if (_examAnsVisibility != value)
                {
                    _examAnsVisibility = value;
                    OnPropertyChanged(() => ExamAnsVisibility);
                }
            }
        }
        #endregion
        #region Property AlertResult
        private Visibility _alertResult;
        public Visibility AlertResult
        {
            get { return _alertResult; }
            set
            {
                if (_alertResult != value)
                {
                    _alertResult = value;
                    OnPropertyChanged(() => AlertResult);
                }
            }
        }
        #endregion
        #region Property State
        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                //if (_state != value)
                //{
                _state = value;
                OnPropertyChanged(() => State);
                //}
            }
        }
        #endregion
        #region Property Timer
        private DispatcherTimer _timer;
        public DispatcherTimer Timer
        {
            get { return _timer; }
            set
            {
                if (_timer != value)
                {
                    _timer = value;
                    OnPropertyChanged(() => Timer);
                }
            }
        }
        #endregion
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
        #region Property IsTimpUp
        private bool _isTimeUp;
        public bool IsTimpUp
        {
            get { return _isTimeUp; }
            set
            {
                if (_isTimeUp != value)
                {
                    _isTimeUp = value;
                    OnPropertyChanged(() => IsTimpUp);
                }
            }
        }
        #endregion    
        #region Events

        public static EventHandler<SendResultArgs> SendResultEvent;

        #endregion Events
        #endregion etc

        #region Constructors
        public ExamViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                svc = new OnsiteServices();
                CurrentSheet = new ExamSheetResponse();
                CurrentSheet = TestingData.sheet;
                IsTimpUp = false;
                State = TestingData.State;
            

                BorderColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));

                First = new ChoiceViewModel { Callback = SetCurrentChoice };
                Second = new ChoiceViewModel { Callback = SetCurrentChoice };
                Third = new ChoiceViewModel { Callback = SetCurrentChoice };
                Fourth = new ChoiceViewModel { Callback = SetCurrentChoice };
                QuestionSelect = new QuestionViewModel { Callback = SetQPreview };

                ChoiceSelect = new ChoiceSelector();

                SelectIndex = 0;
             
                currentQuestionNo = 1;
                CurrentAnswer = new Answer();
                Answers = new List<Answer>();
                InitializeAnswers();
                //MainExamVisibility = Visibility.Collapsed;

                //ExamAns = new ExamAnsViewModel();
                //ExamAns.Answers = Answers;
                QuestionSelect = new QuestionViewModel();

                #region TimeCal
                if (TestingData.State != "CheckAnswer" && this.IsTimpUp == false)
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
                            SendResult();
                            if (this.IsTimpUp)
                            {
                                SendResultEvent(this, new Models.SendResultArgs { IsTimeUp = true });
                                this.IsTimpUp = false;
                            }
                        }
                        Duration = Duration.Subtract(TimeSpan.FromSeconds(1));
                        TestingData.TestDuration = (int)Duration.TotalSeconds;
                        DisplayDuration = Duration.ToString();
                    };
                    Timer.Start();
                }
                #endregion TimeCal

                ProfilePhoto = TestingData.ProfilePhoto;
                FullName = TestingData.sheet.FirstName + "  " + TestingData.sheet.LastName;
                SubjectName = TestingData.sheet.SubjectName;
                TestingData.Activity = StudentActivity.Testing; // ART

                //if (Answers.Count() == 0)
                //{
                //    InitializeAnswers();
                //}

                //set current question  choice
                SetCurrentQuestion(currentQuestionNo);

                MainExamVisibility = Visibility.Visible;
                ExamAnsVisibility = Visibility.Collapsed;
                SendExamVisibility = Visibility.Collapsed;
                AlertResult = Visibility.Collapsed;
                BtnChooseVisibility = Visibility.Collapsed;

            }
        }
        #endregion Constructors  

        #region Methonds
        private void InitializeAnswers()
        {
            int num = 0;
            foreach (var item in TestingData.sheet.Questions)
            {
                var a = new Answer();

                num = num + 1;
                a.AnswerNo = num;
                if (item.UserAnswer != null)
                {
                    int CNO = 1;
                    switch (item.UserAnswer._id)
                    {
                        case "a":
                            CNO = 1;
                            break;
                        case "b":
                            CNO = 2;
                            break;
                        case "c":
                            CNO = 3;
                            break;
                        case "d":
                            CNO = 4;
                            break;

                        default:
                            CNO = 0;
                            break;
                    }

                    a.Choice = new ChoiceSelector
                    {
                        _id = item.UserAnswer._id,
                        Detail = item.UserAnswer.Detail,
                        ChoiceNo = CNO,
                        IsCorrect = item.UserAnswer.IsCorrect.Value,
                        IsSelected = false,
                        QuestionNo = item.QuestionNumber,

                    };

                }
                else
                {
                    a.Choice = new ChoiceSelector { ChoiceNo = 0 };
                }

                Answers.Add(a);
            }
            //for (int i = 0; i < TestingData.sheet.Questions.Count(); i++)
            //{
            //    var a = new Answer();
            //    a.AnswerNo = i + 1;
            //    a.Choice = new ChoiceSelector { ChoiceNo = 0 };
            //    Answers.Add(a);
            //}
            Answers[0].IsSelected = true;
            CurrentAnswer = Answers[0];
        }
        private void SetCurrentQuestion(int newQuestionNo)
        {
            currentQuestionNo = newQuestionNo;

            QuestionSelect = new QuestionViewModel();
            QuestionSelect.Question = CurrentSheet.Questions[currentQuestionNo - 1];

            QuestionViewModel q = new QuestionViewModel();

            q.Question = QuestionSelect.Question;
            q.Callback = SetQPreview;

            QuestionSelect = q;

            Preview = QuestionSelect.TextMD;
            BorderColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            BtnChooseVisibility = Visibility.Collapsed;

            var no = Answers.Where(x => x.AnswerNo == newQuestionNo).FirstOrDefault().Choice.ChoiceNo;

            SetChoice(no);
        }
        private void SetChoice(int chosenChoiceNo)
        {
            //this.choices = new ObservableCollection<ChoiceSelector>();
            int i = 1;
            if (QuestionSelect.Question.Choices.Count() > 0)
            {
                foreach (var item in QuestionSelect.Question.Choices)
                {
                    ChoiceSelector choice = new ChoiceSelector()
                    {
                        ChoiceNo = i,
                        _id = item._id,
                        Detail = item.Detail,
                        IsCorrect = false,
                        IsSelected = i == chosenChoiceNo,
                        QuestionNo = QuestionSelect.Question.QuestionNumber
                    };

                    if (TestingData.State == "CheckAnswer")
                    {
                        choice.IsCorrect = item.IsCorrect.HasValue ? item.IsCorrect.Value : false;
                        choice.IsSelected = item._id == QuestionSelect.Question.UserAnswer?._id;
                    }

                    ChoiceViewModel c = new ChoiceViewModel();

                    c.Choice = choice;
                    c.ChoiceNo = choice.ChoiceNo;
                    c.Callback = SetCurrentChoice;

                    if (choice.ChoiceNo == 1)
                    {
                        First = c;
                    }
                    else if (choice.ChoiceNo == 2)
                    {
                        Second = c;
                    }
                    else if (choice.ChoiceNo == 3)
                    {
                        Third = c;
                    }
                    else if (choice.ChoiceNo == 4)
                    {
                        Fourth = c;
                    }
                    i++;
                }
            }
        }
        public void CallAdmin()
        {

        }
        public void NextQuestion()
        {
            IsFromBtn = true;

            if (currentQuestionNo == CurrentSheet.Questions.Count())
            {
                currentQuestionNo = 1;
                SelectIndex = 0;
            }
            else
            {
                currentQuestionNo++;
                SelectIndex++;
            }

            SetCurrentQuestion(currentQuestionNo);

            //SetPreview(QuestionSelect.TextMD, System.Windows.Media.Color.FromRgb(0, 0, 0));
        }
        public void PreviousQuestion()
        {
            IsFromBtn = true;

            if (currentQuestionNo == 1)
            {
                currentQuestionNo = CurrentSheet.Questions.Count();
                SelectIndex = CurrentSheet.Questions.Count() - 1;
            }
            else
            {
                currentQuestionNo--;
                SelectIndex--;
            }
            SetCurrentQuestion(currentQuestionNo);
        }
        public void FirstAnswer()
        {
            if (SelectIndex != 0)
            {
                IsFromBtn = true;
                SelectIndex = 0;
                currentQuestionNo = 1;
                SetCurrentQuestion(currentQuestionNo);
            }
        }
        public void LastAnswer()
        {
            IsFromBtn = true;

            SelectIndex = 49;
            currentQuestionNo = 50;
            SetCurrentQuestion(currentQuestionNo);
        }
        public void NextNoChoice()
        {
            if (TestingData.State != "CheckAnswer")
            {
                IsFromBtn = true;

                var nextNotChooseAnswer = Answers.OrderBy(p => p.AnswerNo).Where(p => p.AnswerNo > CurrentAnswer.AnswerNo && p.AnswerNo != 0).FirstOrDefault(p => p.Choice.ChoiceNo == 0);
                if (nextNotChooseAnswer == null) // if search through the end, let go to first and search again
                    nextNotChooseAnswer = Answers.OrderBy(p => p.AnswerNo).Where(p => p.AnswerNo < CurrentAnswer.AnswerNo && p.AnswerNo != 0).FirstOrDefault(p => p.Choice.ChoiceNo == 0);
                if (nextNotChooseAnswer == null) // no unchoose answer
                    return;

                int newPos = Answers.IndexOf(nextNotChooseAnswer) + 1;


                SelectIndex = Answers.IndexOf(nextNotChooseAnswer);
                currentQuestionNo = newPos;
                SetCurrentQuestion(newPos);
            }
            else
            {
                //var nextWrongAnswer = Answers.OrderBy(p => p.AnswerNo).Where(p => p.AnswerNo > CurrentAnswer.AnswerNo && p.AnswerNo != 0).FirstOrDefault(p => p.Choice.IsCorrect == false);
                //if (nextWrongAnswer == null) // if search through the end, let go to first and search again
                //    nextWrongAnswer = Answers.OrderBy(p => p.AnswerNo).Where(p => p.AnswerNo < CurrentAnswer.AnswerNo && p.AnswerNo != 0).FirstOrDefault(p => p.Choice.IsCorrect == false);
                //if (nextWrongAnswer == null) // no unchoose answer
                //    return;

                //int newPos = Answers.IndexOf(nextWrongAnswer) + 1;

                //SelectIndex = Answers.IndexOf(nextWrongAnswer);
                //currentQuestionNo = newPos;
                //SetCurrentQuestion(newPos);
            }
        }
        public void ChooseByBtn()
        {
            //ตอบ     
            var answer = Answers.Where(x => x.AnswerNo == CurrentChoices.QuestionNo).FirstOrDefault();
            answer.Choice = CurrentChoices;

            AnswerRequest ans = new AnswerRequest
            {
                ExamSheetId = TestingData.sheet._id,
                ClientId = TestingData.Config.ClientId,
                Qid = QuestionSelect.Question._id,
                ChoiceId = CurrentChoices._id,
            };
            svc.Answer(ans);

            if (Answers.Where(x => x.Choice.ChoiceNo == 0).Count() == 0)
            {
                SendExamVisibility = Visibility.Visible;
                //LeftScrollVisibility = Visibility.Collapsed;
                //TODO ปุ่มส่งผลสอบกระพิบ
            }
            else
            {
                //ไปข้อถัดไป
                NextQuestion();
            }


        }
        public void SetQPreview()
        {
            Preview = QuestionSelect.TextMD;
            BorderColor = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            //CurrentChoice = new ChoiceSelector();
            BtnChooseVisibility = Visibility.Collapsed;
        }
        private string ConvertAnsNo(int i)
        {
            switch (i)
            {
                case 1:
                    return "ก";
                case 2:
                    return "ข";
                case 3:
                    return "ค";
                case 4:
                    return "ง";
                default:
                    return "-";
            }
        }
        public void SendResult()
        {
            var result = svc.SendExam(TestingData.sheet._id, TestingData.Config.ClientId);
            TestingData.sheet = result.ExamSheetRespones;

            TestingData.State = "CheckAnswer";
            if (Timer != null)
            {
                Timer.Stop();
            }

            this.IsTimpUp = true;
        }
        public void CloseSendResultDlg()
        {
            SendExamVisibility = Visibility.Collapsed;
            MainExamVisibility = Visibility.Visible;
        }
        public void CloseAlertResultDlg()
        {
            AlertResult = Visibility.Collapsed;
        }
        public void OpenSendResultDlg()
        {
            if (TestingData.State != "CheckAnswer")
            {
                SendExamVisibility = Visibility.Visible;
                MainExamVisibility = Visibility.Collapsed;
            }
            else
            {
                
            }
         
            //if (Answers.Where(x => x.Choice.ChoiceNo == 0).Count() == 0)
            //{
            //    SendExamVisibility = Visibility.Visible;
            //}
            //else
            //{
            //    if (TestingData.State != "CheckAnswer")
            //    {
            //        //MessageBox.Show("กรุณาทำข้อสอบให้ครบทุกข้อครับ");

            //        AlertResult = Visibility.Visible;
            //    }

            //}
        }

        public void OpenAnsDlg()
        {
            // visibility dlg
            MainExamVisibility = Visibility.Collapsed;
            ExamAnsVisibility = Visibility.Visible;
            //SelectIndex = -1;
        }
        public void PlayVoice()
        {
        }
        public void BackToTutorial()
        {
            //TestingData.State = "Resume";

            //Timer.Stop();
            //State = TestingData.State;
        }
        public void Answer()
        {
            AnswerRequest ans = new AnswerRequest
            {
                ExamSheetId = TestingData.sheet._id,
                ClientId = TestingData.Config.ClientId,
                Qid = CurrentQuestion._id,
                ChoiceId = CurrentQuestion.UserAnswer._id,
            };
            svc.Answer(ans);
        }

        public void SetCurrentChoice(ChoiceSelector choice)
        {

            ChoiceSelect = choice;
            //Preview = choice.TextMD;
            CurrentChoices = choice;

            switch (choice.ChoiceNo)
            {
                case 1:
                    SetPreview(choice.TextMD, System.Windows.Media.Color.FromRgb(206, 49, 68));
                    break;
                case 2:
                    SetPreview(choice.TextMD, System.Windows.Media.Color.FromRgb(255, 201, 14));
                    break;
                case 3:
                    SetPreview(choice.TextMD, System.Windows.Media.Color.FromRgb(34, 177, 76));
                    break;
                case 4:
                    SetPreview(choice.TextMD, System.Windows.Media.Color.FromRgb(0, 162, 232));
                    break;
                default:
                    SetPreview(choice.TextMD, System.Windows.Media.Color.FromRgb(0, 0, 0));
                    break;
            }
        }

        public void SetPreview(string textMd, System.Windows.Media.Color color)
        {

            BorderColor = new System.Windows.Media.SolidColorBrush(color);
            Preview = textMd;
            if (TestingData.State != "CheckAnswer")
                BtnChooseVisibility = Visibility.Visible;

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

        public void CloseExamAns()
        {
            MainExamVisibility = Visibility.Visible;
            ExamAnsVisibility = Visibility.Collapsed;
        }
        #endregion Methonds
    }
}
