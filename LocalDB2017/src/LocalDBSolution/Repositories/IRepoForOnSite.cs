using System;
using System.Collections.Generic;
using LocalDBSolution.ViewModels;

namespace LocalDBSolution.Repositories
{
    public interface IRepoForOnSite
    {
        Center GetCenter();
        MessageRespone Login(string user, string pwd, string centerid);
        IEnumerable<TestRegistration> ListTestRegis(string centerid);
        int GetTestRegisCount(string centerid);
        void Active(DateTime ActiceDateTime, string centerid);
        IEnumerable<TestRegistration> SearchTestRegis(string txt, string centerid);

        TestRegistration GetTestRegisInfo(string pid, string subjectCode);

        TestRegistration GetTestRegisById(string id);
        IEnumerable<TestRegistration> GetTestRegisByIds(List<string> ids);
        TestRegistration GetTestRegisInfoForPrintQR(string pid);
        IEnumerable<ExamSheet> GetResultInfo(string pid);
        ExamSheet GetTestResultInfo(string pid, string sheetId);
        IEnumerable<ExamSheet> ListExamData(string centerid);
        ActiveData GetActive(string centerid);
        bool CheckExamEnough(string centerid);

        IEnumerable<ExamSheet> GetMappedSheet(string centerid);
        IEnumerable<ExamSheet> ListTestingSheet(string centerid);

        void UpdateTestRegis(TestRegistration testRegis);

        void CloseTestRegis(List<TestRegistration> testRegisList);

        //for client api
        IEnumerable<TestRegistration> GetExamRegisInfo(string pid);
        ActiveData GetLastActive();
        ExamSheet GetSheetByPIDAndSubjectCode(string pid, string subjectCode, string contentlanguage);
        ExamSheet GetSheetBySubjectCode(string subjectCode);
        ExamSheet GetSheetBySubjectCodeForMap(string subjectCode, string contentlanguge);
        void UpdateSheet(ExamSheet sheet);

        ExamSheet GetSheetBySheetId(string sheetid);

        ClientMapResponse GetClientId(string uuid);

        void CloseSheet(List<ExamSheet> sheetList);
    }
}
