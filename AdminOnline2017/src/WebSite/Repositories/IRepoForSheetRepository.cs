using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminOnlineModels = WebSite.ViewModels.AdminOnlineModels;
using AdminOnlineModelsBack = WebSite.ViewModels.AdminOnlineModelsBack;

namespace WebSite.Repositories
{
    public interface IRepoForSheetRepository
    {
        void CreatePreExamSheet(List<AdminOnlineModelsBack.ExamSheet> ExamSheets);
        void UpdateExamSheet(List<AdminOnlineModelsBack.ExamSheet> ExamSheets);
        IEnumerable<AdminOnlineModelsBack.ExamSheet> ListExamSheetByID(List<string> id);
        List<AdminOnlineModelsBack.OccupationGroup> ListOccupationGroup(string centerid);
        List<AdminOnlineModelsBack.ExamSheet> GetTestResultByDate(string centerid, DateTime testdate);
        /// <summary>
        /// ALL PASS AND FAIL
        /// </summary>
        /// <param name="centerid"></param>
        /// <returns></returns>
        IEnumerable<AdminOnlineModelsBack.ExamSheet> ListExamSheetForSendTo3rd(string centerid);

        /// <summary>
        /// ALL PASS
        /// </summary>
        /// <param name="centerid"></param>
        /// <returns></returns>
        IEnumerable<AdminOnlineModelsBack.ExamSheet> ListPassExamSheetForSendTo3rd(string centerid);

        /// <summary>
        /// ALL FAIL
        /// </summary>
        /// <param name="centerid"></param>
        /// <returns></returns>
        IEnumerable<AdminOnlineModelsBack.ExamSheet> ListFailExamSheetForSendTo3rd(string centerid);

    }
}
