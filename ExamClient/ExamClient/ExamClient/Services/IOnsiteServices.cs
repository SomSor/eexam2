using ExamClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Services
{
    public interface IOnsiteServices
    {
        PreExamResponse CheckExam(string pid);
        PicResponse SavePic(PicRequest picrequest);
        ExamSheetResponse GetSheet(string pid, string subjectCode, string clientid);
        ActiveResponse CheckActive();
        void Answer(AnswerRequest answer);    
        ResultResponse SendExam(string sheetid,string clientid);       
        void StartExam(ClientSheetRequest clientsheet);     
        void EndExam(ClientSheetRequest clientsheet);
    }
}
