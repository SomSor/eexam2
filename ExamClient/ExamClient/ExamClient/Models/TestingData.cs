using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;

namespace ExamClient.Models
{
    public enum StudentActivity
    {
        Instruction,
        Testing,
        Reviewing,
    }
    public static class TestingData
    {
        public static StudentActivity Activity;
        public static Timer Timer;
        public static Timer ReviewTimer;

        public static List<Answer> Answers;
        public static BitmapImage ProfilePhoto;
        public static BitmapImage MainLogo;
        public static BitmapImage CustomerLogo;
        public static Config Config;
        public static string SubjectCode;
        public static string SubjectName;
        public static int TestDuration { get; set; }
        public static PreExamResponse preExam;
        public static ExamSheetResponse sheet { get; set; }
        public static string State { get; set; }
        public static void Reset()
        {
            Timer = new Timer();
            ReviewTimer = new Timer();
            Answers = new List<Answer>();
            ProfilePhoto = new BitmapImage();
            preExam = new PreExamResponse();
            sheet = new ExamSheetResponse();
            Activity = StudentActivity.Instruction;
            SubjectCode = string.Empty;
            SubjectName = string.Empty;
            State = string.Empty;
            TestDuration = 0;
        }

        public static void SetTimer()
        {
            //var d = (int)sheet.ExamDuration * 60 * 1000;
            //Timer = new Timer(d);
            //Timer = new Timer(sheet.ExamDuration);

            TestDuration = sheet.ExamDuration * 60;
        }
    }
}
