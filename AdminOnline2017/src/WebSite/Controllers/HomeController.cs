using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebSite.Repositories;
using WebSite.ViewModels.AdminOnlineModels;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private serverconfig svconfig;
        public static CenterDataRequest _centerdata = new CenterDataRequest();
        private readonly IRepoForRegistrationRepository repoRegis;

        public HomeController(IOptions<serverconfig> svconfig, IRepoForRegistrationRepository repoRegis)
        {
            this.svconfig = svconfig.Value;
            this.repoRegis = repoRegis;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login([FromBody]CenterDataRequest center)
        {
            //string URL = string.Format("http://10./api/Shared/DownloadLogo/{0}", centerid);
            using (var client = new WebClient())
            {
                try
                {
                    //Create Appoint To Onsiteoy
                    var jsonPreSheet = Newtonsoft.Json.JsonConvert.SerializeObject(center);

                    var dataByte = System.Text.Encoding.UTF8.GetBytes(jsonPreSheet);

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    //dataByte = client.UploadData("http://localhost/localdb/api/shared/createtestregistration", "POST", dataByte);
                    dataByte = client.UploadData(this.svconfig.admindbip + "/api/Shared/LoginToLocalDB/", "POST", dataByte);

                    //string URL = string.Format(this.svconfig.admindbip + "/api/Shared/DownloadLogo/{0}", centerid);
                    //var dataByte = client.DownloadData(URL);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction("");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult CreateTestRegistration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTestRegistration([FromForm]ViewModels.AdminOnlineModelsBack.TestRegistration testRegistration)
        {
            var center = repoRegis.GetCenterData(_centerdata._id);
            if (center == null)
            {
                TempData["errormessage"] = "ไม่พบข้อมูลศูนย์สอบ หรือล็อกอินหมดอายุ";
                return View(testRegistration);
            }

            var site = repoRegis.GetSiteData(center.SiteId);
            if (site == null)
            {
                TempData["errormessage"] = "ไม่พบข้อมูลศูนย์สอบ หรือล็อกอินหมดอายุ";
                return View(testRegistration);
            }

            var subject = repoRegis.GetActivatedSubjectByCode(testRegistration.SubjectCode);
            if (subject == null)
            {
                TempData["errormessage"] = "ไม่พบข้อมูลวิชา";
                return View(testRegistration);
            }

            var oldTestRegistration = repoRegis.ListTestRegisByPID(new List<string> { testRegistration.PID });
            if (oldTestRegistration != null && oldTestRegistration.Any(x => x.SubjectCode == testRegistration.SubjectCode))
            {
                TempData["errormessage"] = "มีข้อมูลการลงทะเบียนแล้ว";
                return View(testRegistration);
            }

            testRegistration._id = Guid.NewGuid().ToString();
            testRegistration.SubjectName = subject.SubjectName;
            testRegistration.ExamLanguage = "th";
            testRegistration.VoiceLanguage = "th";
            testRegistration.RegDate = DateTime.UtcNow;
            testRegistration.ExpiredDate = testRegistration.RegDate.AddDays(90);
            testRegistration.SiteId = site._id;
            testRegistration.CenterId = center._id;
            testRegistration.ForPractice = false;
            testRegistration.ForTestSystem = false;
            testRegistration.Status = "APPROVED";
            testRegistration.ExamStatus = "UNSEND";
            testRegistration.ExamPeriod = "all";
            testRegistration.AppointDate = testRegistration.RegDate;
            testRegistration.MaxCount = site.MaxTestCount;

            var testRegistrations = new List<ViewModels.AdminOnlineModelsBack.TestRegistration> { testRegistration };
            repoRegis.CreateTestRegis(testRegistrations);

            TempData["message"] = "ลงทะเบียนสำเร็จ";
            return View();
        }
    }
}
