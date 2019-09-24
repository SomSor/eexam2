using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebSite.ViewModels.AdminOnlineModels;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private serverconfig svconfig;
        public static CenterDataRequest _centerdata = new CenterDataRequest();
        public HomeController(IOptions<serverconfig> svconfig)
        {
            this.svconfig = svconfig.Value;
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
    }
}
