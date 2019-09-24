using System;
using System.Collections.Generic;
using WebSite.ViewModels.AdminOnlineModelsBack;

namespace WebSite.Repositories
{
    public interface IRepoForRegistrationRepository
    {
        IEnumerable<TestRegistration> ListTestRegisByID(List<string> id);

        void UpdateAppointStatus(IEnumerable<TestRegistration> appointTestRegis);

        IEnumerable<TestRegistration> ListTestRegisByDate(string centerId, DateTime date);

        IEnumerable<TestRegistration> SearchTestRegis(string centerId, string txt);

        IEnumerable<TestRegistration> ListForAproved(string centerId);

        //IEnumerable<TestRegistration> ListResultTestRegis(string centerId);

        List<DateTime> ListAppointDate(string centerId);

        void UpdateLatestStatus(IEnumerable<TestRegistration> testRegis);

        Center GetCenterData(string centerId);

        Site GetSiteData(string siteId);

        void CreateTestRegis(IEnumerable<TestRegistration> testRegis);

        IEnumerable<TestRegistration> ListTestRegisByPID(List<string> PID);
    }
}
